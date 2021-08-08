namespace BookStore_UI_WASM.Static
{
    public static class Endpoints
    {
#if DEBUG
        public static string BaseUrl = "https://localhost:44369";
#else
        public static string BaseUrl = "https://bookstore-api20201128151949.azurewebsites.net/";
#endif
        public static string AuthorUrl = $"{BaseUrl}/api/authors/";
        public static string BookUrl = $"{BaseUrl}/api/books/";

        public static string RegisterUrl = $"{BaseUrl}/api/users/register/";
        public static string LoginrUrl = $"{BaseUrl}/api/users/login/";
    }
}
