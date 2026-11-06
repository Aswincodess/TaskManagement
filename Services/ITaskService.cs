using TaskManagement.Models;
using TaskManagement.DTO;



namespace TaskManagement.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks(int userId, string role);

        TaskItem GetTaskById(int id, int userId, string role);

        TaskItem AddTask(TaskDTO taskDTO, int userId);

        bool UpdateTask(int id, TaskDTO taskDTO, int userId, string role);

        bool DeleteTask(int id);
    } 
}
