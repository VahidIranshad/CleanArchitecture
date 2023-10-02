using CA.Application.DTOs.Identity.Requests;
using CA.Identity.Utility;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    internal static class DefaultData
    {
        internal static async Task<RegisterRequest> RegisterUserAsync(HttpClient client)
        {

            var request = new RegisterRequest
            {
                Email = "test@localhost.com",
                UserName = "test@localhost.com",
                Password = AESEncryptDecrypt.EncryptStringAES("P@ssword1"),
                ConfirmPassword = AESEncryptDecrypt.EncryptStringAES("P@ssword1"),
                FirstName = "Vahid",
                LastName = "Iranshad"
            };
            
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/identity/user", stringContent);
            response1.EnsureSuccessStatusCode();
            return request;
        }
        private static async Task DeleteUserAsync(int id, HttpClient client)
        {
            await client.DeleteAsync($"/api/fuz/Alternative/{id}");
        }
    }
}
