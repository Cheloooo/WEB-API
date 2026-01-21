using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Generic;

public class Role : BaseEntity, IEntity
{
    public Guid RoleId { get; set; }
    public Guid Id => RoleId;
    public string RoleName { get; set; }
    public ICollection<User> Users { get; set; }

}