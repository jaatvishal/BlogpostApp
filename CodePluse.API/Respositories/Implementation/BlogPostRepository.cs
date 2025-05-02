using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Respositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await dbcontext.blogPosts.Include(x=>x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await dbcontext.blogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x=>x.Id== id);    
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingblogpost = await dbcontext.blogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingblogpost == null)
            {

                return null;
            }

            //update blogposts
            dbcontext.Entry(existingblogpost).CurrentValues.SetValues(blogPost);

            //update category

            existingblogpost.Categories = blogPost.Categories;

            await dbcontext.SaveChangesAsync();
            return blogPost;
        }
    }
}
