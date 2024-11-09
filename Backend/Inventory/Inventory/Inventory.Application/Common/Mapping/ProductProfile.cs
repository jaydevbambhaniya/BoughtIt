using Application.Commands;
using Application.Common.Responses;
using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapping
{
    public class ProductProfile :Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<AddProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
