namespace BookStore_UI.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44369";

        public static string AuthorUrl = $"{BaseUrl}/api/authors/";
        public static string BookUrl = $"{BaseUrl}/api/books/";

        public static string RegisterUrl = $"{BaseUrl}/api/users/register/";
        public static string LoginrUrl = $"{BaseUrl}/api/users/login/";
    }
}
