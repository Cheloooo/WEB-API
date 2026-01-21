using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using WEB_DOMAIN.Entity.Generic;
using WEB_SERVICES.DTO.Generic;

namespace WEB_SERVICES.MappingProfile.Generic
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom<GetSessionResolver>())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Auth, opt => opt.MapFrom(src => src.Auth))
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo))
                .AfterMap<IgnoreAuthInClientMapping>(); //sanitation step once the mapping is done we remove a sensitive object before its return

            CreateMap<UserInfoDto, UserInfo>()
                .ForMember(dest => dest.UserInfoId, opt => opt.MapFrom(src => Guid.Empty == src.UserInfoId ? Guid.Empty : src.UserInfoId))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.MiddleName) ? string.Empty : src.MiddleName));

            CreateMap<UserInfo, UserInfoDto>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Auth, opt => opt.Ignore())
                .ForMember(dest => dest.UserInfo, opt => opt.MapFrom(src => src.UserInfo));



        }
    }
}
