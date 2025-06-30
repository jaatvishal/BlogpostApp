using CodePluse.API.Models.DTO;
using CodePluse.API.Respositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePluse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //Post :{apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var indentityuser=await userManager.FindByEmailAsync(request.Email);
            if (indentityuser is not null)
            {
                // check password 
                var checkpaswordresult=await userManager.CheckPasswordAsync(indentityuser, request.Password);
                if (checkpaswordresult)
                {
                    var roles =await userManager.GetRolesAsync(indentityuser);  

                    // create a token and response
                    var jwtToken= tokenRepository.CreateJwtToken(indentityuser, roles.ToList()); 

                    var response = new LoginResponseDto() { 
                        Email = request.Email,
                    Roles=roles.ToList(),
                    Token= jwtToken
                    };

                    return Ok(response);
                }

            }
            ModelState.AddModelError("","Email or Password Incorrect");
            return ValidationProblem(ModelState);
        }

        //Post :{apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };
            // create user
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                //Add Role to user 

                identityResult = await userManager.AddToRoleAsync(user, "REader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);

                        }
                    }
                }

            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
