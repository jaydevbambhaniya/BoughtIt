using Application.Commands;
using Application.Common.Responses;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common.Resources;
using Domain.Model;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddProductAsync(AddProductCommand request)
        {
            var product = _mapper.Map<Product>(request);
            return _mapper.Map<ProductDto>(await _productRepository.AddProductAsync(product));
        }

        public async Task<ApiResponse<(int,string)>> DeleteProductByIdAsync(int productId)
        {
            var response = await _productRepository.DeleteProductByIdAsync(productId);
            return new ApiResponse<(int,string)>() { StatusCode = 200,Data=response, Message = response.Item1 > 0 ? "Product Deleted":"Unable to Delete" };
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetFilteredProductsAsync(string searchTerm)
        {
            var data = _mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetFilteredProductsAsync(searchTerm));
            return new ApiResponse<IEnumerable<ProductDto>>() { Data = data };
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var response = await _productRepository.GetProductByIdAsync(productId);
            if(response is null)
            {
                return null;
            }
            return _mapper.Map<ProductDto>(response);
        }

        public async Task<IEnumerable<ProductDto>> GetProductListAsync()
        {
            var response = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(response);
        }

        public async Task<ApiResponse<int>> UpdateProductAsync(UpdateProductCommand request)
        {
            var product = _mapper.Map<Product>(request);
            var response = await _productRepository.UpdateProductAsync(product);
            string msg = "Product Updated!";
            if (response == 0)
            {
                msg = "Unable to Update the product";
            }
            else if (response == ErrorCodes.GetError("PRODUCT_NOT_FOUND").Code)
            {
                msg = $"Product with Product ID {product.ProductId} does not exists";
            }
            return new ApiResponse<int>() { StatusCode = 200,Data=response, Message = msg };

        }
    }
}
