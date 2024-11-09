using Application.Common.Responses;
using Application.Services.Interfaces;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>> { }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductService _productService;
        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return _productService.GetProductListAsync();
        }
    }
}
