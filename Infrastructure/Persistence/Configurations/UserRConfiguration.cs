using Domain.Users;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserRConfiguration : IEntityTypeConfiguration<UserR>
{
    public void Configure(EntityTypeBuilder<UserR> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasConversion(u => u.Value, u => new UserRId(u));

        builder.Property(u => u.FirstName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(u => u.LastName).IsRequired().HasColumnType("varchar(255)");
        builder.Property(u => u.FullName).IsRequired().HasColumnType("varchar(255)");
    }
}