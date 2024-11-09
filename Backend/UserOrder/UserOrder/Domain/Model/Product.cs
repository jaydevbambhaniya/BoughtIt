using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Model
{
    public class Product :BaseEntity
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string GlobalProductId { get; set; }
        public int AvailableQuantity { get; set; }
        public string FileName {  get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Product() { }
    }
}
