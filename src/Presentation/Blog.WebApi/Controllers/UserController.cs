using Blog.Application.CQRS.Commands.User.UpdatePassword;
using Blog.Application.CQRS.Queries.User.GetUserById;
using Blog.Application.CustomAttributes;
using Blog.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        
        [HttpGet("get-user-by-Id")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Get one user by its Id")]
        public async Task<IActionResult> Get([FromBody] GetUserByIdRequest getUserByIdRequest)
        {
            await this.Mediator.Send(getUserByIdRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPost("update-password")]
        [Authorize(Roles = "Reader, Author")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Update password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            await this.Mediator.Send(updatePasswordRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }
        
    }
}
