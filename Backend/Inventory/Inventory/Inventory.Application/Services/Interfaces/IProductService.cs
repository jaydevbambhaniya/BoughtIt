using Application.Commands;
using Application.Common.Responses;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddProductAsync(AddProductCommand request);
        Task<ApiResponse<(int, string)>> DeleteProductByIdAsync(int productId);
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<ApiResponse<int>> UpdateProductAsync(UpdateProductCommand request);
        Task<IEnumerable<ProductDto>> GetProductListAsync();
        Task<ApiResponse<IEnumerable<ProductDto>>> GetFilteredProductsAsync(string searchTerm);
    }
}
