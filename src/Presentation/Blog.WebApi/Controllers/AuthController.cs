using Blog.Application.CQRS.Commands.User.LoginUser;
using Blog.Application.CQRS.Commands.User.RegisterUser;
using Blog.Application.CustomAttributes;
using Blog.Application.Enums;
using Blog.WebApi.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        [HttpPost("login")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Login")]
        [SwaggerResponse(StatusCodes.Status200OK,
            SwaggerDocumentation.AuthenticationConstants.SuccessLoginRequestDescriptionMessage)]
        public async Task<IActionResult> Login(LoginUserRequest loginUserCommandRequest)
        {
            LoginUserResponse response = await this.Mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("register")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Register")]
        public async Task<IActionResult> Register(RegisterUserRequest registerUserRequest)
        {
            RegisterUserResponse response = await this.Mediator.Send(registerUserRequest);
            return Ok(response);
        }
    }
}
