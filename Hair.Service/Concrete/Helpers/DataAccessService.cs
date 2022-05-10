using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Hair.Core.Models.System;
using Hair.Repository.Abstract;
using Hair.Service.Abstract.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SMBOM.Service.Concrete.DataAccess
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;

        public DataAccessService(IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        //Checks Jwt Is Valid and turns user id 
        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.GetSection("JwtKey").Value);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                //var logicRef = Guid.Parse(jwtToken.Claims.First(x => x.Type == "logicalRef").Value);

                return userId;
            }
            catch
            {
                return 0;
            }
        }
        
        public async Task<bool> CheckUserGrantFromToken(string Token, string ControllerName, string ActionName)
        {
            var userId = GetUserIdFromToken(Token);
            return await CheckAccess(userId, ControllerName, ActionName);
        }

        public async Task<bool> CheckAccess(int UserID, string Controller, string Action)
        {
            if (!Controller.Contains("Controller"))
                Controller += "Controller";
            try
            {
                var page = await _unitOfWork.Page.GetAsync(x => x.ActionName == Action && x.ControllerName == Controller);
                var findedUser = await _unitOfWork.User.GetAsync(x => x.Id == UserID);
                var userRole = await _unitOfWork.UserRole.GetAsync(x => x.UserId == findedUser.Id);
                var role = await _unitOfWork.Role.GetAsync(x => x.Id == userRole.RoleId);

                if (page == null || role == null)
                {
                    return false;
                }
                else
                {
                    return await _unitOfWork.RoleMenu.AnyAsync(x => x.Page == page && x.Role == role);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Page> ReadAllPAges()
        {
            var manifestName = Assembly.GetEntryAssembly().ManifestModule.Name;
            var assemblyPath = Path.Combine(_hostingEnvironment.ContentRootPath + "obj\\Debug\\net6.0\\" + manifestName);
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            
            return assembly.GetTypes()
           .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
           .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
           .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
           .Select(x => new Page
           {
               ControllerName = x.DeclaringType?.Name,
               ActionName = x.Name,

               //    Name = x.GetCustomAttribute<ShowOption>()?.Name,
           })
           .OrderBy(x => x.ControllerName).ThenBy(x => x.ActionName).ToList();
        }
    }
}