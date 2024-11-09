using Application.Common.Responses;
using Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public record UpdateProductCommand : IRequest<ApiResponse<int>>
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
        public int AvailableQuantity { get; set; }
        public string? GlobalProductId { get; set; }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<int>>
    {
        private readonly IProductService _productService;
        private readonly IMessageBroker _messageBroker;
        public UpdateProductCommandHandler(IProductService productService, IMessageBroker messageBroker)
        {
            _productService = productService;
            _messageBroker = messageBroker;
        }
        public async Task<ApiResponse<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var response =  await _productService.UpdateProductAsync(request);
            if(response.Data<=0)return response;
            bool result = await _messageBroker.PublishMessageAsync("Products", request, "ProductUpdated");
            Console.WriteLine("Message Produced: "+result);
            return response;
        }
    }
}
