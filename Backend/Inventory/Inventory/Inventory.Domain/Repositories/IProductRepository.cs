using Application.Common.Responses;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        public Task<(int,string)> DeleteProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int productId);
        Task<int> UpdateProductAsync(Product product);
        Task<bool> UpdateProductQuantity(List<(string GlobalProductId,int Quantity)> values);
        Task<IEnumerable<Product>> GetFilteredProductsAsync(string searchTerm);
    }
}
