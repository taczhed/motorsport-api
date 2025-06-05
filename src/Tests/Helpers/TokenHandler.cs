using System.Net.Http.Json;

namespace MotorsportApi.Tests.Helpers
{
    public class AuthClient
    {
        public async Task<string> GetTokenAsync()
        {
            var app = new MotorsportApiWebApplicationFactory();
            var client = app.CreateClient();

            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
            {
                Username = "admin",
                Password = "admin"
            });

            var token = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

            if (token == null || string.IsNullOrEmpty(token.Token))
            {
                throw new InvalidOperationException("Token was not returned from the API.");
            }

            return token!.Token;
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
