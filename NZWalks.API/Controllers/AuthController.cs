using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : Controller
    {
        private readonly IuserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IuserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
           
            // Check if user is authenticated
            // check username and password
            var user= await userRepository.AuthenticateAsync(
                loginRequest.Username, loginRequest.Password); 
            if(user !=null)
            {
                //Generate a JWT token
                var token = await tokenHandler.CreateTokenAsync(user);
                    return Ok(token);
            }
            return BadRequest("Username or Password is Incorrect.");
        }
    }
}
