using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Models.DTO;
using CodePluse.API.Respositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePluse.API.Controllers
{
    //https://localhost:xxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRespository categoryRespository;        
        public CategoriesController(ICategoryRespository categoryRespository)
        {
           this.categoryRespository = categoryRespository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            // Map DTO to Domain MOdel
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
           
            await categoryRespository.CreateASync(category);
            //Domain model to DTo
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            return Ok(response);
        }

        // GET :/api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
           var caterogies= await categoryRespository.GetAllAsync();

            //map Domain model to DTO
            var  response=new List<CategoryDto>();
            foreach (var category in caterogies) {
                response.Add(new CategoryDto { Id = category.Id, Name = category.Name, UrlHandle = category.UrlHandle });
            }

            return Ok(response);
        }


       // GEt : https://localhost:7726/api/category/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
           var existingCategory= await categoryRespository.GetById(id);
          if(existingCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };
            return Ok(response);
        }

        // PUT https://localhost:7726/api/category/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id,UpdateCategoryRequestDto request)
        {
            // Convert DTo to Domain Model

            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category =await categoryRespository.UpdateAsync(category);

            if (category is null) { 
            return NotFound();
            }
            // Convert Domain Model TO DTO

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);

        }

        //Delete https://localhost:7726/api/category/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
           var category= await categoryRespository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }
            /// convert Domain model to DTo
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
    }
}
