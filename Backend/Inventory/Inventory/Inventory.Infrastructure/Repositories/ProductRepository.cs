using Application.Common.Responses;
using Domain.Common.Resources;
using Domain.Model;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly BoughtItDbContext _boughtItDbContext;
        private readonly IFileRepository _fileRepository;
        public ProductRepository(BoughtItDbContext boughtItDbContext, IFileRepository fileRepository)
        {
            _boughtItDbContext = boughtItDbContext;
            _fileRepository = fileRepository;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            product.GlobalProductId = Guid.NewGuid().ToString();
            product.FileName = await _fileRepository.UploadFileAsync(product.ProductImage,"image",2*1024*1024);
            await _boughtItDbContext.Products.AddAsync(product);
            int x = await _boughtItDbContext.SaveChangesAsync();
            return product;
        }

        public async Task<(int,string?)> DeleteProductByIdAsync(int productId)
        {
            var product = _boughtItDbContext.Products.Where(prd=>prd.ProductId == productId).FirstOrDefault();
            if (product == null)
            {
                return (ErrorCodes.GetError("PRODUCT_NOT_FOUND").Code,"");
            }
            await _fileRepository.DeleteFileAsync(product.FileName);
            _boughtItDbContext.Products.Remove(product);
            int ret= await _boughtItDbContext.SaveChangesAsync();
            return (ret,product.GlobalProductId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _boughtItDbContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(string searchTerm)
        {
            Console.WriteLine(searchTerm);
            return await _boughtItDbContext.Products.Where(p=>p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .AsNoTracking().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _boughtItDbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            var existingProduct = await _boughtItDbContext.Products.FirstOrDefaultAsync(p=>p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                return ErrorCodes.GetError("PRODUCT_NOT_FOUND").Code;
            }
            _boughtItDbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            return await _boughtItDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductQuantity(List<(string GlobalProductId, int Quantity)> values)
        {
            var globalProductIds = values.Select(v=>v.GlobalProductId).ToList();
            var products = await _boughtItDbContext.Products.Where(p => globalProductIds.Contains(p.GlobalProductId)).ToListAsync();
            foreach (var product in products)
            {
                product.AvailableQuantity = values.FirstOrDefault(v=>v.GlobalProductId==product.GlobalProductId).Quantity;
            }
            return await _boughtItDbContext.SaveChangesAsync() != 0;
        }
    }
}
