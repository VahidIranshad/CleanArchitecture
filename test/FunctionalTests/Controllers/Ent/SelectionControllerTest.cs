using CA.Application.DTOs.Ent;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;

namespace FunctionalTests.Controllers.Ent
{
    public class SelectionControllerTest : BaseControllerTests
    {
        public SelectionControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Theory]
        [InlineDataAttribute("pageNumber=0&pageSize=15", 1)]
        [InlineDataAttribute("pageNumber=1&pageSize=2", 1)]
        [InlineDataAttribute("filter=Title~=A", 1)]
        [InlineDataAttribute("filter=ID>1", 0)]
        [InlineDataAttribute("filter=ID==1", 1)]
        public async Task GetData_ReturnsListByFilter(string query, int expected)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/Selection/GetList?{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ListByCount<SelectionDto>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.DataList.ToList().Count == expected);
        }

        [Fact]
        public async Task GetDataByID_ReturnsOneRecord()
        {
            int id = 1;
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/Selection/{id}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SelectionDto>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.Equal(id, result.Id);
            Assert.NotNull(result.Title);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(35)]
        public async Task GetDataByID_ReturnsNotFound(int id)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/Selection/{id}");
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode);
        }
    }
}
