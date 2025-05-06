using CodePluse.API.Data;
using CodePluse.API.Models.Domain;
using CodePluse.API.Respositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePluse.API.Respositories.Implementation
{
    public class ImageRepository : IImageRespository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext DbContext;

        public ImageRepository(IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext DbContext)
        {
            this.webHostEnvironment = _webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this.DbContext = DbContext;
        }


        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // 1 -Upload  the Image to APi/Images
            var localpath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localpath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2-update the database 

            var httpRequest = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await DbContext.BlogImages.AddAsync(blogImage);
            await DbContext.SaveChangesAsync();

            return blogImage;

        }

        public async Task<IEnumerable<BlogImage>> GetAllImages()
        {
            var existing = await DbContext.BlogImages.ToListAsync();
            return existing;
        }
    }
}
