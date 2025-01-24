using AzadTurkSln.Application.Commands.User.LoginUser;
using Microsoft.AspNetCore.Mvc;

namespace AzadTurkSln.WebApi.Controllers
{
    public class AuthController : BaseController
    {
        
        private readonly ILogger<BlogPostController> _logger;

        public AuthController(ILogger<BlogPostController> logger)
        {
            _logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserRequest loginUserCommandRequest)
        {
            LoginUserResponse response = await this.Mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }
    }
}
