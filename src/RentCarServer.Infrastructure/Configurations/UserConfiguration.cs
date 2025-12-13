using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Users;

namespace RentCarServer.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.OwnsOne(user => user.FirstName);
        builder.OwnsOne(user => user.LastName);
        builder.OwnsOne(user => user.FullName);
        builder.OwnsOne(user => user.Email);
        builder.OwnsOne(user => user.Username);
        builder.OwnsOne(user => user.Password);
        
    }
}