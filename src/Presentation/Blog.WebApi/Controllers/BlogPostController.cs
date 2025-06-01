using Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost;
using Blog.Application.CQRS.Commands.BlogPost.UpdateBlogPost;
using Blog.Application.CQRS.Commands.BlogPost.UploadImage;
using Blog.Application.CQRS.Queries.BlogPost.GelAllBlogPosts;
using Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById;
using Blog.Application.CustomAttributes;
using Blog.Application.DTOs.ImageDtos;
using Blog.Application.Enums;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : BaseController
    {
        private readonly IWebHostEnvironment _environment;

        public BlogPostController(IWebHostEnvironment environment) 
        {
            _environment = environment;
        }

        [HttpPost]
        [Authorize]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create a blogpost")]
        public async Task<IActionResult> Post([FromBody] CreateBlogPostRequest createblogPostCommandRequest)
        {
            await Mediator.Send(createblogPostCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPost("upload-image")]
        [AllowAnonymous]
        [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Upload an image for a blogpost")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest uploadImageRequest)
        {
            var response = await Mediator.Send(uploadImageRequest);
            return Ok(response);
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get all blogposts")]
        public async Task<IActionResult> Get([FromQuery] GetAllBlogPostRequest getAllBlogPostQueryRequest)
        {
            GetAllBlogPostResponse response = await Mediator.Send(getAllBlogPostQueryRequest);
            return Ok(response);
        }

        [HttpGet("author/{authorId}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get blogposts by author")]
        public async Task<IActionResult> GetByAuthorId([FromQuery] GetAllBlogPostRequest getAllBlogPostQueryRequest)
        {
            GetAllBlogPostResponse response = await Mediator.Send(getAllBlogPostQueryRequest);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get blogpost by id")]
        public async Task<IActionResult> Get([FromQuery] GetBlogPostByIdRequest GetBlogPostByIdRequest)
        {
            GetBlogPostByIdResponse response = await Mediator.Send(GetBlogPostByIdRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Author, Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update blogpost")]
        public async Task<IActionResult> Update([FromQuery] UpdateBlogPostRequest updateBlogPostRequest)
        {
            UpdateBlogPostResponse response = await Mediator.Send(updateBlogPostRequest);
            return Ok(response);
        }
    }
}
