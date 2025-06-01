using Blog.Application.DTOs.ImageDtos;
using Blog.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Blog.Application.Repositories.Image;
using ImageEntity = Blog.Domain.Entities.Image;
using ImageProcessor = SixLabors.ImageSharp.Image;

namespace Blog.Application.CQRS.Commands.BlogPost.UploadImage
{
    public class UploadImageHandler : IRequestHandler<UploadImageRequest, UploadImageResponse>
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UploadImageHandler> _logger;
        private readonly IImageWriteRepository _imageWriteRepository;

        public UploadImageHandler(
            IWebHostEnvironment environment,
            ILogger<UploadImageHandler> logger,
            IImageWriteRepository imageWriteRepository)
        {
            _environment = environment;
            _logger = logger;
            _imageWriteRepository = imageWriteRepository;
        }

        public async Task<UploadImageResponse> Handle(UploadImageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                string webRootPath = GetOrCreateWebRootPath();
                string uploadsDir = GetOrCreateUploadsDirectory(webRootPath);
                string uniqueFileName = GenerateUniqueFileName(request.Image.FileName);
                string filePath = Path.Combine(uploadsDir, uniqueFileName);

                int width = 0, height = 0;
                using (var image = await ImageProcessor.LoadAsync(request.Image.OpenReadStream(), cancellationToken))
                {
                    width = image.Width;
                    height = image.Height;
                    
                    if (width > 2000 || height > 2000)
                    {
                        var scaleFactor = Math.Min(2000.0 / width, 2000.0 / height);
                        width = (int)(width * scaleFactor);
                        height = (int)(height * scaleFactor);
                        image.Mutate(x => x.Resize(width, height));
                    }

                    await image.SaveAsync(filePath, cancellationToken);
                }

                string relativePath = $"/uploads/blogimages/{uniqueFileName}";
                
                var imageEntity = new ImageEntity
                {
                    Id = Guid.NewGuid(),
                    FileName = uniqueFileName,
                    OriginalFileName = request.Image.FileName,
                    ContentType = request.Image.ContentType,
                    Size = request.Image.Length,
                    Path = relativePath,
                    Storage = "local",
                    Width = width,
                    Height = height,
                    IsThumbnail = request.IsThumbnail,
                    BlogPostId = request.BlogPostId,
                    ThumbnailForBlogPostId = request.ThumbnailForBlogPostId,
                    UploadedAt = DateTime.UtcNow
                };

                var added = await _imageWriteRepository.AddAsync(imageEntity);
                if (!added)
                {
                    throw new ApiException("Failed to save image to database.");
                }

                var imageDto = new ImageDto
                {
                    Id = imageEntity.Id,
                    FileName = imageEntity.FileName,
                    Path = imageEntity.Path,
                    OriginalFileName = imageEntity.OriginalFileName,
                    ContentType = imageEntity.ContentType,
                    Size = imageEntity.Size,
                    Width = imageEntity.Width,
                    Height = imageEntity.Height,
                    IsThumbnail = imageEntity.IsThumbnail,
                    BlogPostId = imageEntity.BlogPostId,
                    ThumbnailForBlogPostId = imageEntity.ThumbnailForBlogPostId,
                    UploadedAt = imageEntity.UploadedAt
                };

                return new UploadImageResponse
                {
                    Value = imageDto,
                    Message = "Image uploaded successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {ErrorMessage}", ex.Message);
                throw new ApiException($"Error uploading image: {ex.Message}");
            }
        }

        private string GetOrCreateWebRootPath()
        {
            string webRootPath;
            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                if (!Directory.Exists(webRootPath))
                {
                    _logger.LogInformation("Creating wwwroot directory at {Path}", webRootPath);
                    Directory.CreateDirectory(webRootPath);
                }
            }
            else
            {
                webRootPath = _environment.WebRootPath;
            }
            return webRootPath;
        }

        private string GetOrCreateUploadsDirectory(string webRootPath)
        {
            string uploadsDir = Path.Combine(webRootPath, "uploads", "blogimages");
            if (!Directory.Exists(uploadsDir))
            {
                _logger.LogInformation("Creating uploads directory at {Path}", uploadsDir);
                Directory.CreateDirectory(uploadsDir);
            }
            return uploadsDir;
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            return $"{Guid.NewGuid()}_{Path.GetFileName(originalFileName)}";
        }
    }
} 