using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using MediatR;


namespace CA.Application.Features.Identity.Role.Queries
{
    public class GetByIdQuery : IRequest<RoleResponse>
    {
        public string id { get; set; }
    }
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, RoleResponse>
    {

        private readonly IRoleService _roleService;

        public GetByIdQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public async Task<RoleResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetByIdAsync(request.id);
        }
    }
}
