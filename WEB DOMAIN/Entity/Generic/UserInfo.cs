using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Entity.Generic;

public class UserInfo: BaseEntity, IEntity
{
    public Guid UserInfoId { get; set; }

    public Guid Id => UserInfoId;
    public Guid UserId {get; set;}
    public string FirstName {get; set;}
    public string MiddleName {get; set;}
    public string LastName {get; set;}
    public string Email {get; set;}
    public string ContactNumber {get; set;}
    public User User {get; set;}
}