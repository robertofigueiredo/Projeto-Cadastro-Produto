using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Projeto_API_Conceitos.Models;
using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto_API_Conceitos.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userInManager;
        private readonly JWTSettings _jwtsettings;

        public AuthController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userInManager,
                              IOptions<JWTSettings> jwtsettings)
        {
            _signInManager = signInManager;
            _userInManager = userInManager;
            _jwtsettings = jwtsettings.Value;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var usuario = new IdentityUser()
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true,
            };

            var response = await _userInManager.CreateAsync(usuario, registerUser.Password);

            if (response.Succeeded)
            {
                await _signInManager.SignInAsync(usuario,false);

                return Ok(await GerarJwt(usuario.Email));
            }

            return Problem("Falha ao registrar usuário");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var response = await _signInManager.PasswordSignInAsync(loginUser.email, loginUser.password, false, true);// você trava o usuário depois de 5 tentativas

            if (response.Succeeded)
            {

                return Ok(await GerarJwt(loginUser.email));
            }

            return Problem("Falha ao registrar usuário");
        }

        private async Task<string> GerarJwt(string email)
        {

            var user = await _userInManager.FindByEmailAsync(email);
            var roles = await _userInManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Name, role));
            }

            var TratativaToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.Segredo);

            var token = TratativaToken.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtsettings.Emissor,
                Audience = _jwtsettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtsettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var EncodedToken = TratativaToken.WriteToken(token);

            return EncodedToken;
        }
    }
}
