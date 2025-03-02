using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentRequest, CreateCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IMapper _mapper;

        public CreateCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            IMapper mapper)
        {
            _commentWriteRepository = commentWriteRepository;
            _mapper = mapper;
        }

        public async Task<CreateCommentResponse> Handle(CreateCommentRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var comment = _mapper.Map<Domain.Entities.Comment>(request);

                // ToDo: Implement a better approach of handling the mapping errors
                if (comment == null)
                    throw new MappingException();

                await _commentWriteRepository.AddAsync(comment);

                var response = new CreateCommentResponse
                {
                    Value = "Comment has been received.",
                    CreatedDate = comment.DateCreated
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error creating comment: ({ex.Message})");
            }
        }
    }
}