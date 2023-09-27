using SchedualingSystem.Models.DTO.Request;
using SchedualingSystem.Models.DTO.Response;
using SchedualingSystem.Models.DTO;
using SchedualingSystem.Models;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SchedualingSystem.Service
{
    public class TaskService
    {
        private readonly AppDbContext _db ;
        //private readonly List<Manager> _Managers = new List<Manager>();  
        //private readonly List<Employee> _Employees = new List<Employee>();
        // add Tasks
        public TaskService(AppDbContext appcontext)
        {
            _db = appcontext;
        }
        public async System.Threading.Tasks.Task<TaskResponse> AddTask(TaskRequest TaskRequest
                                                                ,string managerName ,Guid managerId)
        {
            if (TaskRequest == null)
            {
                throw new ArgumentNullException("Task could not be null");
            }

            ValiadationModel.ValidateModel(TaskRequest);

            var Task = TaskRequest.ToTask();
            Task.Id = Guid.NewGuid();
            Task.ManagerId = managerId; 
            this._db.Tasks.Add(Task);
            await this._db.SaveChangesAsync();
            return Task.ToTasksResponse(managerName); 
        }
        public async System.Threading.Tasks.Task<TaskResponse> GetTaskByid(Guid id ,string managerName )
        {
            if (id != null)
            {
                SchedualingSystem.Models.Task? Task = await this._db.Tasks.Include(x => x.Employees )
                                            .AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
                if (Task == null)
                {
                    throw new InvalidOperationException("there is not such a Task!");
                }
                return Task.ToTasksResponse(managerName) ;
            }
            else
                throw new InvalidOperationException("id could not be null or empty");
        }
        public SchedualingSystem.Models.Task InternallGetTaskByid(Guid id)
        {
            if (id != null)
            {
                SchedualingSystem.Models.Task? Task = this._db.Tasks.Include(t => t.Employees)
                                                            .Single(x => x.Id == id); 
                                           // .AsNoTracking().SingleOrDefault(x => x.Id == id);
                if (Task == null)
                {
                    throw new InvalidOperationException("there is not such a Task!");
                }
                return Task;
            }
            else
                throw new InvalidOperationException("id could not be null or empty");
        }

        public async System.Threading.Tasks.Task<bool> DeleteTask(Guid TaskID)
        {
            if (TaskID == null)
            {
                throw new InvalidOperationException(nameof(TaskID));
            }
            var Task =  InternallGetTaskByid(TaskID);
            if (Task != null)
            {
                this._db.Tasks.Attach(Task);
                this._db.Tasks.Remove(Task);
                await this._db.SaveChangesAsync(); 
                return true;
            }
            else
                return false;
        }

        public async System.Threading.Tasks.Task<bool> DeleteManagerTasks(string managerName , Guid managerId)
        {
            if (managerId == null)
            {
                throw new InvalidOperationException(nameof(managerId));
            }
            var Task = await GetAllManagerTasks(managerId , managerName);
            foreach(var task in Task)
            {
                var b = await DeleteTask(task.Id);
                if (!b)
                    return b; 
            }
            return true;
           
        }

        public async System.Threading.Tasks.Task<TaskResponse> UpdateTask(TaskRequest TaskRequest 
                                                    , string managerName , Guid id )
        {
            if (TaskRequest == null)
                throw new ArgumentNullException("requested Task could not be null");

            ValiadationModel.ValidateModel(TaskRequest);

            //  get matching person object 
            var matchingTask =  InternallGetTaskByid( id);
            if (matchingTask == null)
            {
                throw new InvalidOperationException("there is not such a Task!");
            }
            // update all details 
            this._db.Tasks.Attach(matchingTask);
            matchingTask.Description = TaskRequest.Description;
            matchingTask.DoneDate = TaskRequest.DoneDate;
            matchingTask.PercentageDone = TaskRequest.PercentageDone;
            matchingTask.PercentageLeft = 100-TaskRequest.PercentageLeft;
            matchingTask.Deadline = TaskRequest.Deadline; 
            this._db.Tasks.Update(matchingTask);
            await this._db.SaveChangesAsync();
            return matchingTask.ToTasksResponse(managerName );
        }
        public async System.Threading.Tasks.Task<List<TaskResponse>> GetAllManagerTasks(Guid ManagerId,string managerName ) =>await
            this._db.Tasks.Include(t=> t.Employees ).Where(t => t.ManagerId == ManagerId).Select(t => t.ToTasksResponse(managerName)).ToListAsync();



        public async System.Threading.Tasks.Task<List<TaskResponse>> SearchTaskBy(string Searchby, 
                                                string SearchItem,Guid managerId , string managerName)
        {
            var ManagerTasks = await this._db.Tasks.Where(t => t.ManagerId == managerId).ToListAsync();
            List<TaskResponse> matchingTasks = null;
            switch (Searchby)
            {
                case nameof(TaskRequest.Description):
                    matchingTasks =  ManagerTasks.Where(t => t.Description.Contains(SearchItem) ).
                            Select(t => t.ToTasksResponse(managerName)).ToList();
                    break;

                case nameof(TaskRequest.Deadline ):
                    matchingTasks = ManagerTasks.Where( t=> t.Deadline.ToString().Equals(SearchItem))
                            .Select(t => t.ToTasksResponse(managerName) ).ToList();
                    break;

                default:
                    throw new InvalidOperationException("there is no such a Task");
            }
            return matchingTasks;
        }

        public async System.Threading.Tasks.Task<TaskResponse> ChangePercentage(double percentage ,
                                                                        Guid taskId,string managerName)
        {
            var task =  InternallGetTaskByid(taskId);
            if (task != null)
            {
                this._db.Tasks.Attach(task);
                task.PercentageDone = percentage;
                task.PercentageLeft = 100 - percentage;
                this._db.Tasks.Update(task);
                await _db.SaveChangesAsync();
                return task.ToTasksResponse(managerName);
            }
            else
                throw new InvalidOperationException("there is no such a Task");
        }

        //public async Task AssigneUsesrToTask(User user , Guid taskid)
        //{
        //    var task =  InternallGetTaskByid(taskid );
        //    if (task != null)
        //    {
        //        task.Employees.Add(user);
        //    }
        //    else
        //        throw new InvalidOperationException("there is not such a task ");
        //}
    }
}
