using System.Threading.Tasks;
using Application.Helpers;
using Application.Models.Dtos;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Application.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IConfiguration configuration, IMapper mapper,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserForCreationDto userForCreationDto)
        {
            var userToCreate = _mapper.Map<User>(userForCreationDto);

            var result = await _userManager.CreateAsync(userToCreate, userForCreationDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(userToCreate, "Member");

            var userToReturn = _mapper.Map<UserForListDto>(userToCreate);

            return CreatedAtRoute("GetUserAsync", new { controller = "Users", param = userToCreate.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(UserForLoginDto userForLoginDto,
            [FromServices] IJwtHelper jwtHelper)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            var userToReturn = _mapper.Map<UserForListDto>(user);

            var tokenResult = await jwtHelper.GenerateJwtTokenAsync(user, _userManager, _configuration);
            
            return Ok(new
            {
                token = tokenResult.AccessToken,
                refreshToken = tokenResult.RefreshToken,
                user = userToReturn
            });
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequestDto requestDto,
            [FromServices] IJwtHelper jwtHelper)
        {
            var result = await jwtHelper.RefreshTokenAsync(requestDto.AccessToken, requestDto.RefreshToken, _userManager,
                _configuration);
            
            if (!result.Success) return Unauthorized("Error while refreshing given token.");

            return Ok(result);
        }
    }
}