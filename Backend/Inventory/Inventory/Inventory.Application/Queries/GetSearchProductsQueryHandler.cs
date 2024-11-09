using Application.Common.Responses;
using Application.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public record GetSearchProductsQuery : IRequest<ApiResponse<IEnumerable<ProductDto>>>
    {
        public string SearchTerm { get; set; }
    }
    public class GetSearchProductsQueryHandler : IRequestHandler<GetSearchProductsQuery, ApiResponse<IEnumerable<ProductDto>>>
    {
        private readonly IProductService _productService;
        public GetSearchProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ApiResponse<IEnumerable<ProductDto>>> Handle(GetSearchProductsQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.SearchTerm);
            return await _productService.GetFilteredProductsAsync(request.SearchTerm);
        }
    }
}
