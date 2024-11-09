using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Commands;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PlacedOrderDto> PlaceOrderAsync(PlaceOrderCommand order);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<IList<OrderDto>> GetAllOrdersByUserIdAsync(int userId);
        Task<ApiResponse<int>> DeleteOrderAsync(int orderId);
        Task<OrderDto> UpdateOrderAsync(UpdateOrderCommand order);
    }
}
