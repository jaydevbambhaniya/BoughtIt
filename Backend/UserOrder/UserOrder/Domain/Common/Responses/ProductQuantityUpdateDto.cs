using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Common.Responses
{
    public class ProductQuantityUpdateDto
    {
        public int ProductId { get; set; }
        public string? GlobalProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
