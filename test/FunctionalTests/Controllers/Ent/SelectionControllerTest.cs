using CA.Application.DTOs.Ent.Selection;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

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
        [Fact]
        public async Task PostData_ReturnsCreatedData()
        {
            var client = this.GetNewClientByAdminAuthorization();

            var request = new SelectionCreateDto
            {
                Title = "Test Data",
                SelectionType = "Test Data",
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/ent/Selection", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var createdID = JsonConvert.DeserializeObject<int>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("OK", statusCode1);

            // Get created data

            var response2 = await client.GetAsync($"/api/ent/Selection/{createdID}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<SelectionDto>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(createdID, result2.Id);
            Assert.Equal(request.Title, result2.Title);
            Assert.Equal(request.SelectionType, result2.SelectionType);
        }
    }
}
