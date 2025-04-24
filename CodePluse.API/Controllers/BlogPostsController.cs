using CodePluse.API.Models.Domain;
using CodePluse.API.Models.DTO;
using CodePluse.API.Respositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePluse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        //Post :{apibaseurl}/api/blogposts
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            try
            {
                //copnvert DTO to domain model
                var blogpost = new BlogPost
                {
                    Author = request.Author,
                    Content = request.Content,
                    FeaturedImageURl = request.FeaturedImageURl,
                    IsVisible = request.IsVisible,
                    PublishedDate = request.PublishedDate,
                    ShortDescription = request.ShortDescription,
                    Title = request.Title,
                    UrlHandle = request.UrlHandle
                };

                blogpost = await _blogPostRepository.CreateAysnc(blogpost);

                // Convert Domain model to DTO

                var response = new BlogPostDto
                {
                    Id = blogpost.Id,
                    Author = request.Author,
                    Content = request.Content,
                    FeaturedImageURl = request.FeaturedImageURl,
                    IsVisible = request.IsVisible,
                    PublishedDate = request.PublishedDate,
                    ShortDescription = request.ShortDescription,
                    Title = request.Title,
                    UrlHandle = request.UrlHandle

                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                return  null;
            }


        }
    }
}
