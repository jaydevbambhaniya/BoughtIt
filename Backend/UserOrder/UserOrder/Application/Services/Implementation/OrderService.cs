using AutoMapper;
using UserOrder.Application.Commands;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;
using UserOrder.Domain.Repository;

namespace UserOrder.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public OrderService(IOrderRepository orderRepository,IMapper mapper, IProductService productService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productService = productService;
        }

        public async Task<ApiResponse<int>> DeleteOrderAsync(int orderId)
        {
            int ret = await _orderRepository.DeleteOrderAsync(orderId);
            string message = "Order Deleted Successfully.";
            if (ret == 0) {
                message = "Unable to delete order!";
            }else if (ret == -1)
            {
                message = "No order found for given OrderID!";
            }
            return new ApiResponse<int>() { StatusCode = 200,Data=ret, Message = message };
        }

        public async Task<IList<OrderDto>> GetAllOrdersByUserIdAsync(int userId)
        {
            var response =  await _orderRepository.GetAllOrdersByUserIdAsync(userId);
            return _mapper.Map<IList<OrderDto>>(response);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var respone =  await _orderRepository.GetOrderByIdAsync(orderId);
            return _mapper.Map<OrderDto>(respone);
        }

        public async Task<PlacedOrderDto> PlaceOrderAsync(PlaceOrderCommand command)
        {
            var order = _mapper.Map<Order>(command);
            var response = await _orderRepository.PlaceOrderAsync(order);
            if (response.OrderID > 0)
            {
                await _productService.EmptyUserCart(order.UserId);
            }
            return response;
        }

        public async Task<OrderDto> UpdateOrderAsync(UpdateOrderCommand order)
        {
            Order order1 = _mapper.Map<Order>(order);   
            var response = await _orderRepository.UpdateOrderAsync(order1);
            return _mapper.Map<OrderDto>(response);
        }
    }
}
