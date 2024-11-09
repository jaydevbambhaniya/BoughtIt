using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Common.Responses
{
    public class CartProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
