using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WebApi.Application.AppSettings;
using WebApi.DataAccess;

namespace WebApi.Api.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var config = Configuration.GetConfiguration<AppSettings>();

            var builder = new DbContextOptionsBuilder<DatabaseContext>();

            builder.UseSqlServer(config.ConnectionStrings.Primary, x => x.MigrationsAssembly(typeof(DatabaseContext).Assembly.GetName().Name)).UseLazyLoadingProxies();

            return new DatabaseContext(builder.Options);
        }
    }
}
