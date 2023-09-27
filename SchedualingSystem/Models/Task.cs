using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SchedualingSystem.Models.DTO;


namespace SchedualingSystem.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }
        [Required]
        public Guid ManagerId { get; set; }
        
        [Range(0 , 100 )]
        public double PercentageDone { get; set; }
        [Range(0, 100)]
        public double PercentageLeft { get; set; } 
        public DateTime Deadline { get; set; } 
        public DateTime DoneDate { get; set; } 
        public virtual ICollection<User> Employees { get;set  ; }
        public Task(string description , Guid managerId  , DateTime deadline 
                                                    , double percentageDone , double percentageLeft)
        {
            this.Description = description;
            this.ManagerId = managerId;
            this.Deadline = deadline;
            this.PercentageDone = percentageDone ; 
            this.PercentageLeft = 100 - this.PercentageDone ;
            this.Employees = new HashSet<User>();
         
        }
      
    }
}
