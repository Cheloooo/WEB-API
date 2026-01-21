using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WEB_DOMAIN.Entity.Generic;
using WEB_SERVICES.DTO.Generic;

namespace WEB_SERVICES.MappingProfile.Generic
{
    public class IgnoreAuthInClientMapping : IMappingAction<UserDto, User>
    {
        public void Process(UserDto source, User destination, ResolutionContext context)
        {
            try
            {
                var ignore = context.Items.TryGetValue("IgnoreAuth", out var value) ? (bool)value : false;
                if (ignore)
                {
                    destination.Auth = null;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    
    }
}
