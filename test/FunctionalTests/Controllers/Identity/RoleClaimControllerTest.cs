using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;

namespace FunctionalTests.Controllers.Identity
{
    public class RoleClaimControllerTest : BaseControllerTests
    {
        public RoleClaimControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAll_ReturnsList_ReturnData()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/roleClaim");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<RoleClaimResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
        }

        [Fact]
        public async Task GetAllByRoleId_ReturnsList_ReturnData()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/roleClaim?roleId={CA.Domain.Constants.Identity.RoleConstants.AdministratorRoleID}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<RoleClaimResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
        }

        [Fact]
        public async Task GetAllClaims_ReturnsList_ReturnData()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/roleClaim/GetAllClaims");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Claims>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
        }

    }
}
