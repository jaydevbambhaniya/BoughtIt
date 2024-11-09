using Application.Common.Responses;
using Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public record AddProductCommand : IRequest<ApiResponse<int>>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public double Price { get; set; }
        public int AvailableQuantity { get; set; }
        public IFormFile? ProductImage { get; set; }
    }
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ApiResponse<int>>
    {
        private readonly IProductService _productService;
        private readonly IMessageBroker _messageBroker;
        public AddProductCommandHandler(IProductService productService, IMessageBroker messageBroker)
        {
            _productService = productService;
            _messageBroker = messageBroker;
        }
        public async Task<ApiResponse<int>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.AddProductAsync(request);
            if (product != null)
            {
                bool response = await _messageBroker.PublishMessageAsync("Products",product,"ProductAdded");
                Console.WriteLine("Message Produced:" +response);
            }
            else
            {
                return new ApiResponse<int>() { StatusCode = -1, Message = "Unable to create product!" };
            }
            return new ApiResponse<int>() {StatusCode=200,Data=product.ProductId,Message="Product added successfully!" };
        }
    }
}
