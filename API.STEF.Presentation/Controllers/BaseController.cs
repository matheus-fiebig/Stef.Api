using API.STEF.Application.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.STEF.Presentation.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> HandleResponse<T>(Func<Task<EitherOf<List<Error>, T>>> func)
        {
            try
            {
                var response = await func();

                if (response.Left != null && response.Left.Any())
                {
                    return BadRequest(response.Left);
                }

                return Ok(response.Right);
            }
            catch (Exception)
            {
                return Problem(
                    "Error",
                    "Ocorreu um erro inesperado no servidor.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }

        protected async Task<IActionResult> HandleResponse<T>(Func<Task<EitherOf<Error, T>>> func)
        {
            try
            {
                var response = await func();

                if (response.Left != null)
                {
                    return BadRequest(response.Left);
                }

                return Ok(response.Right);
            }
            catch (Exception)
            {
                return Problem(
                    "Error",
                    "Ocorreu um erro inesperado no servidor.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
