using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;

namespace UserOrder.Domain.Common.Responses
{
    public class PlacedOrderDto
    {
        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
    }
}
