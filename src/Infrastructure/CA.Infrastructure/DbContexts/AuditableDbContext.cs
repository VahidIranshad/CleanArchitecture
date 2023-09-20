using Microsoft.EntityFrameworkCore;

namespace CA.Infrastructure.DbContexts
{
    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
