using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Repository;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Domain.Model;
using UserOrder.Domain.Repository;
using UserOrder.Infrastructure.Repositories;
using UserOrder.Infrastructure.Consumers;
using UserOrder.Application.Services.Interfaces;
using UserOrder.Application.Services.Implementation;

namespace Infrastructure.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BoughtItDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BoughtItConnection") ??
                    throw new InvalidOperationException("Connection string not found"));
            });
            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<BoughtItDbContext>();
            services.AddTransient<IUserRepository,UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddHostedService<ProductConsumer>();
            return services;
        }
    }
}
