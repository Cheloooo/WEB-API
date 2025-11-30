using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEB_DOMAIN.Entity.Authentication;

namespace WEB_DOMAIN.Config.Authentication;

public class UserTokenConfig: IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(t => t.TokenId);
        builder.Property(t => t.RefreshToken)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(t => t.DeviceInfo)
            .HasMaxLength(256);
        
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}