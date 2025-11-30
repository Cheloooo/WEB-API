using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WEB_DOMAIN.Entity.Authentication;
using WEB_DOMAIN.Interface;

namespace WEB_DOMAIN.Config.Authentication;

public class AuthConfig: IEntityTypeConfiguration<Auth>
{
    public void Configure(EntityTypeBuilder<Auth> builder)
    {
        builder.HasKey(a => a.AuthId);
        builder.HasOne(a => a.User)
            .WithOne(u => u.Auth)
            .HasForeignKey<Auth>(a => a.UserId);
        //identify foreign key on auth
    }
}