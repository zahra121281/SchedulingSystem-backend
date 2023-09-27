using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

using SchedualingSystem.Models.Enums; 
namespace SchedualingSystem.Models.DTO.Request
{
    public class UserRequest
    {
        [Required]
        public Guid UserId { get; set; }
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email value should be valid")]
        public string Email { get; set; }
        [Required(ErrorMessage ="birthdate is required")]
        public DateTime DateofBirth { get; set; }
        [Required]
        public GenderOptions Gender { get; set; }
        public UserRequest(string name, DateTime dateofBirth , string email ,GenderOptions gender , Guid userid)
        {
            this.Name = name;
            this.DateofBirth = dateofBirth;
            this.Email = email;
            this.Gender = gender;
            this.UserId = userid;
        }
        public User ToUser() =>
            new User(this.Name , this.DateofBirth , this.Email , this.Gender.ToString());
        
      
    }
}
