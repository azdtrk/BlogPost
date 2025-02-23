using Blog.Application.CQRS.Commands.User.UpdatePassword;
using Blog.Application.CQRS.Commands.User.UpdateUser;
using Blog.Application.CQRS.Queries.User.GetUserById;
using Blog.Application.CustomAttributes;
using Blog.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.WebApi.Controllers
{
    public class UserController : BaseController
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPut("update-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Update user")]
        public async Task<IActionResult> Put([FromBody] UpdateUserRequest updateUserRequest)
        {
            await this.Mediator.Send(updateUserRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet("get-user-by-Id")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Get one user by its Id")]
        public async Task<IActionResult> Get([FromBody] GetUserByIdRequest getUserByIdRequest)
        {
            await this.Mediator.Send(getUserByIdRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            await this.Mediator.Send(updatePasswordRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
