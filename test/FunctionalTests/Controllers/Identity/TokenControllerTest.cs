using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using CA.Identity.Utility;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    public class TokenControllerTest : BaseControllerTests
    {
        public TokenControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
   
    }
}
