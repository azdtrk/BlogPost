using AutoMapper;
using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using MediatR;

namespace Blog.Application.CQRS.Commands.Author.UpdateAuthor
{
    public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorRequest, UpdateAuthorResponse>
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public UpdateAuthorHandler(
            IMapper mapper,
            IAuthorService authorService
        )
        {
            _mapper = mapper;
            _authorService = authorService;
        }

        public async Task<UpdateAuthorResponse> Handle(UpdateAuthorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Map request to Author entity
                Domain.Entities.Author authorToBeUpdated = _mapper.Map<Domain.Entities.Author>(request);
                
                // Call service to update author
                Domain.Entities.Author updatedAuthor =
                    await _authorService.UpdateAuthorAsync(authorToBeUpdated, request.Id);

                // Prepare successful response
                var authorDto = _mapper.Map<AuthorDto>(updatedAuthor);
                UpdateAuthorResponse response = new UpdateAuthorResponse
                {
                    IsSuccess = true,
                    Message = "Author updated successfully",
                    Value = authorDto
                };
                
                return response;
            }
            catch (Exception ex)
            {
                // Return error response
                return new UpdateAuthorResponse
                {
                    IsSuccess = false,
                    Message = $"Error updating the author: {ex.Message}",
                    Value = null
                };
            }
        }
    }
}