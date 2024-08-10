using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Apps.AdminApp.Dtos.UserDto;
using ShopApp.Entities;
using ShopApp.Services.Interfaces;

namespace ShopApp.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;
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
            var roles = await _userManager.GetRolesAsync(existUser);
            return Ok(new { token = _tokenService.GetToken(existUser, roles) });

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser == null) return NotFound();
            return Ok(_mapper.Map<UserGetDto>(existUser));


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
