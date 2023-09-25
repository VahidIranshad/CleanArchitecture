using CA.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(name: "Users", "Idn");
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasKey(p => p.Id).HasName("PK_Idn_User");
            builder.Property(p => p.ProfilePictureDataUrl).HasColumnType("nvarchar");
            var hasher = new PasswordHasher<User>();
            builder.HasData(
                 new User
                 {
                     Id = Domain.Constants.Identity.UserConstants.AdministratorUserID,
                     Email = "admin@localhost.com",
                     NormalizedEmail = "ADMIN@LOCALHOST.COM",
                     FirstName = "System",
                     LastName = "Admin",
                     UserName = "admin@localhost.com",
                     NormalizedUserName = "ADMIN@LOCALHOST.COM",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true,
                     IsActive = true,
                     SecurityStamp = "412dfe9f-7794-4ba2-8672-74777bb1645d"
                 }
            );
        }
    }
}
