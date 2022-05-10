using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Hair.Service.Handler
{
    public static class ExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = context.Features
                                .Get<IExceptionHandlerPathFeature>()
                                .Error;

                        var response = new {

                            //error = exception.Source,
                            message = exception.Message,
                        };

                        //Log.Error($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });

            //* Bu kod direk program.cs içine de yazılabilir

            //app.UseExceptionHandler(c => c.Run(async context => {
            //    var exception = context.Features
            //        .Get<IExceptionHandlerPathFeature>()
            //        .Error;
            //    var response = new { error = exception.Message };

            //    await context.Response.WriteAsJsonAsync(exception.Message);
            //}));                     
        }
    }
}
