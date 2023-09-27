using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchedualingSystem.Models.DTO.Response;
namespace SchedualingSystem.Models.DTO
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        [Range(1, 100, ErrorMessage = "age must be bigger than 1")]
        public int Age { get; set; }
        // return tasks of this users 
        public ICollection<TaskResponse> AssignedTasks { get; set; }  
        public ICollection<TaskResponse> CreatedTasks { get; set;  }
        public UserResponse(string name , string email ,DateTime birthdate ,
                                ICollection<TaskResponse> assignedTasks, ICollection<TaskResponse> createdTasks , Guid id )
        {
            this.Age = DateTime.Now.Year - birthdate.Year;
            this.Name = name;
            this.Email = email;
            this.AssignedTasks = new List<TaskResponse>();
            this.AssignedTasks = assignedTasks;
            this.CreatedTasks = createdTasks;
            this.Id = id; 
        }

        public override bool Equals(object? obj)
        {
            if( obj == null) return false;
            if(obj.GetType() != typeof(UserResponse))   
                return false;
            UserResponse newUser = (UserResponse) obj;
            return Id == newUser.Id && Name == newUser.Name && Email == newUser.Email 
                && Age == newUser.Age ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class UserExtensions
    {
        public static UserResponse ToUserResponse(this User user, ICollection<TaskResponse> assignedTasks,
                        ICollection<TaskResponse> createdTasks )
        {
            return new UserResponse(user.Name, user.Email, user.DateofBirth,
                      assignedTasks, createdTasks , user.Id ); 
        }

        public static UserGetResponse ToUserGetResponse(this User user)
        {
            return new UserGetResponse(user.Name, user.Email, user.DateofBirth , user.Id );
        }
    }
}
