using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using MediatR;

namespace CA.Application.Features.Identity.User.Queries
{
    public class GetListByFopFilterQuery :IRequest<(List<UserResponse>, int)>
    {
        public FopFilter filter { get; set; }
    }

    public class GetListByFopFilterQueryHandler : IRequestHandler<GetListByFopFilterQuery, (List<UserResponse>, int)>
    {


        private readonly IUserService _userService;

        public GetListByFopFilterQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<(List<UserResponse>, int)> Handle(GetListByFopFilterQuery request, CancellationToken cancellationToken)
        {

            var (datas, totalCount) = await _userService.Get(request?.filter?.Filter, request?.filter?.Order, request?.filter?.PageNumber ?? 0, request?.filter?.PageSize ?? 0, request?.filter.DisableTracking);
            return (datas, totalCount);
        }
    }
}
