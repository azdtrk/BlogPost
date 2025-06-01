using AutoMapper;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using MediatR;

namespace Blog.Application.CQRS.Commands.Author.RegisterAuthor
{
    public class RegisterAuthorHandler : IRequestHandler<RegisterAuthorRequest, RegisterAuthorResponse>
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public RegisterAuthorHandler(
            IAuthorService authorService,
            IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }

        public async Task<RegisterAuthorResponse> Handle(RegisterAuthorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Map request to Author entity
                var author = _mapper.Map<Domain.Entities.Author>(request);
                
                // Add author using the service
                var isAdded = await _authorService.AddAuthorAsync(author);
                
                // Prepare response
                var response = new RegisterAuthorResponse();
                if (isAdded) 
                {
                    // Get the author details for response
                    var authorDto = _mapper.Map<AuthorDto>(author);
                    
                    response = new RegisterAuthorResponse
                    {
                        IsSuccess = true,
                        Message = "Author registered successfully",
                        Value = authorDto
                    };
                }
                else
                {
                    response = new RegisterAuthorResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to register author",
                        Value = null
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error registering the author: {ex.Message}");
            }
        }
    }
}