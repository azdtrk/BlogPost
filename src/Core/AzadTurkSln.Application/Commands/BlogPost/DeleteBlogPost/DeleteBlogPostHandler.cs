﻿using AzadTurkSln.Application.Repositories;
using AzadTurkSln.Application.Wrappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AzadTurkSln.Application.Commands.BlogPost.DeleteBlogPost
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostRequest, ServiceResponse<DeleteBlogPostResponse>>
    {
        private readonly IBlogPostWriteRepository _blogPostWriteRepository;
        private readonly ILogger<DeleteBlogPostHandler> _logger;
        private readonly IValidator<DeleteBlogPostRequest> _validator;

        public DeleteBlogPostHandler(
            IBlogPostWriteRepository blogPostWriteRepository,
            ILogger<DeleteBlogPostHandler> logger,
            IValidator<DeleteBlogPostRequest> validator)
        {
            _blogPostWriteRepository = blogPostWriteRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<ServiceResponse<DeleteBlogPostResponse>> Handle(DeleteBlogPostRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Delete blogpost request validation failed: {Errors}",
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

                throw new ValidationException(validationResult.Errors);
            }

            await _blogPostWriteRepository.RemoveAsync(request.Id);

            var response = new DeleteBlogPostResponse()
            {
                Message = $"BlogPost with Id: {request.Id} has been deleted"
            };

            return new ServiceResponse<DeleteBlogPostResponse>(response);
        }
    }
}
