using CodePluse.API.Models.Domain;

namespace CodePluse.API.Respositories.Interface
{
    public interface ICategoryRespository
    {
        Task<Category> CreateASync(Category category);

        Task<IEnumerable<Category>> GetAllAsync(string? query=null,
            string?sortBy = null,string?   sortDirection = null,int? pageNumber =1, int? pageSize = 100
            );

        Task<Category?> GetById(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);

        Task<int> GetCount();
    }
}
