using AzadTurkSln.Application.Commands.BlogPost.CreateBlogPost;
using AzadTurkSln.Application.Queries.BlogPost.GelAllBlogPosts;
using AzadTurkSln.Application.Queries.BlogPost.GetBlogPostById;
using AzadTurkSln.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AzadTurkSln.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : BaseController
    {
        private readonly ILogger<BlogPostController> _logger;

        public BlogPostController(ILogger<BlogPostController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateBlogPostRequest createblogPostCommandRequest)
        {
            try
            {
                await this.Mediator.Send(createblogPostCommandRequest);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating blog post");
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllBlogPostRequest getAllBlogPostQueryRequest)
        {
            try
            {
                GetAllBlogPostResponse response = await this.Mediator.Send(getAllBlogPostQueryRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting blog posts");
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromQuery] GetBlogPostByIdRequest GetBlogPostByIdRequest)
        {
            try
            {
                GetBlogPostByIdResponse response = await this.Mediator.Send(GetBlogPostByIdRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while getting the blog post");
                return StatusCode(500, "An unexpected error occurred while processing your request");
            }
        }


    }
}
