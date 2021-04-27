using Blazored.LocalStorage;
using BookStore_UI.Models;
using BookStore_UI.Servicies.Contracts;
using System.Net.Http;

namespace BookStore_UI.Servicies
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILocalStorageService _localStorage;

        public BookRepository(IHttpClientFactory factory, ILocalStorageService localStorage)
            :base(factory, localStorage)            
        {
            _factory = factory;
            _localStorage = localStorage;
        }

    }
}
