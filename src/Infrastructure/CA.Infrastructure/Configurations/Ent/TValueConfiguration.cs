using CA.Domain.Constants.Identity;
using CA.Domain.Ent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Infrastructure.Configurations.Ent
{
    internal class TValueConfiguration : IEntityTypeConfiguration<TValue>
    {
        public void Configure(EntityTypeBuilder<TValue> builder)
        {
            builder.ToTable("TValue", "Ent");
            builder.HasKey(p => p.Id).HasName("PK_Ent_TValue");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Title).HasColumnType("nvarchar").HasMaxLength(200);
            builder.HasData(
                new TValue
                {
                    Id = 0,
                    Title = "Test",
                    CreatorID = UserConstants.AdministratorUserID,
                    LastEditorID = UserConstants.AdministratorUserID,
                    CreateDate = new DateTime(2015, 01, 01),
                    LastEditDate = new DateTime(2015, 01, 01)
                }
           );
        }
    }
}
