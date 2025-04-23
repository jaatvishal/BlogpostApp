using CodePluse.API.Models.Domain;

namespace CodePluse.API.Respositories.Interface
{
    public interface ICategoryRespository
    {
        Task<Category> CreateASync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);
    }
}
