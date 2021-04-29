using Blazored.LocalStorage;
using BookStore_UI_WASM.Servicies.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BookStore_UI_WASM.Servicies
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        public BaseRepository(HttpClient client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        public async Task<bool> Create(string url, T obj)
        {
            _client.DefaultRequestHeaders.Authorization = await HeaderValue();
            var response = await _client.PostAsJsonAsync<T>(url, obj);

            if (response.StatusCode == HttpStatusCode.Created)
                return true;

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
                return false;

            _client.DefaultRequestHeaders.Authorization = await HeaderValue();
            var response = await _client.DeleteAsync(url + id);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public async Task<IList<T>> GetAll(string url)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = await HeaderValue();
                var response = await _client.GetFromJsonAsync<IList<T>>(url);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<T> GetbyId(string url, int id)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = await HeaderValue();
                var response = await _client.GetFromJsonAsync<T>(url + id);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            if (obj == null)
                return false;

            _client.DefaultRequestHeaders.Authorization = await HeaderValue();

           var response = await _client.PutAsJsonAsync<T>(url+id, obj);

            return response.IsSuccessStatusCode;
        }

        private async Task<AuthenticationHeaderValue> HeaderValue()
        {
            var bearerToken = await _localStorage.GetItemAsStringAsync("authToken");
            return new AuthenticationHeaderValue("bearer", bearerToken);
        }
    }
}
