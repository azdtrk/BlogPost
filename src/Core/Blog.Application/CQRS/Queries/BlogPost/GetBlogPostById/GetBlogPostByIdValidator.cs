using Blog.Application.CQRS.Commands.BlogPost.CreateBlogPost;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.CQRS.Queries.BlogPost.GetBlogPostById
{
    public class GetBlogPostByIdValidator : AbstractValidator<GetBlogPostByIdRequest>
    {
        public GetBlogPostByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
