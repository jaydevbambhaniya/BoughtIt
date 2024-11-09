using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;
using UserOrder.Domain.Repository;

namespace UserOrder.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BoughtItDbContext _boughtItDbContext;
        public ProductRepository(BoughtItDbContext boughtItDbContext)
        {
            _boughtItDbContext = boughtItDbContext;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            product.ProductId = 0;
            await _boughtItDbContext.Products.AddAsync(product);
            return await _boughtItDbContext.SaveChangesAsync()!=0;
        }

        public async Task<bool> DeleteProductAsync(string globalProductId)
        {
            var product = _boughtItDbContext.Products.Where(prd => prd.GlobalProductId == globalProductId).FirstOrDefault();
            if (product == null)
            {
                return false;
            }
            _boughtItDbContext.Products.Remove(product);
            return await _boughtItDbContext.SaveChangesAsync() != 0;
        }
        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existingProduct = await _boughtItDbContext.Products.FirstOrDefaultAsync(p => p.GlobalProductId == product.GlobalProductId);
            if (existingProduct == null)
            {
                return false;
            }
            product.ProductId = existingProduct.ProductId;
            _boughtItDbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            return await _boughtItDbContext.SaveChangesAsync() != 0;
        }
        public async Task<List<ProductQuantityUpdateDto>> GetProductQuantity(IEnumerable<string> globalProductIds)
        {
            var data = await _boughtItDbContext.Products.Where(p=> globalProductIds.Contains(p.GlobalProductId))
                .Select(p=> new ProductQuantityUpdateDto() {ProductId=p.ProductId,GlobalProductId= p.GlobalProductId,ProductQuantity=p.AvailableQuantity}).ToListAsync();
            return data;
        }
    }
}
