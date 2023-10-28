using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    public class UserControllerTest : BaseControllerTests
    {
        public UserControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetAll_ReturnsList()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/user");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<UserResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
        }
        [Theory]
        [InlineDataAttribute($"filter=username=={CA.Domain.Constants.Identity.UserConstants.AdministratorUser}", 1)]
        [InlineDataAttribute($"filter=ID=={CA.Domain.Constants.Identity.UserConstants.AdministratorUserID}", 1)]
        public async Task GetData_ReturnsListByFilter(string query, int expected)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/user/GetList?{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ListByCount<UserResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.DataList.ToList().Count == expected);
        }
        [Theory]
        [InlineDataAttribute($"email={CA.Domain.Constants.Identity.UserConstants.AdministratorUser}", CA.Domain.Constants.Identity.UserConstants.AdministratorUserID)]
        public async Task GetData_GetByEmail_ReturnUserResponse(string query, string expectedID)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/user/GetByEmail?{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.Id == expectedID);
        }
        [Theory]
        [InlineDataAttribute($"{CA.Domain.Constants.Identity.UserConstants.AdministratorUserID}", CA.Domain.Constants.Identity.UserConstants.AdministratorUser)]
        public async Task GetData_GetByID_ReturnUserResponse(string query, string expectedUsername)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/user/{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.UserName == expectedUsername);
        }
        [Theory]
        [InlineDataAttribute($"{CA.Domain.Constants.Identity.UserConstants.AdministratorUserID}", CA.Domain.Constants.Identity.UserConstants.AdministratorUser)]
        public async Task ResetPass_ReturnNoContent(string query, string expectedUsername)
        {
            var client = this.GetNewClientByAdminAuthorization(); 
            var request = new ResetPasswordRequest
            {
                ID = "9e224968-33e4-4652-b7b7-8574d048cdb9",
                
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = client.PutAsync("/api/identity/user/reset-password", stringContent).Result;
            response.EnsureSuccessStatusCode();

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode);
        }

        [Fact]
        public async Task GetAllData_ReturnUnauthorized()
        {
            var client = this.GetNewClientByAdminAuthorization();
            client.DefaultRequestHeaders.Authorization = null;

            var response = await client.GetAsync($"/api/identity/user");
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("Unauthorized", statusCode);
        }
    }
}
