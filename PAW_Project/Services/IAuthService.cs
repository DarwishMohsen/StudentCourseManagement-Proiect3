using Microsoft.AspNetCore.Identity;
using PAW_Project.Models.ViewModels;
using System.Threading.Tasks;

namespace PAW_Project.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}