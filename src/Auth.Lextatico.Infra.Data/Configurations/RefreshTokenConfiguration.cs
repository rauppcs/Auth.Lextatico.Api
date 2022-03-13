using Auth.Lextatico.Domain.Models;
using Auth.Lextatico.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Lextatico.Infra.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.DefineDefaultFields();

            builder.Property(refreshToken => refreshToken.Token)
                .HasColumnType("VARCHAR(32)")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.TokenExpiration)
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.HasIndex(refreshToken => new { refreshToken.Token, refreshToken.TokenExpiration });

            builder.HasOne(refreshToken => refreshToken.ApplicationUser)
                .WithMany(applicationUser => applicationUser.RefreshTokens)
                .HasForeignKey(refreshToken => refreshToken.ApplicationUserId);
        }
    }
}
