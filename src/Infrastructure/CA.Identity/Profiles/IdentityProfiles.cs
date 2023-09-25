using AutoMapper;
using CA.Identity.Models;
using CA.Application.DTOs.Identity.Requests;
using CA.Application.DTOs.Identity.Responses;

namespace CA.Identity.Profiles
{
    public class IdentityProfiles : Profile
    {
        public IdentityProfiles()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
            //CreateMap<AuditResponse, Audit>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaim>()
                .ForMember(nameof(RoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(RoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();

            CreateMap<RoleClaimRequest, RoleClaim>()
                .ForMember(nameof(RoleClaim.ClaimType), opt => opt.MapFrom(c => c.Type))
                .ForMember(nameof(RoleClaim.ClaimValue), opt => opt.MapFrom(c => c.Value))
                .ReverseMap();
            CreateMap<RoleResponse, Role>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();

        }
    }
}
