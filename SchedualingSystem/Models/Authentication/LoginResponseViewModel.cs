namespace SchedualingSystem.Models.Authentication
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
