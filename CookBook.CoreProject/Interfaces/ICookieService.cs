namespace CookBook.CoreProject.Interfaces
{
    public interface ICookieService
    {
        void WriteHttpOnlyCookie(string key, string value);

        bool TryGetCookie(string key, out string value);
    }
}
