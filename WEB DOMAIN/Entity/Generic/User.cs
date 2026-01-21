using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Generic;

public class User : BaseEntity ,IEntity
{
    public Guid UserId { get; set; }

    public Guid Id => UserId;
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public Auth Auth { get; set; }
    public UserInfo UserInfo { get; set; }
}