using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ProjectHr.EntityFrameworkCore
{
    public static class ProjectHrDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ProjectHrDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ProjectHrDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}
