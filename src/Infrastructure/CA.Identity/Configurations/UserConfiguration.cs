using CA.Identity.Models;
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
        }
    }
}
