using System.Collections.Generic;

namespace CA.Application.DTOs.Identity.Responses
{
    public class GetAllUsersResponse
    {
        public IEnumerable<UserResponse> Users { get; set; }
    }
}