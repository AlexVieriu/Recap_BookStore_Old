using Blazored.LocalStorage;
using BookStore_UI_WASM.Models;
using BookStore_UI_WASM.Servicies.Contracts;
using System.Net.Http;

namespace BookStore_UI_WASM.Servicies
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthorRepository(HttpClient httpClient, ILocalStorageService localStorage)
            :base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
    }
}
