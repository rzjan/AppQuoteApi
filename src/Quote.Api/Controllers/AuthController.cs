
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Quote.Api.DTOs;
using Quote.Api.Models.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Quote.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
   // [Route("auth/v{version:apiVersion}")]

    [Route("auth")]
    
    public class AuthController : Controller
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
        ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }

        /// <summary>
        /// Registrar nuevo usuario
        /// </summary>
        /// <param name="model">       
        /// </param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto model)
        {
            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Username
            }, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

      
        /// <summary>
        /// Iniciar sesión
        /// </summary>
        /// <param name="model">       
        /// </param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Username);

            if (user == null)
            {
                return BadRequest("Acceso denegado");
            }

            var response = (await _signInManager.CheckPasswordSignInAsync(user, model.Password, false));

            if (response.Succeeded)
            {
                return Ok(new { access_token = Token(user) });
            }

            return BadRequest("Acceso denegado");
        }

        private string Token(ApplicationUser user)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }
    }
}