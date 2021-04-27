using Blazored.LocalStorage;
using BookStore_UI.Models;
using BookStore_UI.Servicies.Contracts;
using System.Net.Http;

namespace BookStore_UI.Servicies
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthorRepository(IHttpClientFactory httpClient, ILocalStorageService localStorage)
            :base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
    }
}
