using System.ComponentModel.DataAnnotations;

namespace SchedualingSystem.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "UserName can't be blank")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone can't be blank")]
        [RegularExpression("^[0-9]*$" , ErrorMessage ="phone number should contains numbers only.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
