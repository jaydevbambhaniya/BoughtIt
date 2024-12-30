using Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Domain.Model
{
    public class Order :BaseEntity
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public string Status { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public Order() { }
    }
}
