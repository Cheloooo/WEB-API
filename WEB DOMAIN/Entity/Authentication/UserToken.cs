using WEB_DOMAIN.Entity.Generic;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Authentication;

public class UserToken: IEntity
{
    public Guid TokenId  { get; set; }
    public Guid Id => TokenId;
    public Guid UserId  { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public string AccessTokenJti { get; set; }
    public DateTime IssuedAt { get; set; }
    public bool IsRevoked { get; set; }
    public string DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public DateTime LastAccessUtc { get; set; }
    public User User { get; set; }
}