using Blazored.LocalStorage;
using BookStore_UI.Models;
using BookStore_UI.Servicies.Contracts;
using BookStore_UI.Static;
using BookStore_UI.Providers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies
{
    public class AuthentificationRepository : IAuthentificationRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly ApiAuthenticationStateProvider _authStateProvider;

        public AuthentificationRepository(IHttpClientFactory httpClient,
                                          ILocalStorageService localStorage,
                                          ApiAuthenticationStateProvider authStateProvider)            
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            HttpRequestMessage request = new(HttpMethod.Post, Endpoints.LoginrUrl);

            request.Content = new StringContent(JsonConvert.SerializeObject(user),
                                                Encoding.UTF8,
                                                "application/json");

            var client = _httpClient.CreateClient();
            
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode == false)
                return false;

            // get the Token from the response
                        
            //var content = await response.Content.ReadAsStringAsync();
            var content = "{\"token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb2RhbmdhamF0IjoiMzI4OCIsInN1YiI6InZBbGV4IiwiZW1haWwiOiJhbGV4QGdtYWlsLmNvbSIsInJvbGUiOiJBZG1pbiIsIkRhdGUiOiIxNS0wOS0yMDIxIDE0OjA0OjE3IiwianRpIjoiNWNhN2FhNzktNTRiNC00Y2FkLWIxMWEtNWU3NzE5MWVmYWUxIiwiZXhwIjoxNjMxNzA3NDU3LCJpc3MiOiJQZXNvbmFsQWNjaWRlbnRAZ2FyYW50YS5jb20iLCJhdWQiOiJQZXNvbmFsQWNjaWRlbnRZ2FyYW50YS5jb20ifQ.yQKccCQVuGFw6_RInGfX8jnzNFC6h36HI9VMXHrOHa4\"}";
            var token =  JsonConvert.DeserializeObject<TokenResponse>(content);
            

            // Store token
            await _localStorage.SetItemAsync("authToken", token.Token);

            // Change State of the app
            await _authStateProvider.LoggedIn();

            client.DefaultRequestHeaders.Authorization =
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
            HttpRequestMessage request = new(HttpMethod.Post, Endpoints.RegisterUrl);

            request.Content = new StringContent(JsonConvert.SerializeObject(user),
                                                Encoding.UTF8,
                                                "application/json");

            var client = _httpClient.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

    }
}
