
using TaskManagement.Models;
using TaskManagement.DTO;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagement.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();

        private int _nextId = 1;

      
        public List<TaskItem> GetAllTasks(int userId, string role)
        {
            if (role == "Admin")
            {
                return _tasks;
            }

            return _tasks.Where(t => t.UserId == userId).ToList();
        }

        
        public TaskItem? GetTaskById(int id, int userId, string role)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return null;
            }

            if (role == "Admin" || task.UserId == userId)
            {
                return task;
            }

            return null;
        }

        
        public TaskItem AddTask(TaskDTO taskDTO, int userId)
        {
            var task = new TaskItem
            {
                Id = _nextId++,
                Title = taskDTO.Title,
                Description = taskDTO.Description,
                Status = taskDTO.Status,
                UserId = userId
            };

            _tasks.Add(task);

            return task;
        }

       
        public bool UpdateTask(int id, TaskDTO taskDTO, int userId, string role)
        {
            var task = GetTaskById(id, userId, role);

            if (task == null)
            {
                return false;
            }

            task.Title = taskDTO.Title;
            task.Description = taskDTO.Description;
            task.Status = taskDTO.Status;

            return true;
        }

 
        public bool DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return false;
            }

            _tasks.Remove(task);

            return true;
        }
    }
}
