using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    public class UserRoleControllerTest : BaseControllerTests
    {
        public UserRoleControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetUserRolesByUserID_ReturnsList_CheckStatusCode()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/userRole/{CA.Domain.Constants.Identity.UserConstants.AdministratorUserID}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<UserRoleModel>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
        }


        [Fact]
        public async Task InsertDeleteUserRole_NoContent_CheckStatusCode()
        {
            var client = this.GetNewClientByAdminAuthorization();

            var request = new UpdateUserRolesRequest
            {
                UserId = SharedDatabaseSetup.Identity.DefaultUser.normalCurrentUser2Service.Object.UserId,
                RoleID = CA.Domain.Constants.Identity.RoleConstants.DefaultRoleID
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/identity/userRole/InsertUserRole", stringContent);
            response.EnsureSuccessStatusCode();
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode);

            var query = $"UserId={SharedDatabaseSetup.Identity.DefaultUser.normalCurrentUser2Service.Object.UserId}&RoleID={CA.Domain.Constants.Identity.RoleConstants.DefaultRoleID}";
            var responseDelete = await client.DeleteAsync($"/api/identity/userRole/DeleteUserRole?{query}");
            responseDelete.EnsureSuccessStatusCode();
            var statusCodeDelete = response.StatusCode.ToString();

            Assert.Equal("NoContent", statusCodeDelete);
        }
    }
}
