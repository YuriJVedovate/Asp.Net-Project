using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TalkToAPI.Repositories.Contracts;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;

namespace TalkToAPI.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public UserController(IMapper mapper , IUserRepository userRepository, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _tokenRepository = tokenRepository;

        }

        [Authorize]
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var userAppUser = _userManager.Users.ToList();
            var userListDTO = _mapper.Map<List<ApplicationUser>, List<UserDTO>>(userAppUser);

            foreach (var userDTO in userListDTO)
            {
                userDTO.Links.Add(new LinkDTO("_self", Url.Link("GetUser", new { id = userDTO.Id }), "GET"));
            }

            var list =  new ListDTO<UserDTO>() { List = userListDTO };
            list.Links.Add(new LinkDTO("_self", Url.Link("GetAll", null), "GET"));

            return Ok(list);

        }


        [Authorize]
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetOne(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
                return NotFound();

            var userDTOdb = _mapper.Map<ApplicationUser, UserDTO>(user);
            userDTOdb.Links.Add(new LinkDTO("_self", Url.Link("GetUser", new { id = user.Id }), "GET"));
            userDTOdb.Links.Add(new LinkDTO("_Update", Url.Link("Update", new { id = user.Id }), "PUT"));


            return Ok(userDTOdb);
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
        public IActionResult renew([FromBody] TokenDTO tokenDTO)
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

        [HttpPost(Name = "Register")]
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
                    var userDTOdb = _mapper.Map<ApplicationUser, UserDTO>(user);
                    userDTOdb.Links.Add(new LinkDTO("_self", Url.Link("Register", new { id = user.Id }), "POST"));
                    userDTOdb.Links.Add(new LinkDTO("_Get", Url.Link("GetUser", new { id = user.Id }), "GET"));
                    userDTOdb.Links.Add(new LinkDTO("_Update", Url.Link("Update", new { id = user.Id }), "PUT"));

                    return Ok(userDTOdb);

                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }


        [Authorize]
        [HttpPut("{id}", Name = "Update")]
        public IActionResult Update(string id, [FromBody] UserDTO userDTO)
        {
            ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (user.Id != id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                user.FullName = userDTO.Name;
                user.UserName = userDTO.Email;
                user.Email = userDTO.Email;
                user.Slogan = userDTO.Slogan;

                var result = _userManager.UpdateAsync(user).Result;
                _userManager.RemovePasswordAsync(user);
                _userManager.AddPasswordAsync(user, userDTO.Password);

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
                    var userDTOdb = _mapper.Map<ApplicationUser, UserDTO>(user);
                    userDTOdb.Links.Add(new LinkDTO("_self", Url.Link("Update", new { id = user.Id }), "PUT"));
                    userDTOdb.Links.Add(new LinkDTO("_Get", Url.Link("GetUser", new { id = user.Id }), "GET"));

                    return Ok(userDTOdb);
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Key-Api-Jwt-My-Taks"));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(2);

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: exp,
                    signingCredentials: sign
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var expRefreshToken = DateTime.UtcNow.AddHours(3);

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
