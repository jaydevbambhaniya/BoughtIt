using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductQuantityUpdateDto>> GetGlobalProductId(List<string> productIds);
        public Task<IList<CartProductDto>> GetCartProductsFromCacheAsync(int UserId);
        public Task<bool> UpdateCartAsync(int UserId,CartProductDto product, bool isAdd);
        public Task<IList<int>> GetWishlistProductsFromCacheAsync(int UserId);
        public Task<bool> UpdateWishlistAsync(int UserId, int ProductId,bool isAdd);
        public Task<bool> EmptyUserCart(int UserId);
    }
}
