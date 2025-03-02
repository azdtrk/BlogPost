using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostRequest, DeleteBlogPostResponse>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IBlogPostReadRepository _blogPostReadRepository;

        public DeleteBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IBlogPostReadRepository blogPostReadRepository)
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _blogPostReadRepository = blogPostReadRepository;
        }

        public async Task<DeleteBlogPostResponse> Handle(DeleteBlogPostRequest request,
            CancellationToken cancellationToken)
        {
            var blogPostToBeDeleted = await _blogPostReadRepository.GetByIdAsync(request.Id);

            if (blogPostToBeDeleted == null)
                throw new EntityNotFoundException(nameof(BlogPost), request.Id);

            _blogPostWriteRepository.Remove(blogPostToBeDeleted);

            var response = new DeleteBlogPostResponse()
            {
                Value = $"BlogPost with Id: {request.Id} has been deleted"
            };

            return response;
        }
    }
}