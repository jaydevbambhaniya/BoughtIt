using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Commands
{
    public record PlaceOrderCommand : IRequest<PlacedOrderDto>
    {   
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IEnumerable<OrderItemsCommand> OrderItems { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }

    }
    public record OrderItemsCommand
    {
        public string GlobalProductId { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
    }
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlacedOrderDto>
    {
        private readonly IOrderService _orderService;
        private readonly IMessageBroker _messageBroker;
        private readonly IProductService _productService;
        public PlaceOrderCommandHandler(IOrderService orderService,IMessageBroker messageBroker,IProductService productService)
        {
            _orderService = orderService;
            _messageBroker = messageBroker;
            _productService= productService;
        }
        public async Task<PlacedOrderDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var response = await _orderService.PlaceOrderAsync(request);
            if (response.OrderID > 0)
            {
                var productIds = request.OrderItems.Select(oi=>oi.GlobalProductId).ToList();
                var products = await _productService.GetGlobalProductId(productIds);
                var data = products.Select(pd =>
                {
                    return new { pd.GlobalProductId, Quantity = pd.ProductQuantity };
                }).ToList();
                bool result = await _messageBroker.PublishMessageAsync("Inventory", data, "QuantityUpdate");
                Console.WriteLine($"Message Produced: ${result}");
            }
            return response;
        }
    }
}
