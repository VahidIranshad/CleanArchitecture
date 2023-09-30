using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Identity.Utility;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FunctionalTests.Controllers.Common
{
    public class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private static string _tokenForAdmin;
        private static string _tokenForOrdinaryUser;
        public HttpClient GetNewClientByAdminAuthorization()
        {
            var newClient = _factory.WithWebHostBuilder(builder =>
            {
                _factory.CustomConfigureServices(builder);
            }).CreateClient();

            if (_tokenForAdmin == null)
            {
                var request = new TokenRequest
                {
                    Email = AESEncryptDecrypt.EncryptStringAES("admin@localhost.com"),
                    Password = AESEncryptDecrypt.EncryptStringAES("P@ssword1")
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = newClient.PostAsync($"/api/identity/Token/Login", stringContent).Result;
                response.EnsureSuccessStatusCode();

                var stringResponse = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TokenResponse>(stringResponse);
                _tokenForAdmin = result.Token;
            }

            newClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", _tokenForAdmin);

            return newClient;
        }
        public HttpClient GetNewClientWithoutAccess()
        {
            var newClient = _factory.WithWebHostBuilder(builder =>
            {
                _factory.CustomConfigureServices(builder);
            }).CreateClient();

            if (_tokenForOrdinaryUser == null)
            {
                var request = new TokenRequest
                {
                    Email = AESEncryptDecrypt.EncryptStringAES("user@localhost.com"),
                    Password = AESEncryptDecrypt.EncryptStringAES("P@ssword1")
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = newClient.PostAsync($"/api/identity/Token/Login", stringContent).Result;
                response.EnsureSuccessStatusCode();

                var stringResponse = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TokenResponse>(stringResponse);
                _tokenForOrdinaryUser = result.Token;
            }

            newClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", _tokenForOrdinaryUser);

            return newClient;
        }
    }
}
