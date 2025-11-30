using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEB_DOMAIN.Entity.Generic;

namespace WEB_DOMAIN.Config.Generic;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x =>  x.UserId);
        builder.HasOne(x => x.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
    }
}