
using CA.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(name: "Roles", "Idn");
            builder.HasKey(p => p.Id).HasName("PK_Idn_Role");

            builder.HasData(
         new Role
         {
             Id = Domain.Constants.Identity.RoleConstants.DefaultRoleID,
             Name = "Default",
             NormalizedName = "DEFAULT",
             CreatedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
             LastModifiedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
             CreatedOn = new DateTime(2015, 01, 01),
             LastModifiedOn = new DateTime(2015, 01, 01)
         },
         new Role
         {
             Id = Domain.Constants.Identity.RoleConstants.AdministratorRoleID,
             Name = "Administrator",
             NormalizedName = "ADMINISTRATOR",
             CreatedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
             LastModifiedBy = "8e445865-a24d-4543-a6c6-9443d048cdb9",
             CreatedOn = new DateTime(2015, 01, 01),
             LastModifiedOn = new DateTime(2015, 01, 01)

         }
     );
        }
    }
}
