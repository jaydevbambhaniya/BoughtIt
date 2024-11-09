using Domain.Repositories;
using Infrastructure.Consumers;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddDbContext<BoughtItDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BoughtIt2Connection") ??
                    throw new ArgumentNullException("Connection string not found!"));
            });
            services.AddTransient<IFileRepository>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var baseDirectory = configuration["FileStorage:BaseDirectory"];
                return new FileRepository(baseDirectory);
            });
            services.AddHostedService<InventoryConsumer>();
            return services;
        }
    }
}
