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
    public interface IOrderRepository
    {
        Task<PlacedOrderDto> PlaceOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int OrderID);
        Task<IList<Order>> GetAllOrdersByUserIdAsync(int UserID);
        Task<int> DeleteOrderAsync(int OrderID);
        Task<Order> UpdateOrderAsync(Order order);
    }
}
