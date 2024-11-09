using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(options=>options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient<IProductService, ProductService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IMessageBroker, MessageBroker>();
            return services;
        }
    }
}
