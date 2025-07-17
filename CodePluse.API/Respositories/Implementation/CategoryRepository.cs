using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Respositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePluse.API.Respositories.Implementation
{
    public class CategoryRepository : ICategoryRespository
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

        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null,int? pageNumber = 1,int? pageSize=100 )
        {
            //query 
            var categories = dbContext.Categories.AsQueryable();

            // Filtering 
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            //Sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;

                    categories = isAsc
                        ? categories.OrderBy(x => x.Name)
                        : categories.OrderByDescending(x => x.Name);  
                }
                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;

                    categories = isAsc
                        ? categories.OrderBy(x => x.UrlHandle)
                        : categories.OrderByDescending(x => x.UrlHandle);
                }
            }


            //Pagination
            // Page Number 1 pageSize 5 -skip 0 take 5
            // Page Number 2 pageSize 5 -skip 5 take 5
            // Page Number 3 pageSize 5 -skip 10 take 5

            var skipResults =   (pageNumber - 1) * pageSize;
            categories =categories.Skip(skipResults??0 ).Take(pageSize?? 100);



            return await categories.ToListAsync();

        }

        public async Task<Category?> GetById(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingcategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);

            if (existingcategory != null)
            {
                dbContext.Entry(existingcategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingcategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existingcategory is null)
            {
                return null;
            }

            dbContext.Categories.Remove(existingcategory);
            await dbContext.SaveChangesAsync();
            return existingcategory;
        }

         public async Task<int> GetCount()
        {
            return  await dbContext.Categories.CountAsync();
        }
    }
}
