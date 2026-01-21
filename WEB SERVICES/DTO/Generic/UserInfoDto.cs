
using System.Diagnostics.Contracts;

namespace WEB_SERVICES.DTO.Generic
{
    public class UserInfoDto : BaseInfoDto
    {
        public string Firstname { get; set; }
        public string? MiddleName { get; set; }
        public string Lastname { get; set; }
    }

    public class BaseInfoDto
    {
        public Guid? UserInfoId { get; set; }
        public Guid? UserId { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        

    }
}
