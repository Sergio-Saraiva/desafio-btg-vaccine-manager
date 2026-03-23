using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineManager.Application.Common.Responses;
using VaccineManager.Application.Users.Commands;
using VaccineManager.Application.Users.Queries;

namespace VaccineManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ApiBaseController
    {
        public AuthController(ISender sender) : base(sender)
        {
        }

        [HttpPost("signup")]
        [ProducesResponseType(typeof(ApiResponse<CreateUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SignUp([FromBody] CreateUserCommand command)
        {
            return await SendRequest(command);
        }

        [HttpPost("signin")]
        [ProducesResponseType(typeof(ApiResponse<SignInQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SignIn([FromBody] SignInQuery query)
        {
            return await SendRequest(query);
        }
    }
}
