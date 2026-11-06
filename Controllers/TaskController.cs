
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.DTO;
using TaskManagement.Services;
using System.Security.Claims;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

      
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            string role = User.FindFirst(ClaimTypes.Role)!.Value;

            var tasks = _taskService.GetAllTasks(userId, role);

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            string role = User.FindFirst(ClaimTypes.Role)!.Value;

            var task = _taskService.GetTaskById(id, userId, role);

            if (task == null)
            {
                return NotFound("Task not found or access denied");
            }

            return Ok(task);
        }

        [HttpPost]
        public IActionResult AddTask(TaskDTO taskDTO)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var task = _taskService.AddTask(taskDTO, userId);

            return CreatedAtAction(nameof(GetTaskById),
                new { id = task.Id }, task);
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskDTO taskDTO)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            string role = User.FindFirst(ClaimTypes.Role)!.Value;

            var result = _taskService.UpdateTask(id, taskDTO, userId, role);

            if (!result)
            {
                return NotFound("Task not found or access denied");
            }

            return Ok("Task updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteTask(int id)
        {
            var result = _taskService.DeleteTask(id);

            if (!result)
            {
                return NotFound("Task not found");
            }

            return Ok("Task deleted successfully");
        }
    }
}
