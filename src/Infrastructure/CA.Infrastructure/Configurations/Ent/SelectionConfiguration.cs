using CA.Domain.Ent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Infrastructure.Configurations.Ent
{
    internal class SelectionConfiguration : IEntityTypeConfiguration<Selection>
    {
        public void Configure(EntityTypeBuilder<Selection> builder)
        {
            builder.ToTable("Selection", "Ent");
            builder.HasKey(p => p.Id).HasName("PK_Ent_Selection");
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Title).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(p => p.SelectionType).HasColumnType("nvarchar").HasMaxLength(100);
            builder.HasIndex(p => new { p.SelectionType, p.Title }).IsUnique(true);

        }
    }
}
