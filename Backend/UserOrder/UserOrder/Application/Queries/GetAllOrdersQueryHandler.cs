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
    public record GetAllOrdersQuery : IRequest<ApiResponse<IList<OrderDto>>>
    {
        public int UserID { get; set; }
    }
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResponse<IList<OrderDto>>>
    {
        private readonly IOrderService _orderService;
        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<ApiResponse<IList<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = await _orderService.GetAllOrdersByUserIdAsync(request.UserID);
            return new ApiResponse<IList<OrderDto>>() { Data = response };
        }
    }
}
