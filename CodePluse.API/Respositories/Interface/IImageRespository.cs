using CodePluse.API.Models.Domain;

namespace CodePluse.API.Respositories.Interface
{
    public interface IImageRespository
    {

        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAllImages();
    }
}
