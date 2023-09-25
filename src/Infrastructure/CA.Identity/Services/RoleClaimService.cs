using AutoMapper;
using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;
using CA.Identity.DbContexts;
using CA.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CA.Identity.Services
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IdentityDbContext _db;
        private readonly UserManager<User> _userManager;
        //private readonly manager;

        public RoleClaimService(
            UserManager<User> userManager,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IdentityDbContext db)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<RoleClaimResponse>> GetAllAsync()
        {
            var roleClaims = await _db.RoleClaims.ToListAsync();
            var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
            return roleClaimsResponse;
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _db.RoleClaims.CountAsync();
            return count;
        }

        public async Task<RoleClaimResponse> GetByIdAsync(int id)
        {
            var roleClaim = await _db.RoleClaims
                .SingleOrDefaultAsync(x => x.Id == id);
            var roleClaimResponse = _mapper.Map<RoleClaimResponse>(roleClaim);
            return roleClaimResponse;
        }

        public async Task<List<RoleClaimResponse>> GetAllByRoleIdAsync(string roleId)
        {
            var roleClaims = await _db.RoleClaims
                .Include(x => x.Role)
                .Where(x => x.RoleId == roleId)
                .ToListAsync();
            var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
            return roleClaimsResponse;
        }

        public async Task<string> SaveAsync(RoleClaimRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleId))
            {
                throw new Exception("Role is required.");
            }

            if (request.Id == 0)
            {
                var existingRoleClaim =
                    await _db.RoleClaims
                        .SingleOrDefaultAsync(x =>
                            x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
                if (existingRoleClaim != null)
                {
                    throw new Exception("Similar Role Claim already exists.");
                }
                var roleClaim = _mapper.Map<RoleClaim>(request);

                roleClaim.LastModifiedOn = DateTime.Now;
                roleClaim.CreatedOn = DateTime.Now;
                roleClaim.CreatedBy = _currentUserService.UserId;
                roleClaim.LastModifiedBy = _currentUserService.UserId;
                await _db.RoleClaims.AddAsync(roleClaim);
                await _db.SaveChangesAsync(_currentUserService.UserId);
                return string.Format("Role Claim {0} created.", request.Value);
            }
            else
            {
                var existingRoleClaim =
                    await _db.RoleClaims
                        .Include(x => x.Role)
                        .SingleOrDefaultAsync(x => x.Id == request.Id);
                if (existingRoleClaim == null)
                {
                    throw new Exception("Role Claim does not exist.");
                }
                else
                {
                    existingRoleClaim.ClaimType = request.Type;
                    existingRoleClaim.ClaimValue = request.Value;
                    existingRoleClaim.Group = request.Group;
                    existingRoleClaim.Description = request.Description;
                    existingRoleClaim.RoleId = request.RoleId;
                    _db.RoleClaims.Update(existingRoleClaim);
                    await _db.SaveChangesAsync(_currentUserService.UserId);
                    return string.Format("Role Claim {0} for Role {1} updated.", request.Value, existingRoleClaim.Role.Name);
                }
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            var existingRoleClaim = await _db.RoleClaims
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingRoleClaim != null)
            {
                _db.RoleClaims.Remove(existingRoleClaim);
                await _db.SaveChangesAsync(_currentUserService.UserId);
                return string.Format("Role Claim {0} for {1} Role deleted.", existingRoleClaim.ClaimValue, existingRoleClaim.Role.Name);
            }
            else
            {
                throw new Exception("Role Claim does not exist.");
            }
        }

        public async Task<bool> HasPermission(string userID, List<string> permissions)
        {
            var roleIdList = await _db.UserRoles.Where(p => p.UserId == userID).Select(p => p.RoleId).ToListAsync();
            if (roleIdList == null || roleIdList.Count == 0)
            {
                return false;
            }
            else
            {
                return await HasPermission(roleIdList.ToList(), permissions);
            }
        }

        public async Task<bool> HasPermission(List<string> roleIds, List<string> permissions)
        {
            if (roleIds.Contains(Domain.Constants.Identity.RoleConstants.AdministratorRoleID))
            {
                return true;
            }
            return await _db.RoleClaims.AnyAsync(p => roleIds.Contains(p.RoleId) && permissions.Contains(p.ClaimValue));
        }
    }
}