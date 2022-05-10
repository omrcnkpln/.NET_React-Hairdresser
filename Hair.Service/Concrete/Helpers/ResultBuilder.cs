using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hair.Service.Concrete.Helpers
{
    public class ResultBuilder<T> : ControllerBase
    {
        public IActionResult ActionResult;

        public ResultBuilder(HttpStatusCode StatusCode, T data, string message)
        {
            if (StatusCode == HttpStatusCode.OK)
            {
                //Log.Information("Response Turned On Controller", data);
                ActionResult = Ok(data);
            }
            else if (StatusCode == HttpStatusCode.NoContent)
            {
                //Log.Information("No Content On Controller");
                ActionResult = NoContent();
            }
            else if (StatusCode == HttpStatusCode.NotFound)
            {
                //Log.Information("No Content Found On Controller");
                ActionResult = NotFound(message);
            }
            else
            {
                //Log.Information("Bad Request On Controller");
                ActionResult = BadRequest(message);
            }
        }

        public IActionResult Go()
        {
            return ActionResult;
        }
    }
}
