using CommerceFlow.Shared.Results; // ServiceResult ve ResultStatus hangi namespace'teyse onu yaz
using Microsoft.AspNetCore.Mvc;

namespace CommerceFlow.Services.Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected int? CurrentUserId => User.GetUserId();

        protected IActionResult ToActionResult<T>(ServiceResult<T> result)
        {
            return result.Status switch
            {
                ResultStatus.Success => Ok(result),

                ResultStatus.NotFound => NotFound(result),

                ResultStatus.BadRequest => BadRequest(result),

                ResultStatus.ValidationError => BadRequest(result),

                ResultStatus.Conflict => Conflict(result),

                ResultStatus.Unauthorized => Unauthorized(result),

                ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result),

                ResultStatus.Error => StatusCode(StatusCodes.Status500InternalServerError, result),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
        }

        protected IActionResult ToActionResult(ServiceResult result)
        {
            return result.Status switch
            {
                ResultStatus.Success => Ok(result),

                ResultStatus.NotFound => NotFound(result),

                ResultStatus.BadRequest => BadRequest(result),

                ResultStatus.ValidationError => BadRequest(result),

                ResultStatus.Conflict => Conflict(result),

                ResultStatus.Unauthorized => Unauthorized(result),

                ResultStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, result),

                ResultStatus.Error => StatusCode(StatusCodes.Status500InternalServerError, result),

                _ => StatusCode(StatusCodes.Status500InternalServerError, result)
            };
        }
    }
}