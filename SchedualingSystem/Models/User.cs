using SchedualingSystem.Models.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedualingSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Email value should be valid")]
        public string Email { get; set; }
        //public string Password { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Gender { get; set; }  
        public virtual ICollection<Task> Tasks { get; set; }
        public User(string name , DateTime dateofBirth, string email,string gender ) 
        { 
            this.Name = name;
            this.DateofBirth = dateofBirth;
            this.Email = email;
            this.Gender = gender;
            this.Tasks = new HashSet<Task>();   
        }
        
    }
}
