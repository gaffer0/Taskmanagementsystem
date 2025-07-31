using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application_.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Taskmanagementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]//api/account/register
        public async Task<IActionResult> Register(ResgiterUserDTO RUser)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = RUser.UserName,
                    Email = RUser.Email,
                    Role = UserRole.Member, // Default role for new users
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, RUser.Password);
                if (result.Succeeded)
                {
                    // Add user to role
                    await _userManager.AddToRoleAsync(user, user.Role.ToString());
                    return Ok("User registered successfully");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

            }
            return BadRequest(ModelState);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO UserFromRequest)
        {
            if (ModelState.IsValid)
            {
                //check
                ApplicationUser? UserFromDB = await _userManager.FindByNameAsync(UserFromRequest.userName);
                if (UserFromDB == null)
                {
                    ModelState.AddModelError("UserName", "User name or password is invalid");
                    return BadRequest(ModelState);
                }

                bool found = await _userManager.CheckPasswordAsync(UserFromDB, UserFromRequest.password);

                if (found && UserFromDB.IsActive)
                {
                    // Update last login
                    UserFromDB.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(UserFromDB);

                    // Token Genrated id change (JWT predefind Claims JIT)
                    //Generate Token

                    List<Claim> UserClaim = new List<Claim>();
                    UserClaim
                        .Add(new Claim(ClaimTypes.NameIdentifier, UserFromDB.Id.ToString()));
                    UserClaim
                        .Add(new Claim(ClaimTypes.Name, UserFromDB.UserName ?? ""));
                    UserClaim
                        .Add(new Claim(ClaimTypes.Email, UserFromDB.Email ?? ""));
                    UserClaim
                        .Add(new Claim("Role", UserFromDB.Role.ToString()));

                    UserClaim
                        .Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var UserRoles = await _userManager.GetRolesAsync(UserFromDB);
                    foreach (var RoleName in UserRoles)
                    {
                        UserClaim.Add(new Claim(ClaimTypes.Role, RoleName));
                    }
                    var jwtKey = _configuration["Jwt:Key"];
                    var jwtIssuer = _configuration["Jwt:Issuer"];
                    var jwtAudience = _configuration["Jwt:Audience"];

                    if (string.IsNullOrEmpty(jwtKey))
                    {
                        return StatusCode(500, "JWT configuration is missing");
                    }

                    var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                    SigningCredentials credentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken MyToken = new JwtSecurityToken(
                        issuer: jwtIssuer,
                        audience: jwtAudience,
                        expires: DateTime.Now.AddHours(1),
                        claims: UserClaim,
                        signingCredentials: credentials
                    );



                    //generate Token response
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                        expiration = DateTime.Now.AddHours(1),
                        user = new
                        {
                            id = UserFromDB.Id,
                            userName = UserFromDB.UserName,
                            email = UserFromDB.Email,
                            fullName = UserFromDB.FullName,
                            role = UserFromDB.Role.ToString()
                        }
                    });


                }

                ModelState.AddModelError("UserName", "User name or password is invalid");

            }
            return BadRequest(ModelState);
        }



        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // For JWT, logout is typically handled on client by discarding token
            // Alternatively, implement token blacklist if needed
            return Ok("Logout successful");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(forgotDto.Email);
            if (user == null || !user.IsActive)
            {
                // To prevent account enumeration, respond with success message
                return Ok("If an account with that email exists, a password reset link has been sent.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            // TODO: Send email with resetLink (via SMTP or email service)
            // For demonstration, returning link in response
            return Ok(new { message = "Password reset link has been sent.", resetLink });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (resetDto.NewPassword != resetDto.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Passwords do not match");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(resetDto.Email);
            if (user == null || !user.IsActive)
            {
                return BadRequest("Invalid request");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetDto.Token, resetDto.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password has been reset successfully");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
    }
}
