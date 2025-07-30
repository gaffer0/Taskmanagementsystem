using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application_.DTOs;
using Domain.Entities;
using Domain.Enums;
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
        public AccountController(UserManager<ApplicationUser> UserManager)
        {
            _userManager = UserManager;
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
                    var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsAReallyStrongSecretKey1234567890!"));
                    SigningCredentials credentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);


                    JwtSecurityToken MyToken = new JwtSecurityToken(
                        issuer: "https://localhost:7284/",
                        audience: "",
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
    }
}
