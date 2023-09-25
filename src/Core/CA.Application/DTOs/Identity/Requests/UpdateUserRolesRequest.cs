using CA.Application.DTOs.Identity.Responses;

namespace CA.Application.DTOs.Identity.Requests
{
    public class UpdateUserRolesRequest
    {
        public string UserId { get; set; }
        public string RoleID { get; set; }
    }
}