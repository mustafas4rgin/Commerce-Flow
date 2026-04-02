using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.BuildingBlocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommerceFLow.Services.Auth.Infrastructure.Configurations;

public sealed class RoleConfiguration : BaseEntityConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(64);
    }
}