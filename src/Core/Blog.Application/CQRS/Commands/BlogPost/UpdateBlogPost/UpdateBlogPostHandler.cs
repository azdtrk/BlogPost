using AutoMapper;
using Blog.Application.DTOs.BlogPost;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using MediatR;

namespace Blog.Application.CQRS.Commands.BlogPost.UpdateBlogPost
{
    public class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostRequest, UpdateBlogPostResponse>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IBlogPostReadRepository _blogPostReadRepository;
        private readonly IMapper _mapper;

        public UpdateBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IBlogPostReadRepository blogPostReadRepository,
            IMapper mapper)
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _blogPostReadRepository = blogPostReadRepository;
            _mapper = mapper;
        }

        public async Task<UpdateBlogPostResponse> Handle(UpdateBlogPostRequest request,
            CancellationToken cancellationToken)
        {
            var blogPostToBeUpdated = await _blogPostReadRepository.GetByIdAsync(request.Id);

            if (blogPostToBeUpdated == null)
                throw new EntityNotFoundException(nameof(BlogPost), request.Id);

            blogPostToBeUpdated.Title = request.Title;
            blogPostToBeUpdated.Preface = request.Preface;
            blogPostToBeUpdated.Content = request.Content;
            blogPostToBeUpdated.ThumbnailImage = request.ThumbNailImage;
            blogPostToBeUpdated.Images = request.Images;

            _blogPostWriteRepository.Update(blogPostToBeUpdated);

            var blogPostUpdateDto = _mapper.Map<BlogPostUpdateDto>(blogPostToBeUpdated);

            var response = new UpdateBlogPostResponse()
            {
                Value = blogPostUpdateDto
            };

            return response;
        }
    }
}