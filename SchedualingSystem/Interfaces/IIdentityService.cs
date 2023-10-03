using SchedualingSystem.Models.Authentication;

namespace SchedualingSystem.Interfaces
{
    public interface IIdentityService
    {
        Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel);
        Task<ResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel);
        Task<ResponseViewModel> DeleteAsync(Guid id );
        Task<ResponseViewModel> RegisterAdminAsync(RegisterViewModel registerViewModel);
    }
}

