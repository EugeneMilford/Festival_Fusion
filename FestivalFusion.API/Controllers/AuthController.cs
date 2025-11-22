using FestivalFusion.API.Models.DTO;
using FestivalFusion.API.Repositories.Implementation;
using FestivalFusion.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FestivalFusion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: {apibaseurl}/api/auth/register
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

            // Create User
            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                // Define valid roles
                var validRoles = new[] { "Reader", "Writer", "Editor", "Moderator" };

                // If no roles specified, default to Reader
                var rolesToAssign = request.Roles != null && request.Roles.Any()
                    ? request.Roles
                    : new[] { "Reader" };

                // Validate that all requested roles are valid
                var invalidRoles = rolesToAssign.Except(validRoles, StringComparer.OrdinalIgnoreCase).ToArray();
                if (invalidRoles.Any())
                {
                    ModelState.AddModelError("Roles", $"Invalid role(s): {string.Join(", ", invalidRoles)}. Valid roles are: {string.Join(", ", validRoles)}");
                    return ValidationProblem(ModelState);
                }

                // Add roles to user
                foreach (var role in rolesToAssign)
                {
                    identityResult = await userManager.AddToRoleAsync(user, role);
                    if (!identityResult.Succeeded)
                    {
                        if (identityResult.Errors.Any())
                        {
                            foreach (var error in identityResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                        return ValidationProblem(ModelState);
                    }
                }

                return Ok(new { Message = "User registered successfully", Email = user.Email, Roles = rolesToAssign });
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

        // POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Check Email
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                // Check Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    // Create a Token and Response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");


            return ValidationProblem(ModelState);
        }

    }
}

