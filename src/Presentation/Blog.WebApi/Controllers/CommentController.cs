using Blog.Application.CQRS.Commands.Comment.CreateComment;
using Blog.Application.CQRS.Commands.Comment.DeleteComment;
using Blog.Application.CQRS.Queries.Comment.GetAllComments;
using Blog.Application.CQRS.Queries.Comment.GetCommentByBlogPost;
using Blog.Application.CustomAttributes;
using Blog.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.WebApi.Controllers
{
    public class CommentController : BaseController
    {

        private readonly ILogger<CommentController> _logger;

        public CommentController(ILogger<CommentController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Reader, Author")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Post a comment")]
        public async Task<IActionResult> Post([FromBody] CreateCommentRequest createCommentRequest)
        {
            await this.Mediator.Send(createCommentRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet]
        [Authorize(Roles = "Reader, Author")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get all comments")]
        public async Task<IActionResult> Get([FromBody] GetAllCommentsRequest getAllCommentsRequest)
        {
            await this.Mediator.Send(getAllCommentsRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Reader, Author")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get comments of a blogpost")]
        public async Task<IActionResult> Get([FromBody] GetCommentByBlogPostRequest getCommentByBlogPostRequest)
        {
            await this.Mediator.Send(getCommentByBlogPostRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpDelete]
        [Authorize(Roles = "Author")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Delete comment")]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentRequest deleteCommentRequest)
        {
            await this.Mediator.Send(deleteCommentRequest);
            return StatusCode((int)HttpStatusCode.OK);
        }

    }
}
