using Blog.Application.CQRS.Commands.User.LoginUser;
using Blog.Application.CQRS.Commands.User.RegisterUser;
using Blog.WebApi.SwaggerExamples;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Blog.WebApi.Controllers
{
    public class AuthController : BaseController
    {
        
        public AuthController()
        {

        }

        [HttpPost("[action]")]
        [SwaggerResponse(StatusCodes.Status200OK,
            SwaggerDocumentation.AuthenticationConstants.SuccessLoginRequestDescriptionMessage)]
        public async Task<IActionResult> Login(LoginUserRequest loginUserCommandRequest)
        {
            LoginUserResponse response = await this.Mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            RegisterUserResponse response = await this.Mediator.Send(registerUserRequest);
            return Ok(response);
        }

    }
}
