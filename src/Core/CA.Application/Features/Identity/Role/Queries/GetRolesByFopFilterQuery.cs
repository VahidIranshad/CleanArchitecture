using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Responses;
using CA.Domain.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Application.Features.Identity.Role.Queries
{
    public class GetRolesByFopFilterQuery : IRequest<(List<RoleResponse>, int)>
    {
        public FopFilter filter { get; set; }
    }

    public class GetRolesByFopFilterQueryHandler : IRequestHandler<GetRolesByFopFilterQuery, (List<RoleResponse>, int)>
    {


        private readonly IRoleService _roleService;

        public GetRolesByFopFilterQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<(List<RoleResponse>, int)> Handle(GetRolesByFopFilterQuery request, CancellationToken cancellationToken)
        {

            var (datas, totalCount) = await _roleService.Get(request?.filter?.Filter, request?.filter?.Order, request?.filter?.PageNumber ?? 0, request?.filter?.PageSize ?? 0, request?.filter.DisableTracking);
            return (datas, totalCount);
        }
    }
}
