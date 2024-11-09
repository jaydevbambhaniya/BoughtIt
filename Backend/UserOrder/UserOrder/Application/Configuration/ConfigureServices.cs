using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserOrder.Application.Services.Implementation;
using UserOrder.Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using UserOrder.Application.Validators;
using StackExchange.Redis;

namespace Application.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddValidatorsFromAssemblyContaining<AuthUserCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IUserService,UserService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IMessageBroker, MessageBroker>();
            services.AddMemoryCache();
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnectionString")));
            services.AddHttpClient();   
            return services;
        }
    }
}
