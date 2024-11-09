using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Queries
{
    public class GetCartProductsQuery : IRequest<ApiResponse<IList<CartProductDto>>>
    {
        public int UserId { get; set; }
    }
    public class GetCartProductsQueryHandler : IRequestHandler<GetCartProductsQuery, ApiResponse<IList<CartProductDto>>>
    {
        private readonly IProductService _productService;   
        public GetCartProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ApiResponse<IList<CartProductDto>>> Handle(GetCartProductsQuery request, CancellationToken cancellationToken)
        {
            var productIds = await _productService.GetCartProductsFromCacheAsync(request.UserId);
            return new ApiResponse<IList<CartProductDto>>() { Data = productIds };
        }
    }
}
