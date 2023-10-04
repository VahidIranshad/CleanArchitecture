using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using CA.Identity.Utility;
using FunctionalTests.Controllers.Common;
using Newtonsoft.Json;
using System.Text;

namespace FunctionalTests.Controllers.Identity
{
    public class RoleClaimControllerTest : BaseControllerTests
    {
        public RoleClaimControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

    }
}
