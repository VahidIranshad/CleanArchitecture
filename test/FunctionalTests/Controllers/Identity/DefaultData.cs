using CA.Application.DTOs.Identity.Requests;
using CA.Identity.Utility;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    internal static class DefaultData
    {
        private static object lockObject = new object();
        private static bool isCreated = false;
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
            lock (lockObject)
            {
                if (isCreated)
                {
                    return request;
                }
                var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response1 = client.PostAsync("/api/identity/user", stringContent).Result;
                response1.EnsureSuccessStatusCode();
                isCreated = true;
                return request;

            }
        }
    }
}
