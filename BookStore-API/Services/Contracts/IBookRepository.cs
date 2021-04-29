using BookStore_API.Data;
using System.Threading.Tasks;

namespace BookStore_API.Services.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        public Task<string> GetImageFileName(int id);
    }
}
