using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.User.Queries
{
    public class GetUserByIDQuery : IRequest<UserResponse>
    {
        public string UserID { get; set; }
    }
    public class GetUserByIDQueryHandler : IRequestHandler<GetUserByIDQuery, UserResponse>
    {

        private readonly IUserService _userService;

        public GetUserByIDQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserResponse> Handle(GetUserByIDQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetAsync(request.UserID);
        }
    }
}
