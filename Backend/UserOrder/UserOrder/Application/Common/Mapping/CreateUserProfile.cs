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
    public class CreateUserProfile :Profile
    {
        public CreateUserProfile() {
            CreateMap<CreateUserCommand, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<GoogleUserInfo, User>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src=>"Other"))
                .ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.Email))
                .ForMember(dest=>dest.PasswordHash,opt=>opt.MapFrom(src=>""))
                .ForMember(dest=>dest.IsExternalLogin,opt=>opt.MapFrom(src=>true));
            CreateMap<ExternalLoginCommand, GoogleUserInfo>();
        }
    }
}
