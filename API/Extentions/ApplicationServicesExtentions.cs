using Core.Interfaces;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Infrastructure.Repositories;
using Infrastructure.Repositories;



namespace API.Extensions
{
    public static class ApplicationServicesExtensions 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Repo
             services.AddScoped<IUnitOfWork, UnitOfWork>(); 
             services.AddScoped<IUserRepository, UserRepository>();
             services.AddScoped<IWarehouseRepository, WarehouseRepository>();
             services.AddScoped<IRoleRepository, RoleRepository>();

           // Register Repo Service
            services.AddScoped<IAuthService, AuthService>();              
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWarehouseService, WarehouseService>();


    


            // Register logging
            services.AddLogging();
            return services;
        } 
    }
}
