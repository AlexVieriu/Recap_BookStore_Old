using Blazored.LocalStorage;
using BookStore_UI_WASM.Models;
using BookStore_UI_WASM.Servicies.Contracts;
using System.Net.Http;

namespace BookStore_UI_WASM.Servicies
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly HttpClient _factory;
        private readonly ILocalStorageService _localStorage;

        public BookRepository(HttpClient factory, ILocalStorageService localStorage)
            :base(factory, localStorage)            
        {
            _factory = factory;
            _localStorage = localStorage;
        }

    }
}
