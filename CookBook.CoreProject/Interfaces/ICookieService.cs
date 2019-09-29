using System;

namespace CookBook.CoreProject.Interfaces
{
    public interface ICookieService
    {
        void WriteHttpOnlyCookie(string key, string value, DateTime? expiryDate = null);

        bool TryGetCookie(string key, out string value);
    }
}
