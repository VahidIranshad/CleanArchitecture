using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CA.Application.Contracts.Identity;
using CA.Domain.Constants.Identity;
using CA.Domain.Constants.Permission;
using CA.Identity.Helpers;
using CA.Identity.Models;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using Fop.FopExpression;
using Fop;

namespace CA.Identity.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IRoleClaimService _roleClaimService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public RoleService(
            RoleManager<Role> roleManager,
            IMapper mapper,
            UserManager<User> userManager,
            IRoleClaimService roleClaimService,
            ICurrentUserService currentUserService)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _roleClaimService = roleClaimService;
            _currentUserService = currentUserService;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != RoleConstants.AdministratorRole && existingRole.Name != RoleConstants.DefaultRoleID)
            {
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return string.Format("Role {0} Deleted.", existingRole.Name);
                }
                else
                {
                    return string.Format("Not allowed to delete {0} Role as it is being used.", existingRole.Name);
                }
            }
            else
            {
                return string.Format("Not allowed to delete {0} Role.", existingRole.Name);
            }
        }

        public async Task<List<RoleResponse>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return rolesResponse;
        }

        public async Task<PermissionResponse> GetAllPermissionsAsync(string roleId)
        {
            var model = new PermissionResponse();
            var allPermissions = GetAllPermissions();
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.RoleName = role.Name;
                var roleClaimsResult = await _roleClaimService.GetAllByRoleIdAsync(role.Id);

                var roleClaims = roleClaimsResult;
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = roleClaims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                        var roleClaim = roleClaims.SingleOrDefault(a => a.Value == permission.Value);
                        if (roleClaim?.Description != null)
                        {
                            permission.Description = roleClaim.Description;
                        }
                        if (roleClaim?.Group != null)
                        {
                            permission.Group = roleClaim.Group;
                        }
                    }
                }

            }
            model.RoleClaims = allPermissions;
            return model;
        }

        private List<RoleClaimResponse> GetAllPermissions()
        {
            var allPermissions = new List<RoleClaimResponse>();

            #region GetPermissions

            allPermissions.GetAllPermissions();

            #endregion GetPermissions

            return allPermissions;
        }

        public async Task<RoleResponse> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return rolesResponse;
        }

        public async Task<string> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null)
                {
                    throw new Exception("Similar Role already exists.");
                }

                var response = await _roleManager.CreateAsync(new Role(request.Name, request.Description, _currentUserService.UserId));
                if (response.Succeeded)
                {
                    return string.Format("Role {0} Created.", request.Name);
                }
                else
                {
                    throw new Exception(String.Join(',', response.Errors.Select(e => e.Description.ToString()).ToList()));
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.DefaultRoleID)
                {
                    throw new Exception(string.Format("Not allowed to modify {0} Role.", existingRole.Name));
                }
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                existingRole.Description = request.Description;
                await _roleManager.UpdateAsync(existingRole);
                return string.Format("Role {0} Updated.", existingRole.Name);
            }
        }

        public async Task<string> UpdatePermissionsAsync(PermissionRequest request)
        {
            try
            {
                var errors = new List<string>();
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role.Name == RoleConstants.AdministratorRole)
                {
                    var currentUser = await _userManager.Users.SingleAsync(x => x.Id == _currentUserService.UserId);
                    if (await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
                    {
                        throw new Exception("Not allowed to modify Permissions for this Role.");
                    }
                }

                var selectedClaims = request.RoleClaims.Where(a => a.Selected == true).ToList();
                if (role.Name == RoleConstants.AdministratorRole)
                {
                    if (!selectedClaims.Any(x => x.Value == Permissions.RolesPermissions.View)
                       || !selectedClaims.Any(x => x.Value == Permissions.RoleClaimsPermissions.View)
                       || !selectedClaims.Any(x => x.Value == Permissions.RoleClaimsPermissions.Edit))
                    {
                        throw new Exception(string.Format(
                            "Not allowed to deselect {0} or {1} or {2} for this Role.",
                            Permissions.RolesPermissions.View, Permissions.RolesPermissions.View, Permissions.RolesPermissions.Edit));
                    }
                }

                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                foreach (var claim in selectedClaims)
                {
                    var addResult = await _roleManager.AddPermissionClaim(role, claim.Value);
                    if (!addResult.Succeeded)
                    {
                        errors.AddRange(addResult.Errors.Select(e => e.Description.ToString()));
                    }
                }

                var addedClaims = await _roleClaimService.GetAllByRoleIdAsync(role.Id);

                foreach (var claim in selectedClaims)
                {
                    var addedClaim = addedClaims.SingleOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);
                    if (addedClaim != null)
                    {
                        claim.Id = addedClaim.Id;
                        claim.RoleId = addedClaim.RoleId;
                        var saveResult = await _roleClaimService.SaveAsync(claim);
                    }
                }

                if (errors.Any())
                {
                    throw new Exception(string.Join(',', errors));
                }

                return "Permissions Updated.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _roleManager.Roles.CountAsync();
            return count;
        }
        public async Task<(List<RoleResponse>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true)
        {

            var fopRequest = FopExpressionBuilder<Role>.Build(Filter, Order, PageNumber ?? 0, PageSize ?? 0);
            IQueryable<Role> query = this._roleManager.Roles.AsQueryable();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            var (datas, count) = query.ApplyFop(fopRequest);
            var list = _mapper.Map<List<RoleResponse>>(await datas.ToListAsync());
            return (list, count);
        }
    }
}