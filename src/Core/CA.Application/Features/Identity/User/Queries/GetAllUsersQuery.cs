using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.User.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserResponse>>
    {
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {

        private readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAllAsync();
        }
    }
}
