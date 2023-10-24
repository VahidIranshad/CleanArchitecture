using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;

namespace FunctionalTests.Controllers.Identity
{
    public class RoleControllerTest : BaseControllerTests
    {
        public RoleControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAll_ReturnsList()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/role");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<RoleResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.Count == 2);
        }
        [Theory]
        [InlineDataAttribute("pageNumber=0&pageSize=15", 2)]
        [InlineDataAttribute("pageNumber=1&pageSize=1", 1)]
        [InlineDataAttribute($"filter=ID~={CA.Domain.Constants.Identity.RoleConstants.AdministratorRoleID}", 1)]
        [InlineDataAttribute($"filter=ID=={CA.Domain.Constants.Identity.RoleConstants.AdministratorRoleID}", 1)]
        public async Task GetData_ReturnsListByFilter(string query, int expected)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/role/GetList?{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ListByCount<RoleResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.DataList.ToList().Count == expected);
        }
        [Fact]
        public async Task GetAll_ReturnsList_ReturnData()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/identity/role");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<RoleResponse>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.ToList().Count == 2);
        }
    }
}
