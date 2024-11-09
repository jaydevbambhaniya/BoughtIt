using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Commands
{
    public class UpdateWishlistCommand : IRequest<ApiResponse<bool>>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool IsAdd { get; set; } // If false then delete
    }
    public class UpdateWishlistCommandHandler : IRequestHandler<UpdateWishlistCommand, ApiResponse<bool>>
    {
        private readonly IProductService _productService;
        public UpdateWishlistCommandHandler(IProductService productService) {
            _productService = productService;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateWishlistCommand request, CancellationToken cancellationToken)
        {
            var resp = await _productService.UpdateWishlistAsync(request.UserId, request.ProductId,request.IsAdd);
            return new ApiResponse<bool>() { Data = resp };
        }
    }
}
