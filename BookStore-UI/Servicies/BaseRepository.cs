using BookStore_UI.Servicies.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_UI.Servicies
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _client;

        public BaseRepository(IHttpClientFactory client)
        {
            _client = client;
        }

        public async Task<bool> Create(string url, T obj)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(obj));

            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return true;

            return false;
        }

        public async Task<bool> Delete(string url, int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NoContent)
                return true;

            return false;
        }

        public async Task<IList<T>> GetAll(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<T>>(content);
                
                return list;
            }

            return null;    
        }

        public async Task<T> GetbyId(string url, int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        public async Task<T> Update(string url, int id, T obj)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url + id);

            request.Content = new StringContent(JsonConvert.SerializeObject(obj),
                                                Encoding.UTF8,
                                                "application/json");

            var client = _client.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }
    }
}
