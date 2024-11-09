using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Model
{
    public class OrderItem : BaseEntity
    {
        public int Id { get; set; }
        public string GlobalProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; } = 0;

        public OrderItem() { }  
    }
}
