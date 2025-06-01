using Blog.Domain.Entities;

namespace Blog.Application.Abstractions.Services;

public interface IAuthorService
{
    Task<bool> AddAuthorAsync(Author updateAuthorRequest);
    Task<Author> UpdateAuthorAsync(Author updateAuthorRequest, Guid Id);
}