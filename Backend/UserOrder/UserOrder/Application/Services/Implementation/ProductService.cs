using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Repository;

namespace UserOrder.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cache;
        public ProductService(IProductRepository productRepository,ICacheService cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<bool> UpdateCartAsync(int UserId, CartProductDto product, bool IsAdd)
        {
            var products = await GetCartProductsFromCacheAsync(UserId);
            if (products == null) products = new List<CartProductDto>();
            var existingProduct = products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct != null && IsAdd)
            {
                existingProduct.Quantity = product.Quantity;
            }
            else if (existingProduct != null && !IsAdd)
            {
                products.Remove(existingProduct);
            }
            else
            {
                if (IsAdd)
                {
                    products.Add(product);
                }
            }
            var cacheKey = $"CartProducts:{UserId}";
            return await _cache.Set<IList<CartProductDto>>(cacheKey, products, 24 * 60 * 30);
        }

        public async Task<IList<CartProductDto>> GetCartProductsFromCacheAsync(int UserId)
        {
            var cacheKey = $"CartProducts:{UserId}";
            var products = await _cache.Get<IList<CartProductDto>>(cacheKey);
            return products;
        }

        public async Task<bool> EmptyUserCart(int UserId)
        {
            var cacheKey = $"CartProducts:{UserId}";
            return await _cache.Delete(cacheKey);
        }
        public async Task<List<ProductQuantityUpdateDto>> GetGlobalProductId(List<string> productIds)
        {
            return await _productRepository.GetProductQuantity(productIds);
        }

        public async Task<IList<int>> GetWishlistProductsFromCacheAsync(int UserId)
        {
            var cacheKey = $"WishlistProducts:{UserId}";
            var products = await _cache.Get<IList<int>>(cacheKey);
            return products;
        }

        public async Task<bool> UpdateWishlistAsync(int UserId, int ProductId, bool IsAdd)
        {
            var products = await GetWishlistProductsFromCacheAsync(UserId);
            if(products == null)products = new List<int>();
            if (products.Any(p => p == ProductId) && IsAdd)
            {
                return true;
            }
            if (IsAdd)
            {
                products.Add(ProductId);
            }
            else
            {
                products.Remove(ProductId);
            }
            var cacheKey = $"WishlistProducts:{UserId}";
            return await _cache.Set<IList<int>>(cacheKey, products,24*60*30);
        }
    }
}
