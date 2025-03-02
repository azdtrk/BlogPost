using AutoMapper;
using Blog.Application.DTOs.Comment;
using Blog.Application.Exceptions;
using Blog.Application.Repositories;
using MediatR;

namespace Blog.Application.CQRS.Commands.Comment.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest, UpdateCommentResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IMapper _mapper;

        public UpdateCommentHandler(
            ICommentWriteRepository commentWriteRepository,
            ICommentReadRepository commentReadRepository,
            IMapper mapper
        )
        {
            _commentWriteRepository = commentWriteRepository;
            _commentReadRepository = commentReadRepository;
            _mapper = mapper;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var commentToBeUpdated = await _commentReadRepository.GetByIdAsync(request.Id);

                if (commentToBeUpdated == null)
                    throw new EntityNotFoundException(nameof(Comment), request.Id);

                commentToBeUpdated.Content = request.Content;

                _commentWriteRepository.Update(commentToBeUpdated);

                var commentDto = _mapper.Map<CommentDto>(commentToBeUpdated);

                var response = new UpdateCommentResponse()
                {
                    Value = commentDto
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating comment: ({ex.Message})");
            }
        }
    }
}