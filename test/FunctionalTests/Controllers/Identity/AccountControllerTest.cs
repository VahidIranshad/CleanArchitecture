using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    public class AccountControllerTest : BaseControllerTests
    {
        public AccountControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task UpdateProfile_ReturnContent()
        {
            var client = this.GetNewClientByAdminAuthorization();

            // Update Data
            var data = await DefaultData.RegisterUserAsync(client);
            var request = new UpdateProfileRequest
            {
                FirstName = "Vahid",
                LastName = "Iranshad",
                Email = data.Email,
                PhoneNumber = "989108610052"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PutAsync($"/api/identity/account/UpdateProfile", stringContent);
            response1.EnsureSuccessStatusCode();
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode1);

            // Get updated Data

            var response2 = await client.GetAsync($"/api/identity/user/GetByEmail?email={request.Email}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<UserResponse>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(request.FirstName, result2.FirstName);
            Assert.Equal(request.LastName, result2.LastName);
            Assert.Equal(request.PhoneNumber, result2.PhoneNumber);

            //Delete(id);
        }
     
    }
}
