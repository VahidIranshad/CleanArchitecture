namespace CA.Application.DTOs.Identity.Responses
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}