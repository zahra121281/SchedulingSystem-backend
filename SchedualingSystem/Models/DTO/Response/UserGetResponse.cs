using System.ComponentModel.DataAnnotations;

namespace SchedualingSystem.Models.DTO.Response
{
    public class UserGetResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        [Range(1, 100, ErrorMessage = "age must be bigger than 1")]
        public int Age { get; set; }
        // return tasks of this users 
        public UserGetResponse(string name, string email, DateTime birthdate,Guid id )
        {
            this.Age = DateTime.Now.Year - birthdate.Year;
            this.Name = name;
            this.Email = email;
            this.Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(UserResponse))
                return false;
            UserResponse newUser = (UserResponse)obj;
            return Id == newUser.Id && Name == newUser.Name && Email == newUser.Email
                && Age == newUser.Age;
        }
    }
}
