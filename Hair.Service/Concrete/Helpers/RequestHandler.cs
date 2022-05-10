using Hair.Service.Abstract.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Hair.Service.Handler
{
    public class AuthorizationRequirement : IAuthorizationRequirement { }
    public class RequestHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IDataAccessService _dataAccessService;
        public RequestHandler(IDataAccessService dataAccessService)
        {
            _dataAccessService = dataAccessService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            var cntx = context.Resource as HttpContext;

            var cntxRoutes = cntx?.Request.RouteValues;
            var controllerName = cntxRoutes?["controller"]?.ToString();
            var actionName = cntxRoutes?["action"]?.ToString();
            var areaName = cntxRoutes?["area"]?.ToString();
            var pageName = cntxRoutes?["page"]?.ToString();
            var bearer = cntx?.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault(x => x.Contains("Bearer"));
            
            if (bearer != null && bearer.Contains("Bearer"))
            {
                var token = bearer.Split(" ")[1];
                var Grant = await _dataAccessService.CheckUserGrantFromToken(token, controllerName, actionName);
                
                if (Grant)
                    context.Succeed(requirement);
                else
                {
                    cntx.Response.StatusCode = 401;
                    await cntx.Response.WriteAsync("Access Denied");
                    context.Fail();
                    return;
                }
                context.Succeed(requirement);
            }
            else
            {
                cntx.Response.StatusCode = 401;
                await cntx.Response.WriteAsync("Access Denied");
            }
        }
    }


}