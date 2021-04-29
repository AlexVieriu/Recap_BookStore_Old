using Blazored.LocalStorage;
using BookStore_UI_WASM.Models;
using BookStore_UI_WASM.Servicies.Contracts;
using BookStore_UI_WASM.Static;
using BookStore_UI_WASM.Providers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace BookStore_UI_WASM.Servicies
{
    public class AuthentificationRepository : IAuthentificationRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthentificationRepository(HttpClient client,
                                          ILocalStorageService localStorage,
                                          ApiAuthenticationStateProvider authStateProvider)            
        {
            _client = client;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            var response = await _client.PostAsJsonAsync(Endpoints.LoginrUrl, user);
            
            if (response.IsSuccessStatusCode == false)
                return false;

            // get the Token from the response
            var content = await response.Content.ReadAsStringAsync();
            var token =  JsonConvert.DeserializeObject<TokenResponse>(content);

            // Store token
            await _localStorage.SetItemAsync("authToken", token.Token);

            // Change State of the app
            await _authStateProvider.LoggedIn();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", token.Token);

            return true;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _authStateProvider.LoggedOut(); 
        }

        public async Task<bool> Register(RegisterModel user)
        {
            var response = await _client.PostAsJsonAsync(Endpoints.RegisterUrl, user);

            return response.IsSuccessStatusCode;
        }

    }
}
