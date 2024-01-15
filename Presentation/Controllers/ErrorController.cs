using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("/errors/{code}")]
    public class ErrorController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new BaseResponse(success: false, statusCode: code));
        }
    }
}
