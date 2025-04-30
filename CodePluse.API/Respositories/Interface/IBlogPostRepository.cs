using CodePluse.API.Models.Domain;

namespace CodePluse.API.Respositories.Interface
{
    public interface IBlogPostRepository 
    {
        Task<BlogPost> CreateAysnc(BlogPost blogPost);

        Task<IEnumerable<BlogPost>>GetAllAsync();
    }
}
