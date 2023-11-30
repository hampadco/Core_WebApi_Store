using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using shop.Core.Infrastructure;
using shop.Data.ApplicationContext;
using shop.Data.Persistent.Dapper;
using shop.Data.Repository;


namespace shop.Data.Infrastructure
{
    public class DataBaseStartUp : IApplicationStartup
    {
        public MiddleWarePriority Priority => MiddleWarePriority.AboveNormal;
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = "Data Source=arthur.iran.liara.ir,34334;Initial Catalog=myDB;User Id=sa;Password=ayzcpOW3Hm0vEYffKuJO1BWf;MultipleActiveResultSets=true;TrustServerCertificate=true";
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddDbContextPool<IApplicationContext,SqlServerApplicationContext>((options) =>
            {

                options.UseSqlServer(connectionString).UseLazyLoadingProxies();
            }, poolSize: 16);

            services.AddTransient(_ => new DapperContext(connectionString));
        }

        public void Configure(IApplicationBuilder app)
        {

        }
    }
}
