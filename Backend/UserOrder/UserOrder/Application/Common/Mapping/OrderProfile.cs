using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Commands;
using UserOrder.Domain.Common.Responses;
using UserOrder.Domain.Model;

namespace UserOrder.Application.Common.Mapping
{
    public class OrderProfile :Profile
    {
        public OrderProfile() {
            CreateMap<PlaceOrderCommand, Order>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Pending"));
            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<UpdateOrderItemsCommand, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src=>src.OrderItemId));  
            CreateMap<OrderItemsCommand, OrderItem>();
            CreateMap<Product,ProductDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest=>dest.Product,opt=>opt.MapFrom(src=>src.Product));
            CreateMap<Order, OrderDto>()
                .ForMember(dest=> dest.OrderStatus,opt=>opt.MapFrom(src=>src.Status))
                .ForMember(dest=>dest.OrderDate,opt=> opt.MapFrom(src=>src.CreatedDate));
        }
    }
}
