using BookStore_UI_WASM.Models;
using System.Threading.Tasks;

namespace BookStore_UI_WASM.Servicies.Contracts
{
    public interface IAuthentificationRepository
    {
        Task<bool> Register(RegisterModel model);
        Task<bool> Login(LoginModel model);
        Task Logout();
    }
}
