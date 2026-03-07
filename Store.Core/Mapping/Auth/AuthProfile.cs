using AutoMapper;
using Store.Core.DTOs.Auth;
using Store.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Mapping.Auth
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
