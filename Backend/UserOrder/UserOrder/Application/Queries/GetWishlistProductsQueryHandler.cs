using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Queries
{
    public class GetWishlistProductsQuery :IRequest<ApiResponse<IList<int>>>
    {
        public int UserId { get; set; }
    }
    public class GetWishlistProductsQueryHandler : IRequestHandler<GetWishlistProductsQuery, ApiResponse<IList<int>>>
    {
        private readonly IProductService _productService;
        public GetWishlistProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ApiResponse<IList<int>>> Handle(GetWishlistProductsQuery request, CancellationToken cancellationToken)
        {
            var response = await _productService.GetWishlistProductsFromCacheAsync(request.UserId);
            return new ApiResponse<IList<int>>() { Data = response };
        }
    }
}
