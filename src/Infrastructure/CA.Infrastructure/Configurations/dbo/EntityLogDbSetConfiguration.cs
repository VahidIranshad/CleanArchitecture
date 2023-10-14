using CA.Domain.Ent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CA.Infrastructure.Configurations.dbo
{
    internal class EntityLogDbSetConfiguration : IEntityTypeConfiguration<EntityLog>
    {
        public void Configure(EntityTypeBuilder<EntityLog> builder)
        {
            builder.ToTable("EntityLogDbSet");
            builder.HasKey(p => p.Id).HasName("PK_EntityLogDbSet");
        }
    }
}
