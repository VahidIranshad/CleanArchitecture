using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

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

        [Fact]
        public async Task Create_WithMock_ReturnsNoContent()
        {
            var request = new CreateRoleClaim
            {
                RoleId = CA.Domain.Constants.Identity.RoleConstants.DefaultRoleID,
                Value = CA.Domain.Constants.Permission.Permissions.AllPermision.GetAllPermision().First().Id,

            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var client = this.GetNewClientByAdminAuthorization<IRoleClaimService>(mock => {
                mock.Setup(p => p.SaveAsync(It.IsAny<RoleClaimRequest>())).Returns(Task.FromResult(string.Empty));
                mock.Setup(p => p.HasPermission(It.IsAny<List<string>>(), It.IsAny<List<string>>())).Returns(Task.FromResult(true));
                mock.Setup(p => p.HasPermission(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(Task.FromResult(true));
            });
            var response = await client.PostAsync($"/api/identity/roleClaim", stringContent);
            response.EnsureSuccessStatusCode();

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode);
        }

        [Fact]
        public async Task Delete_WithMock_ReturnsNoContent()
        {

            var mockOilRepository = new Mock<IRoleClaimService>().Setup(p => p.DeleteAsync(It.IsAny<int>())).Returns(Task.FromResult(string.Empty));
            var client = this.GetNewClientByAdminAuthorization<IRoleClaimService>(mock => {
                mock.Setup(p => p.DeleteAsync(It.IsAny<int>())).Returns(Task.FromResult(string.Empty));
                mock.Setup(p => p.HasPermission(It.IsAny<List<string>>(), It.IsAny<List<string>>())).Returns(Task.FromResult(true));
                mock.Setup(p => p.HasPermission(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(Task.FromResult(true));
            });
            var response = await client.DeleteAsync($"/api/identity/roleClaim/{1}");
            response.EnsureSuccessStatusCode();

            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode);
        }

    }
    
}
