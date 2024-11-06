using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.Business.Operations.Order;
using OnlineShoppingPlatform.Business.Operations.User;
using OnlineShoppingPlatform.Business.Operations.User.Dtos;
using OnlineShoppingPlatform.WebApi.Jwt;
using OnlineShoppingPlatform.WebApi.Models;

namespace OnlineShoppingPlatform.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Dependency injection for user service
        private readonly IUserService _userService;

        // Constructor to initialize the AuthController with user service
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // Endpoint for user registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid) // Check if the model state is valid, return bad request if not
                return BadRequest(ModelState);
           

            var addUserDto = new AddUserDto // Create a DTO (Data Transfer Object) for adding a new user
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
                
            };
            
            var result = await _userService.AddUser(addUserDto); // Add the user through user service and check the result

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            
            return Ok("User registered successfully.");
        }

        // Endpoint for user login
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
                return BadRequest(result.Message);
             
            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto   // Generate JWT token for the logged-in user
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse
            {
                Message = "Logged in successfully.",
                Token = token,
            });
        }

        // Endpoint to retrieve all users, accessible only by admin users
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
