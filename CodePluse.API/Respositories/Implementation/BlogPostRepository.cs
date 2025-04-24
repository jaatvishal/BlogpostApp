using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Respositories.Interface;

namespace CodePluse.API.Respositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext dbcontext;
        public BlogPostRepository(ApplicationDbContext _dbContext) {

            dbcontext = _dbContext;
        }
        public async Task<BlogPost> CreateAysnc(BlogPost blogPost)
        {
            try
            {
                await dbcontext.blogPosts.AddAsync(blogPost);
                await dbcontext.SaveChangesAsync();
                return blogPost;
            }
            catch (Exception ex)
            {
                return null;
            }

            
        }
    }
}
