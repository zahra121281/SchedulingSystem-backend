using SchedualingSystem.Models.DTO.Response;
using System.ComponentModel.DataAnnotations;
namespace SchedualingSystem.Models.DTO
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string ManagerName { get; set; }
        public double PercentageDone { get; set; }
        public double PercentageLeft { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DoneDate { get; set; }
        public ICollection<UserGetResponse> Employees { get; set; }
        public TaskResponse(string description , string managerName , double Done , 
                            double Left , DateTime deadline ,DateTime doneDate,Guid id ,ICollection<UserGetResponse> employees)
        {
            this.Description = description;
            this.ManagerName = managerName;
            this.PercentageLeft = Left; 
            this.Deadline = deadline;
            this.DoneDate = doneDate;
            this.PercentageDone = Done;
            this.Id = id; 
            this.Employees = employees;
        }
    }

    public static class TaskExtensions
    {
        public static TaskResponse ToTasksResponse(this Task Task, string ManagerName) =>
            new TaskResponse(Task.Description, ManagerName, Task.PercentageDone, Task.PercentageLeft,
                    Task.Deadline, Task.DoneDate, Task.Id, Task.Employees.Select(x => x.ToUserGetResponse()).ToList()); //, employees );
        //public static TaskGetResponse ToTasksGetResponse(this Task Task, string ManagerName) =>
        //    new TaskGetResponse(Task.Description, ManagerName, Task.PercentageDone, Task.PercentageLeft,
        //            Task.Deadline, Task.DoneDate, Task.Id);, ICollection<UserResponse> employees

    }
}
