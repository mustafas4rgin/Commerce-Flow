using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Services.Auth.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommerceFLow.Services.Auth.Infrastructure.Configurations;

public sealed class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(u => u.FirstName)
            .HasMaxLength(64);
        
        builder.Property(u => u.LastName)
            .HasMaxLength(64);
        
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        builder.Property(u => u.PasswordSalt)
            .IsRequired();
        
    }
}