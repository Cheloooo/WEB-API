using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WEB_DOMAIN.Config.Authentication;
using WEB_DOMAIN.Entity.Authentication;

namespace WEB_SERVICES.MappingProfile.Authentication
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<Auth, AuthDto>();
            CreateMap<AuthDto, Auth>();
        }
        

    }
}
