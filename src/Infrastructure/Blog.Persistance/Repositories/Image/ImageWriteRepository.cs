using Blog.Application.Repositories.Image;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Image
{
    public class ImageWriteRepository : WriteRepository<Domain.Entities.Image>, IImageWriteRepository
    {
        public ImageWriteRepository(ApplicationDbContext context) : base(context) { }
    }
}
