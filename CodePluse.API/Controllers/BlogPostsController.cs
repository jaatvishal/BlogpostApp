using CodePluse.API.Models.Domain;
using CodePluse.API.Models.DTO;
using CodePluse.API.Respositories.Implementation;
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
        private readonly ICategoryRespository _categoryRespository;
        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRespository categoryRespository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRespository = categoryRespository;
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
                    UrlHandle = request.UrlHandle,
                    Categories = new List<Category>()
                };
                  
                foreach(var categoryGuid in request.Categories)
                {
                    var existingcategories = await _categoryRespository.GetById(categoryGuid);
                    if(existingcategories is not null)
                    {
                        blogpost.Categories.Add(existingcategories);
                    }
                }


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
                    UrlHandle = request.UrlHandle,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                return  null;
            }


        }

        // Get :{apibaseurl}/api/blogposts         
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
          var blogposts =await _blogPostRepository.GetAllAsync();

            /// conert Domain model to DTO
            /// 
            var response = new List<BlogPostDto>();
            foreach(var blogpost in blogposts)
            {
                response.Add(new BlogPostDto
                {
                    Author = blogpost.Author,
                    Content = blogpost.Content,
                    FeaturedImageURl = blogpost.FeaturedImageURl,
                    IsVisible = blogpost.IsVisible,
                    PublishedDate = blogpost.PublishedDate,
                    Id = blogpost.Id,
                    ShortDescription = blogpost.ShortDescription,
                    Title = blogpost.Title,
                    UrlHandle = blogpost.UrlHandle,
                    Categories = blogpost.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
          var blogpostdomain=await _blogPostRepository.GetByIdAsync(id);

            if(blogpostdomain is not null)
            {
                var reponse = new BlogPostDto{
                    Author = blogpostdomain.Author,
                    Content = blogpostdomain.Content,
                    Id = blogpostdomain.Id,
                    ShortDescription = blogpostdomain.ShortDescription,
                    Title = blogpostdomain.Title,
                    UrlHandle = blogpostdomain.UrlHandle,
                    FeaturedImageURl=blogpostdomain.FeaturedImageURl,
                    IsVisible = blogpostdomain.IsVisible,
                    PublishedDate = blogpostdomain.PublishedDate,
                    Categories= blogpostdomain.Categories.
                    Select(x=> new CategoryDto
                    { Id=x.Id,Name=x.Name,UrlHandle=x.UrlHandle}).ToList()                    
                };

                return Ok(reponse); 
            }
            else
            {
                return NotFound();
            }
        }


        // GET  :{apibaseurl}/api/blogPosts/{urlhandle}

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostsByUrlHandle([FromRoute] string urlHandle)
        {
             //Get  blogpost details from respository  
            var blogpostdomain = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (blogpostdomain is not null)
            {
                var reponse = new BlogPostDto
                {
                    Author = blogpostdomain.Author,
                    Content = blogpostdomain.Content,
                    Id = blogpostdomain.Id,
                    ShortDescription = blogpostdomain.ShortDescription,
                    Title = blogpostdomain.Title,
                    UrlHandle = blogpostdomain.UrlHandle,
                    FeaturedImageURl = blogpostdomain.FeaturedImageURl,
                    IsVisible = blogpostdomain.IsVisible,
                    PublishedDate = blogpostdomain.PublishedDate,
                    Categories = blogpostdomain.Categories.
                    Select(x => new CategoryDto
                    { Id = x.Id, Name = x.Name, UrlHandle = x.UrlHandle }).ToList()
                };

                return Ok(reponse);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
        {
            //convert DTO to Domain 
            var blogPost = new BlogPost
            {
                Id = id,
                Author = request.Author,
                Content = request.Content,
                FeaturedImageURl = request.FeaturedImageURl,
                PublishedDate = request.PublishedDate,
                IsVisible = request.IsVisible,
                ShortDescription = request.ShortDescription,
                Title = request.Title,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()


            };
            foreach (var categoryGuid in request.Categories) {
                 var exsitingcategoery=await _categoryRespository.GetById(categoryGuid);
                if (exsitingcategoery != null)
                {
                    blogPost.Categories.Add(exsitingcategoery);
                }
            }
            //call Repository  TO Update BlogPost Domain  Model
          var updatedblogpost=  await _blogPostRepository.UpdateAsync(blogPost);
            if (updatedblogpost == null) {
                return NotFound();
            }

            var response = new BlogPostDto
            {
                Id = updatedblogpost.Id,
                Author = updatedblogpost.Author,
                Content = updatedblogpost.Content,
                IsVisible = updatedblogpost.IsVisible,
                PublishedDate = updatedblogpost.PublishedDate,
                ShortDescription = updatedblogpost.ShortDescription,
                Title = updatedblogpost.Title,
                Categories=updatedblogpost.Categories.Select(x=> new CategoryDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    UrlHandle=x.UrlHandle
                }).ToList()
            };
            
            return Ok(response);
        }

        //Delete :{apibase}/api/blogposts/{id}

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var isexists = await _blogPostRepository.DeleteAsync(id);
             if(isexists == null) { return NotFound();
                }
             var response = new BlogPostDto
             {
                 Author = isexists.Author,
                 Content = isexists.Content,
                 PublishedDate = isexists.PublishedDate,
                 FeaturedImageURl = isexists.FeaturedImageURl,
                 Id = id,
                 IsVisible = isexists.IsVisible,
                 ShortDescription = isexists.ShortDescription,
                 Title = isexists.Title,
                 UrlHandle = isexists.UrlHandle
             };

            return Ok(response);
        }
    }
}
