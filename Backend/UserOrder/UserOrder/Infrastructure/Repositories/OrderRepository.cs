using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;
using UserOrder.Domain.Repository;

namespace UserOrder.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BoughtItDbContext boughtItDbContext;
        public OrderRepository(BoughtItDbContext boughtItDbContext)
        {
            this.boughtItDbContext = boughtItDbContext;
        }

        public async Task<int> DeleteOrderAsync(int OrderID)
        {
            var order = boughtItDbContext.Orders.Where(ord=>ord.OrderId == OrderID).FirstOrDefault();
            if (order == null)
            {
                return -1; // No record found
            }
            boughtItDbContext.Remove(order);
            return await boughtItDbContext.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetAllOrdersByUserIdAsync(int UserID)
        {
            var response = await boughtItDbContext.Orders.Where(ord => ord.UserId==UserID)
                                                   .Include(ord=>ord.OrderItems)
                                                   .ThenInclude(oi=>oi.Product)
                                                   .ToListAsync();
            return response;
        }

        public async Task<Order> GetOrderByIdAsync(int OrderID)
        {
            return await boughtItDbContext.Orders.Where(ord => ord.OrderId == OrderID)
                                                          .Include(ord => ord.OrderItems)
                                                          .ThenInclude(oi=>oi.Product)
                                                          .FirstAsync();
        }

        public async Task<PlacedOrderDto> PlaceOrderAsync(Order order)
        {
            await boughtItDbContext.Orders.AddAsync(order);
            var products = boughtItDbContext.Products
            .AsEnumerable()
            .Join(order.OrderItems.AsEnumerable(),
                p => p.GlobalProductId,
                oi => oi.GlobalProductId,
                (p, oi) => new { Product = p, UpdateQuantity = p.AvailableQuantity - oi.Quantity });

            foreach( var product in products)
            {
                product.Product.AvailableQuantity = product.UpdateQuantity;
            }
            int ret = await boughtItDbContext.SaveChangesAsync();
            if (ret == 0)
            {
                return new PlacedOrderDto() { OrderID = -1, OrderStatus = "Failed"};
            }
            return new PlacedOrderDto() { OrderID = order.OrderId, OrderStatus = "Pending" };
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            var existingOrder = await boughtItDbContext.Orders.Include(ord=>ord.OrderItems)
                                            .ThenInclude(oi=>oi.Product)
                                            .FirstOrDefaultAsync(ord=>ord.OrderId==order.OrderId);  
            if(existingOrder == null)throw new Exception($"No Order found for given OrderId: {order.OrderId}");

            boughtItDbContext.Entry(existingOrder).CurrentValues.SetValues(order);
            Console.WriteLine(existingOrder);
            var productDictionary = existingOrder.OrderItems
                                    .ToDictionary(item => item.Product.GlobalProductId, item => item.Product);

            foreach (var item in order.OrderItems)
            {
                var existingItem = existingOrder.OrderItems.FirstOrDefault(oi=>oi.Id==item.Id);
                if(existingItem == null)
                {
                    item.OrderId = order.OrderId;
                    existingOrder.OrderItems.Add(item);
                    var product = boughtItDbContext.Products.Where(p=>p.GlobalProductId==item.GlobalProductId).FirstOrDefault();
                    product.AvailableQuantity = product.AvailableQuantity - item.Quantity;
                }
                else
                {
                    existingItem.Product.AvailableQuantity += existingItem.Quantity-item.Quantity;
                    boughtItDbContext.Entry(existingItem).CurrentValues.SetValues(item);
                    existingItem.OrderId= order.OrderId;
                }
            }
            foreach(var item in existingOrder.OrderItems)
            {
                if (!order.OrderItems.Any(oi => oi.Id == item.Id))
                {
                    if (productDictionary.ContainsKey(item.GlobalProductId))
                    {
                        productDictionary[item.GlobalProductId].AvailableQuantity += item.Quantity;
                    }
                    boughtItDbContext.OrdersItems.Remove(item);
                }
            }
            await boughtItDbContext.SaveChangesAsync();
            await boughtItDbContext.Entry(existingOrder).ReloadAsync();
            await boughtItDbContext.Entry(existingOrder).Collection(ord=>ord.OrderItems).LoadAsync();
            return existingOrder;
        }
    }
}
