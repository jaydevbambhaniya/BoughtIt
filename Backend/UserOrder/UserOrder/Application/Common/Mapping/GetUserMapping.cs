using AutoMapper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Commands;
using UserOrder.Domain.Common.Responses;

namespace UserOrder.Application.Common.Mapping
{
    public class GetUserMapping:Profile
    {
        public GetUserMapping() {
            CreateMap<User, UserDto>();
            CreateMap<UpdateUserCommand, UserDto>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.UserId)).ReverseMap();
            CreateMap<RefreshTokenCommand, TokenDto>().ReverseMap();
        } 
    }
}
