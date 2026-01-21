using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using WEB_DOMAIN.Entity.Generic;
using WEB_SERVICES.DTO.Generic;
using WEB_SERVICES.IService.Generic;

namespace WEB_SERVICES.MappingProfile.Generic
{
    class GetSessionResolver : IValueResolver<UserDto, User, string>
    {
        public readonly IUserContextService _userContextService;
        public GetSessionResolver(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }
        public string Resolve(UserDto source, User destination, string destMember, ResolutionContext context)
        {
            return _userContextService.UserId;

        }
    }
}
