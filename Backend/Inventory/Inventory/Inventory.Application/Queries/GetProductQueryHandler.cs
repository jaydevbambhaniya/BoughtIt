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
    public record GetProductQuery : IRequest<ProductDto>
    {
        public int ProductID { get; set; }
    }
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductService _productService;
        public GetProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _productService.GetProductByIdAsync(request.ProductID);
        }
    }
}
