using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;

namespace FunctionalTests.Controllers
{
    public class WeatherForecastControllerTest : BaseControllerTests
    {
        public WeatherForecastControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task CheckForRuning_GetString()
        {
            var client = this.GetNewClientWithoutAccess();
            var response = await client.GetAsync($"/api/WeatherForecast");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string>(stringResponse);
            var statusCode = response.StatusCode.ToString();

            Assert.Equal("OK", statusCode);
            Assert.True(result == "Hello");
        }
    }
}
