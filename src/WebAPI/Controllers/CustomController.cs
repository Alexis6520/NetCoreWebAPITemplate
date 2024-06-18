using Microsoft.AspNetCore.Mvc;
using Services.Wrappers;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class CustomController : ControllerBase
    {
        protected ObjectResult CustomResult(Result result)
        {
            var body = result.Succeeded ? null : result;
            return StatusCode((int)result.StatusCode, body);
        }

        protected ObjectResult CustomResult<T>(Result<T> result)
        {
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
