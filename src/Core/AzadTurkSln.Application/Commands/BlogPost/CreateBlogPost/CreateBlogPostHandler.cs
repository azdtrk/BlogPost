using AutoMapper;
using AzadTurkSln.Application.Exceptions;
using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace AzadTurkSln.Application.Commands.BlogPost.CreateBlogPost
{
    public class CreateBlogPostHandler : IRequestHandler<CreateBlogPostRequest, ServiceResponse<CreateBlogPostResponse>>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBlogPostRequest> _validator;
        private readonly ILogger<CreateBlogPostHandler> _logger;

        public CreateBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            IMapper mapper,
            IValidator<CreateBlogPostRequest> validator,
            ILogger<CreateBlogPostHandler> logger
        )
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<ServiceResponse<CreateBlogPostResponse>> Handle(CreateBlogPostRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogError("Create blogpost validation failed: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    throw new ValidationException(validationResult.Errors);
                }

                var blogPost = _mapper.Map<Domain.Entities.BlogPost>(request);

                if (blogPost == null)
                    throw new MappingException();

                await _blogPostWriteRepository.AddAsync(blogPost);

                var response = new CreateBlogPostResponse { CreatedDate = blogPost.DateCreated };
                return new ServiceResponse<CreateBlogPostResponse>(response);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error creating blogpost: ({ex.Message})");
            }
        }
    }
}
