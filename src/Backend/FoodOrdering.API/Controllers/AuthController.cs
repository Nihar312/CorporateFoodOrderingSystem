using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FoodOrdering.API.Models;
using FoodOrdering.API.DTOs;
using FoodOrdering.API.Services;

namespace FoodOrdering.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CompanyName = request.CompanyName,
                UserType = UserType.User,
                FCMToken = request.FCMToken
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var token = _tokenService.GenerateToken(user, "User");

                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString(),
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                // Update FCM token if provided
                if (!string.IsNullOrEmpty(request.FCMToken))
                {
                    user.FCMToken = request.FCMToken;
                    await _userManager.UpdateAsync(user);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, role);

                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString(),
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpPost("create-vendor")]
        public async Task<IActionResult> CreateVendor([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CompanyName = request.CompanyName,
                UserType = UserType.Vendor,
                FCMToken = request.FCMToken
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Vendor");

                var token = _tokenService.GenerateToken(user, "Vendor");

                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString(),
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };

                return Ok(response);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
