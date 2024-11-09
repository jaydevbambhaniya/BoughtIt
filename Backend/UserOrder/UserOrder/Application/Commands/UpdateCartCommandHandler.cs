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
    public class UpdateCartCommand : IRequest<ApiResponse<bool>>
    {
        public int UserId { get; set; }
        public CartProductDto CartProducts { get; set; }
        public bool IsAdd { get; set; } // if false then delete
    }
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ApiResponse<bool>>
    {
        private readonly IProductService _productService;
        public UpdateCartCommandHandler(IProductService productService) {
            _productService = productService;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var response = await _productService.UpdateCartAsync(request.UserId, request.CartProducts,request.IsAdd);
            return new ApiResponse<bool> { Data=response };
        }
    }
}
