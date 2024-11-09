using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;

namespace UserOrder.Application.Commands
{
    public record UpdateOrderCommand : IRequest<OrderDto>
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public IEnumerable<UpdateOrderItemsCommand> OrderItems { get; set; }
        public string Status {  get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
    public record UpdateOrderItemsCommand
    {
        public int OrderItemId { get; set; }
        public string GlobalProductId { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderService _orderService;
        public UpdateOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var updatedOrder = await _orderService.UpdateOrderAsync(request);
            return updatedOrder;
        }
    }
}
