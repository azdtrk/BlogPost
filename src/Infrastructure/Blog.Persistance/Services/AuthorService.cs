using AutoMapper;
using Blog.Application.Abstractions.Services;
using Blog.Application.CQRS.Commands.Author.RegisterAuthor;
using Blog.Application.DTOs.User;
using Blog.Application.Exceptions;
using Blog.Application.Repositories.Author;
using Blog.Application.Repositories.Image;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Persistance.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorWriteRepository _authorWriteRepository;
    private readonly IAuthorReadRepository _authorReadRepository;
    private readonly IImageReadRepository _imageReadRepository;
    private readonly IImageWriteRepository _imageWriteRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorService(
        IAuthorWriteRepository authorWriteRepository,
        IAuthorReadRepository authorReadRepository,
        IImageReadRepository imageReadRepository,
        IImageWriteRepository imageWriteRepository,
        IMapper mapper,
        UserManager<ApplicationUser> userManager)
    {
        _authorWriteRepository = authorWriteRepository;
        _authorReadRepository = authorReadRepository;
        _imageReadRepository = imageReadRepository;
        _imageWriteRepository = imageWriteRepository;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    public async Task<bool> AddAuthorAsync(Author addAuthorRequest)
    {
        try
        {
            // Check if author already exists
            var existingAuthor = await _authorReadRepository.GetByIdAsync(addAuthorRequest.Id);
            if (existingAuthor != null)
            {
                throw new ApiException($"Author with ID {addAuthorRequest.Id} already exists");
            }

            // Handle profile photo if provided
            if (addAuthorRequest.ProfilePhoto != null)
            {
                var image = addAuthorRequest.ProfilePhoto;
                image.IsThumbnail = true;
                image.AuthorId = addAuthorRequest.Id;
                
                await _imageWriteRepository.AddAsync(image);
                addAuthorRequest.ProfilePhotoId = image.Id;
            }

            // Add the author to the database
            return await _authorWriteRepository.AddAsync(addAuthorRequest);
        }
        catch (Exception ex)
        {
            throw new ApiException($"Error adding author: {ex.Message}");
        }
    }

    public async Task<Author> UpdateAuthorAsync(Author updateAuthorRequest, Guid Id)
    {
        try
        {
            // Get the existing author
            var existingAuthor = await _authorReadRepository.GetByIdAsync(Id);
            if (existingAuthor == null)
            {
                throw new ApiException($"Author with ID {Id} not found");
            }

            // Update author properties
            existingAuthor.Name = updateAuthorRequest.Name ?? existingAuthor.Name;
            existingAuthor.Email = updateAuthorRequest.Email ?? existingAuthor.Email;
            existingAuthor.About = updateAuthorRequest.About ?? existingAuthor.About;

            // Handle profile photo update if provided
            if (updateAuthorRequest.ProfilePhoto != null)
            {
                // If there's an existing profile photo, update it
                if (existingAuthor.ProfilePhotoId.HasValue)
                {
                    var existingPhoto = await _imageReadRepository.GetByIdAsync(existingAuthor.ProfilePhotoId.Value);
                    if (existingPhoto != null)
                    {
                        // Update properties of existing photo
                        existingPhoto.Height = updateAuthorRequest.ProfilePhoto.Height;
                        existingPhoto.Width = updateAuthorRequest.ProfilePhoto.Width;
                        existingPhoto.IsThumbnail = true;
                        
                        bool updated = _imageWriteRepository.Update(existingPhoto);
                        if (!updated)
                        {
                            throw new ApiException("Failed to update profile photo");
                        }
                    }
                    else
                    {
                        // Create new photo if existing one not found
                        updateAuthorRequest.ProfilePhoto.IsThumbnail = true;
                        updateAuthorRequest.ProfilePhoto.AuthorId = Id;
                        await _imageWriteRepository.AddAsync(updateAuthorRequest.ProfilePhoto);
                        existingAuthor.ProfilePhotoId = updateAuthorRequest.ProfilePhoto.Id;
                    }
                }
                else
                {
                    // Add new profile photo
                    updateAuthorRequest.ProfilePhoto.IsThumbnail = true;
                    updateAuthorRequest.ProfilePhoto.AuthorId = Id;
                    await _imageWriteRepository.AddAsync(updateAuthorRequest.ProfilePhoto);
                    existingAuthor.ProfilePhotoId = updateAuthorRequest.ProfilePhoto.Id;
                }
            }

            // Update the author in the database
            bool success = _authorWriteRepository.Update(existingAuthor);
            if (!success)
            {
                throw new ApiException("Failed to update author");
            }
            
            // Return the updated author
            return existingAuthor;
        }
        catch (Exception ex)
        {
            throw new ApiException($"Error updating author: {ex.Message}");
        }
    }
}