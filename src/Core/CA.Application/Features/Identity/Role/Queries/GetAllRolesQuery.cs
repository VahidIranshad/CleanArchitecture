using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;

namespace CA.Application.Features.Identity.Role.Queries
{
    public class GetAllRolesQuery : IRequest<List<RoleResponse>>
    {
    }
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleResponse>>
    {

        private readonly IRoleService _roleService;

        public GetAllRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public async Task<List<RoleResponse>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetAllAsync();
        }
    }
}
