﻿using Blazored.LocalStorage;
using BookStore_UI.Servicies.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        public BaseRepository(IHttpClientFactory client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
        }

        public async Task<bool> Create(string url, T obj)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(obj),
                                                Encoding.UTF8,
                                                "application/json");

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = await HeaderValue();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
                return true;

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            if (id < 1)
                return false;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = await HeaderValue();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public async Task<IList<T>> GetAll(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var client = _client.CreateClient();
                client.DefaultRequestHeaders.Authorization = await HeaderValue();

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<IList<T>>(content);

                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<T> GetbyId(string url, int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = await HeaderValue();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        public async Task<bool> Update(string url, T obj, int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url + id);

            request.Content = new StringContent(JsonConvert.SerializeObject(obj),
                                                Encoding.UTF8,
                                                "application/json");

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = await HeaderValue();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;

            return false;
        }

        private async Task<AuthenticationHeaderValue> HeaderValue()
        {
            var bearerToken = await _localStorage.GetItemAsStringAsync("authToken");
            return new AuthenticationHeaderValue("bearer", bearerToken);
        }
    }
}
