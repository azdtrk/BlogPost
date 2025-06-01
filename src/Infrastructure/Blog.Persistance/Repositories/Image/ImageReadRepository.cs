using Blog.Application.Repositories.Image;
using Blog.Persistance.Context;

namespace Blog.Persistance.Repositories.Image
{
    public class ImageReadRepository : ReadRepository<Domain.Entities.Image>, IImageReadRepository
    {
        public ImageReadRepository(ApplicationDbContext context) : base(context) { }
    }
}
