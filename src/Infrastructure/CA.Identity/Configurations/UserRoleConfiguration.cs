using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("UserRoles", "Idn");
            builder.HasKey(p => new { p.UserId, p.RoleId }).HasName("PK_Idn_UserRole"); 
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = Domain.Constants.Identity.RoleConstants.AdministratorRoleID,
                    UserId = Domain.Constants.Identity.UserConstants.AdministratorUserID
                }
            );
        }
    }
}
