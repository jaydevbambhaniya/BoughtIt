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
    public record DeleteProductCommand : IRequest<ApiResponse<(int, string)>>
    {
        public int ProductId { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResponse<(int,string)>>
    {
        private readonly IProductService _productService;
        private readonly IMessageBroker _messageBroker;
        public DeleteProductCommandHandler(IProductService productService,IMessageBroker messageBroker)
        {
            _productService = productService;
            _messageBroker = messageBroker;
        }
        public async Task<ApiResponse<(int,string)>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var response = await _productService.DeleteProductByIdAsync(request.ProductId);
            if (response.Data.Item1 <= 0)
            {
                return response;
            }
            bool result = await _messageBroker.PublishMessageAsync("Products", response.Data.Item2,"ProductDeleted");
            Console.WriteLine($"Message Produced - {result}");
            return response;
        }
    }
}
