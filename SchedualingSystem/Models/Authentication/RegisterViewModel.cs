using System.ComponentModel.DataAnnotations;

namespace SchedualingSystem.Models.Authentication
{
    public class RegisterViewModel
    {
       
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
       
    }
}
