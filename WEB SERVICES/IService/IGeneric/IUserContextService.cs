using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_SERVICES.IService.Generic
{
    public class IUserContextService
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string AccesesToken { get; set; }
        public string IpAddress { get; set; }
    }
}
