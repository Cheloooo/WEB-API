using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Generic;

public class Role : BaseEntity, IEntity
{
    public Guid RoleID { get; set; }

    public Guid Id => UserId;
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    public Role Role {get; set;}
    public Auth Auth { get; set; }
    public object RoleName { get; set; }
}