using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEB_DOMAIN.Entity.Generic;

namespace WEB_DOMAIN.Config.Generic;

public class UserInfoConfig : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasOne(p => p.User)
            .WithOne(p => p.UserInfo)
            .HasForeignKey<UserInfo>(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}