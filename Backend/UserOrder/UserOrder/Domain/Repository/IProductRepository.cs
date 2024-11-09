using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;

namespace UserOrder.Domain.Repository
{
    public interface IProductRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(string globalProductId);
        Task<bool> UpdateProductAsync(Product product);
        Task<List<ProductQuantityUpdateDto>> GetProductQuantity(IEnumerable<string> productIds);
    }
}
