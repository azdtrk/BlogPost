﻿using Blog.Application.Wrappers;
using MediatR;

namespace Blog.Application.CQRS.Queries.User.GetUserById
{
    public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public Guid UserId { get; set; }
    }
}
