using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using FitLog.DataAccess;
using FitLog.Application.AppSettings;

namespace FitLog.Api.Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FitLogContext>
    {
        public FitLogContext CreateDbContext(string[] args)
        {
            var config = Configuration.GetConfiguration<AppSettings>();

            var builder = new DbContextOptionsBuilder<FitLogContext>();

            builder.UseSqlServer(config.ConnectionStrings.Primary, x => x.MigrationsAssembly(typeof(FitLogContext).Assembly.GetName().Name)).UseLazyLoadingProxies();

            return new FitLogContext(builder.Options);
        }
    }
}
