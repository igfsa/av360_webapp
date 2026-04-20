using Microsoft.AspNetCore.Http;

namespace Application.Helpers;

public class CookiesHelper
{
    public static void SetCookies(HttpResponse response, string accessToken, string refreshToken)
    {
        response.Cookies.Append("auth_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(30),
            Path = "/",
            SameSite = SameSiteMode.Lax,
            Secure = false
        });

        response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(30),
            Path = "/",
            SameSite = SameSiteMode.Lax,
            Secure = false
        });
    }
}