using Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost;
using Blog.Application.CQRS.Commands.BlogPost.UpdateBlogPost;
using Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts;
using Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById;
using Blog.Application.CustomAttributes;
using Blog.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : BaseController
    {
        public BlogPostController() { }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create a blogpost")]
        public async Task<IActionResult> Post([FromBody] CreateBlogPostRequest createblogPostCommandRequest)
        {
            await Mediator.Send(createblogPostCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllBlogPostRequest getAllBlogPostQueryRequest)
        {
            GetAllBlogPostResponse response = await Mediator.Send(getAllBlogPostQueryRequest);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetBlogPostByIdRequest GetBlogPostByIdRequest)
        {
            GetBlogPostByIdResponse response = await Mediator.Send(GetBlogPostByIdRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromQuery] UpdateBlogPostRequest updateBlogPostRequest)
        {
            UpdateBlogPostResponse response = await Mediator.Send(updateBlogPostRequest);
            return Ok(response);
        }

    }
}
