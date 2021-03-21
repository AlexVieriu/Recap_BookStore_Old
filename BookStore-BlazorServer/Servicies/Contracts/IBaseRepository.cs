using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> GetAll(string url);
        Task<T> GetbyId(string url, int id);
        Task<bool> Create(string url, T obj);
        Task<bool> Delete(string url, int id);
        Task<T> Update(string url, int id, T obj);
    }
}
