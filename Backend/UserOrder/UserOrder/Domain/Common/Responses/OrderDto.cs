using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Responses;
using UserOrder.Domain.Model;

namespace UserOrder.Domain.Common.Responses
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public DateTime OrderDate {  get; set; }
        public string OrderStatus { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
