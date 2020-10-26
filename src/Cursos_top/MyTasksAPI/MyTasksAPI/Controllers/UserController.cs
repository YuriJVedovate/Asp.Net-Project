using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTasksAPI.Models;
using MyTasksAPI.Repositories.Contracts;
using System.Collections.Generic;

namespace MyTasksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IUserRepository userRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
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
                    _signInManager.SignInAsync(user, false);

                    return Ok("acesso permitido");
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
    }
}
