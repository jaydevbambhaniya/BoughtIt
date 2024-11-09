using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Application.Services.Interfaces;

namespace UserOrder.Application.Commands
{
    public record DeleteOrderCommand : IRequest<ApiResponse<int>>
    {
        public int OrderID { get; set; }
    }
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResponse<int>>
    {
        private readonly IOrderService _orderService;
        public DeleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<ApiResponse<int>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            return await _orderService.DeleteOrderAsync(request.OrderID);
        }
    }
}
