using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.User.Queries
{
    public class GetUserRolesByUserIDQuery : IRequest<List<UserRoleModel>>
    {
        public string UserID { get; set; }
    }
    public class GetUserRolesByUserIDQueryHandler : IRequestHandler<GetUserRolesByUserIDQuery, List<UserRoleModel>>
    {

        private readonly IUserService _userService;

        public GetUserRolesByUserIDQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async  Task<List<UserRoleModel>> Handle(GetUserRolesByUserIDQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetRolesAsync(request.UserID);
        }
    }
}
