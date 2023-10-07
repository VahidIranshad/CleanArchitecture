using CA.Application.DTOs.Ent.TValue;
using CA.Domain.Base;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Ent
{
    public class TValueControllerTest : BaseControllerTests
    {
        public TValueControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetData_GetAll()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/TValue/GetAll");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<TValueDto>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.Count == 1);
        }

        [Theory]
        [InlineDataAttribute("pageNumber=0&pageSize=15", 1)]
        [InlineDataAttribute("pageNumber=1&pageSize=2", 1)]
        [InlineDataAttribute("filter=ID>1", 0)]
        [InlineDataAttribute("filter=ID==1", 1)]
        public async Task GetData_ReturnsListByFilter(string query, int expected)
        {
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/TValue/GetList?{query}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ListByCount<TValueDto>>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result.DataList.ToList().Count == expected);
        }

        [Fact]
        public async Task GetDataByID_ReturnsOneRecord()
        {
            int id = 1;
            var client = this.GetNewClientByAdminAuthorization();
            var response = await client.GetAsync($"/api/ent/TValue/{id}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TValueDto>(stringResponse);
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
            var response = await client.GetAsync($"/api/ent/TValue/{id}");
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode);
        }

        [Fact]
        public async Task PostData_ReturnsCreatedData()
        {
            var client = this.GetNewClientByAdminAuthorization();

            var request = new TValueDto
            {
                Title = "Test Data",
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/ent/TValue", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var createdID = JsonConvert.DeserializeObject<int>(stringResponse1);
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("OK", statusCode1);

            // Get created data

            var response2 = await client.GetAsync($"/api/ent/TValue/{createdID}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<TValueDto>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(createdID, result2.Id);
            Assert.Equal(request.Title, result2.Title);
            Delete(createdID);
        }

        [Fact]
        public async Task PutData_ReturnsUpdatedData()
        {

            var client = this.GetNewClientByAdminAuthorization();

            // Update Data
            var id = await CreateDataForTest();
            var request = new TValueDto
            {
                Id = id,
                Title = "Test Data",
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PutAsync($"/api/ent/TValue", stringContent);
            response1.EnsureSuccessStatusCode();
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode1);

            // Get updated Data

            var response2 = await client.GetAsync($"/api/ent/TValue/{request.Id}");
            response2.EnsureSuccessStatusCode();

            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonConvert.DeserializeObject<TValueDto>(stringResponse2);
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("OK", statusCode2);
            Assert.Equal(request.Id, result2.Id);
            Assert.Equal(request.Title, result2.Title);

            Delete(id);
        }

        [Fact]
        public async Task DeleteDataById_ReturnsNoContent()
        {
            var client = this.GetNewClientByAdminAuthorization();
            var id = await CreateDataForTest();

            // Delete data

            var response1 = await client.DeleteAsync($"/api/ent/TValue/{id}");
            var statusCode1 = response1.StatusCode.ToString();

            Assert.Equal("NoContent", statusCode1);

            // Get deleted data

            var response2 = await client.GetAsync($"/api/ent/TValue/{id}");
            var statusCode2 = response2.StatusCode.ToString();

            Assert.Equal("NotFound", statusCode2);
        }

        [Theory]
        [InlineData("Delete", "/api/ent/TValue/", 1)]
        public async Task Data_CheckAcess_ThrowException(string type, string url, object parameter)
        {
            var client = this.GetNewClientWithoutAccess();
            HttpResponseMessage responseMessage;
            switch (type)
            {
                case "Delete":
                    responseMessage = await client.DeleteAsync($"{url}{parameter}");
                    break;
                default:
                    responseMessage = await client.DeleteAsync($"{url}{parameter}");
                    break;
            }
            var statusCode = responseMessage.StatusCode.ToString();


            Assert.Equal("Forbidden", statusCode);
        }
        #region Private Methode
        private async Task<int> CreateDataForTest()
        {
            var client = this.GetNewClientByAdminAuthorization();

            var request = new TValueDto
            {
                Title = "Test Data",
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response1 = await client.PostAsync("/api/ent/TValue", stringContent);
            response1.EnsureSuccessStatusCode();

            var stringResponse1 = await response1.Content.ReadAsStringAsync();
            var createdID = JsonConvert.DeserializeObject<int>(stringResponse1);
            return createdID;
        }
        private async Task Delete(int id)
        {
            var client = this.GetNewClientByAdminAuthorization();
            await client.DeleteAsync($"/api/ent/TValue/{id}");
        }
        #endregion
    }
}
