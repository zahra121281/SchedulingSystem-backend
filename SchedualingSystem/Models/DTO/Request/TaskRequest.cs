using System.ComponentModel.DataAnnotations;

namespace SchedualingSystem.Models.DTO.Request
{
    public class TaskRequest
    {
        [Required]
        public Guid Id { get; set; }    
        public string Description { get; set; }
        [Required(ErrorMessage ="could not make a task without providing its Manager Id.")]
        public Guid ManagerId { get; set; }
        public double PercentageDone { get; set; }
        public double PercentageLeft { get; set; }
        [Required]
        public DateTime Deadline { get; set; }
        public DateTime DoneDate { get; set; }

        public TaskRequest(string description, Guid managerid, DateTime deadline  , Guid id ,double percentageDone ,
                                    double percentageLeft , DateTime doneDate )
        {
            this.Description = description;
            this.ManagerId = managerid;
            this.Deadline = deadline;
            this.PercentageDone = 0; 
            this.PercentageLeft = 100 ;
            this.Id = id;
        }
        public Task ToTask()
        {
            return new Task(this.Description , this.ManagerId , this.Deadline , this.PercentageDone , this.PercentageLeft );
        }
    }
}
