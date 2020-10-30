using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TalkToAPI.Models;
using TalkToAPI.Repositories;
using TalkToAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TalkToAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _tokenRepository = tokenRepository;

        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserDTO userDTO)
        {
            ModelState.Remove("Name");
            ModelState.Remove("PassConfirm");

            if (ModelState.IsValid)
            {
                ApplicationUser user = _userRepository.Get(userDTO.Email, userDTO.Password);
                if (user != null)
                {
                    return crudToken(user);
                }
                else
                {
                    return NotFound("User not found!");
                }

            }
            else
            {
                return UnprocessableEntity("acesso negado " + ModelState);
            }
        }

        [HttpPost("renew")]
        public IActionResult renew([FromBody] TokenDTO tokenDTO )
        {
            var refreshTokenDB = _tokenRepository.GetToken(tokenDTO.RefreshToken);
            if (refreshTokenDB == null)
                return NotFound();

            refreshTokenDB.Update = DateTime.Now;
            refreshTokenDB.Used = true;
            _tokenRepository.UpdateToken(refreshTokenDB);


            var user = _userRepository.Get(refreshTokenDB.UserId);

            return crudToken(user);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.FullName = userDTO.Name;
                user.UserName = userDTO.Email;
                user.Email = userDTO.Email;
                var result = _userManager.CreateAsync(user, userDTO.Password).Result;

                if (!result.Succeeded)
                {
                    List<string> sb = new List<string>();
                    foreach (var erro in result.Errors)
                    {
                        sb.Add(erro.Description);
                    }
                    return UnprocessableEntity(sb);
                }
                else
                {
                    return Ok(user);
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        private TokenDTO BuildTolken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( "Key-Api-Jwt-My-Taks" ));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddMinutes(2);

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: exp,
                    signingCredentials: sign
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var expRefreshToken = DateTime.UtcNow.AddMinutes(3);

            var refreshToken = Guid.NewGuid().ToString();

            var tokenDTO = new TokenDTO { Token = tokenString, expires = exp, ExpirationRefreshToken = expRefreshToken, RefreshToken = refreshToken };

            return tokenDTO;
        }

        private IActionResult crudToken(ApplicationUser user)
        {
            var token = BuildTolken(user);

            var tokenModel = new Token()
            {
                RefreshToken = token.RefreshToken,
                ExpirationToken = token.expires,
                ExpirationRefreshToken = token.ExpirationRefreshToken,
                User = user,
                Insert = DateTime.Now,
                Used = false
            };

            _tokenRepository.RegisterToken(tokenModel);

            return Ok(token);
        }
    }
}
