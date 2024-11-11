using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(user => user.PersonalPhone).IsUnique();
        builder.HasIndex(user => user.WorkPhone).IsUnique();
        builder.HasIndex(user => user.WorkEmailAddress).IsUnique();
    }
}