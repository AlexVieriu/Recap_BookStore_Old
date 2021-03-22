using BookStore_UI.Models;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies.Contracts
{
    public interface IAuthentificationRepository
    {
        Task<bool> Register(RegisterModel model);
        Task<bool> Login(LoginModel model);
        Task Logout();
    }
}
