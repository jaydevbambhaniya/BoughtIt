using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Queries
{
    public record GetOrderQuery : IRequest<ApiResponse<OrderDto>>
    {
        public int OrderID { get; set; }
    }
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, ApiResponse<OrderDto>>
    {
        private readonly IOrderService _orderService;
        public GetOrderQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<ApiResponse<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var response = await _orderService.GetOrderByIdAsync(request.OrderID);
            return new ApiResponse<OrderDto>() { Data = response };
        }
    }
}
