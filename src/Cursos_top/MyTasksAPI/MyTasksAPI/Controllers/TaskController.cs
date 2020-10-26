using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyTasksAPI.Models;
using MyTasksAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;

namespace MyTasksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }


        [Authorize]
        [HttpPost("Sync")]
        public IActionResult Sync([FromBody]List<Task> tasks)
        {
            return Ok(_taskRepository.Synchronization(tasks));
        }


        [Authorize]
        [HttpGet("Restoration")]
        public IActionResult Restoration(DateTime date)
        {

            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            return Ok(_taskRepository.Restoration(user, date));

        }
    }
}
