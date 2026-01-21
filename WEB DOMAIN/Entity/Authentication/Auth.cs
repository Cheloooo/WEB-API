using WEB_DOMAIN.Entity.Generic;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Authentication;

public class Auth : IEntity
{
    public Guid AuthId { get; set; }
    public Guid Id => AuthId;
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime LastLogin { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}