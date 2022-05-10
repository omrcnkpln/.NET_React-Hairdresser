using Hair.Service.Abstract.Helpers;
using Hair.Service.Abstract.UserService;
using Hair.Service.Concrete.UserService;
using Hair.Service.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SMBOM.Service.Concrete.DataAccess;

namespace Hair.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RequestHandler>();
            services.AddScoped<IDataAccessService, DataAccessService>();

            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IRoleMenuService, RoleMenuService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IUserRoleService, UserRoleService>();
            //services.AddScoped<IPageService, PageService>();

            return services;
        }
    }
}