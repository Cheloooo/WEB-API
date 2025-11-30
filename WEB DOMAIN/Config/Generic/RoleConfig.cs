using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEB_DOMAIN.Entity.Generic;

namespace WEB_DOMAIN.Config.Generic;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(b => b.RoleID);
        builder.Property(r => r.RoleName)
            .IsRequired()
            .HasMaxLength(50);
    }
    
}