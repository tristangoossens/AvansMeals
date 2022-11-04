using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Webservice.WebApi.Models;
using System.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Webservice.WebApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration config
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        [Route("/api/v1/[controller]/SignIn")]
        public async Task<IActionResult> SignIn([FromBody]SignInDTO signInCredentials)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInCredentials.Email);
                if(user != null)
                {
                    if((await _signInManager.PasswordSignInAsync(user, 
                        signInCredentials.Password, false, false)).Succeeded)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var securityToken = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
                        {
                            Subject = (await _signInManager.CreateUserPrincipalAsync(user)).Identities.First(),
                            Expires = DateTime.Now.AddMinutes(int.Parse(_config["BearerToken:ExpiryMinutes"])),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(_config["BearerToken:Key"])), SecurityAlgorithms.HmacSha256Signature)
                        });

                        return Ok(new { Token = handler.WriteToken(securityToken) , ExpiresInMinutes = int.Parse(_config["BearerToken:ExpiryMinutes"]) });
                    }

                    return BadRequest(new { Message = "Incorrect wachtwoord en/of email" });
                }

                return BadRequest(new { Message = "Gebruiker niet gevonden" });
            }
            else
            {
                return BadRequest(new { Message = "Vul een email en wacthwoord in (Email, Password)" });
            }
        }

        [HttpPost]
        [Route("/api/v1/[controller]/SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Je bent successvol uitgelogd"});
        }
    }
}
