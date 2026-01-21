using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEB_DOMAIN.Config.Authentication;

namespace WEB_SERVICES.DTO.Generic
{
    public class UserDto : IBaseDto
    {
        public Guid UserId { get; set; }
        [JsonIgnore]
        public Guid Id => UserId;
        public Guid RoleId { get; set; }
        public AuthDto? Auth { get; set; }
        public UserInfoDto? UserInfo { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }


    }
}
