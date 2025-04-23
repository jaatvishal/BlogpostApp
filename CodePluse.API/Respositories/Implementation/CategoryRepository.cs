using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Respositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePluse.API.Respositories.Implementation
{
    public class CategoryRepository :ICategoryRespository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateASync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() { 
             return await dbContext.Categories.ToListAsync();
        }
        
        public async Task<Category?> GetById(Guid id)
        {
          return  await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
           var existingcategory=   await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id==category.Id);

            if (existingcategory != null)
            {
                dbContext.Entry(existingcategory).CurrentValues.SetValues(category);
               await  dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingcategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(existingcategory is null)
            {
                return null;
            }

            dbContext.Categories.Remove(existingcategory);
              await  dbContext.SaveChangesAsync();
            return existingcategory;
        }
    }
}
