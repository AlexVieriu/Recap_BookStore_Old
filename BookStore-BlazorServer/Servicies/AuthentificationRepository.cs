using BookStore_UI.Models;
using BookStore_UI.Servicies.Contracts;
using System.Net.Http;

namespace BookStore_UI.Servicies
{
    public class AuthentificationRepository : BaseRepository<RegisterModel>, IAuthentificationRepository
    {
        public AuthentificationRepository(IHttpClientFactory clientFactory)
            : base(clientFactory)
        {

        }
    }
}
