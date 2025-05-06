using CodePluse.API.Models.Domain;
using CodePluse.API.Models.DTO;
using CodePluse.API.Respositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePluse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRespository imageRespository;
        public ImagesController(IImageRespository imageRespository)
        {
         this.imageRespository = imageRespository;
        }


        //  {apibase}/api/Images
        //Get
        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var extingimagelist = await imageRespository.GetAllImages();

            // converting domain to DTO
            if (extingimagelist != null)
            {

               var response = new List<BlogImageDto>();
                foreach (var image in extingimagelist)
                {

                    var temp = new BlogImageDto
                    {
                        Id = image.Id,
                        DateCreated = image.DateCreated,
                        FileExtension = image.FileExtension,
                        Title = image.Title,
                        Url = image.Url
                    };

                    response.Add(temp);
                }
                return Ok(response);
            }
            return NotFound();
        }
        //POst :{apibaseurl}/api/images
        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string filename, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid) {

                // file Upload

                var blogimage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName.ToLower()),
                    FileName = filename,
                    Title = title,
                    DateCreated = DateTime.Now,

                };
              blogimage=   await imageRespository.Upload(file, blogimage);
                //convert domain to DTO
                var response = new BlogImageDto
                {
                    Id = blogimage.Id,
                    Title = blogimage.Title,
                    DateCreated = blogimage.DateCreated,
                    FileExtension = blogimage.FileExtension,
                    FileName = blogimage.FileName,
                    Url = blogimage.Url,
                };
                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file) {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) {

                ModelState.AddModelError("Error", "Unsuppoerted File Format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("Error", "File Size cannnot be more than 10 MB");
            }
        }
    }
}
