using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Domain.Model;

namespace UserOrder.Domain.Common.Responses
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public ProductDto Product {  get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }   
    }
}
