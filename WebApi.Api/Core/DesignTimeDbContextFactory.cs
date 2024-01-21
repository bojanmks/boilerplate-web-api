using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using WebApi.DataAccess;
using WebApi.Application.AppSettings;

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
