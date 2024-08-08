using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Apps.AdminApp.Dtos.UserDto;
using ShopApp.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopApp.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return BadRequest();
            AppUser user = new()
            {
                FullName = registerDto.UserName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "member");

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var existUser = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail);
            if (existUser == null)
            {
                existUser = await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);
                if (existUser == null) return BadRequest();
            }
            var result = await _userManager.CheckPasswordAsync(existUser, loginDto.Password);
            if (!result) return BadRequest("Password or Email wrong");
            //jwt
            var handler = new JwtSecurityTokenHandler();
            var privateKey = Encoding.UTF8.GetBytes(_configuration.GetSection("jwt:SecretKey").Value);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, existUser.Id));
            ci.AddClaim(new Claim(ClaimTypes.Name, existUser.UserName));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, existUser.FullName));
            ci.AddClaim(new Claim(ClaimTypes.Email, existUser.Email));
            var roles = (await _userManager.GetRolesAsync(existUser))
                .Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            ci.AddClaims(roles);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddHours(1),
                Subject = ci,
                Audience = _configuration.GetSection("Jwt:Audience").Value,
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
            };
            var token = handler.WriteToken(handler.CreateToken(tokenDescriptor));
            return Ok(new { token = token });
        }






        //[HttpGet]
        //public async Task CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(RolesEnum)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}

    }
}
