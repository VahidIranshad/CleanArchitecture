using CA.Application.Contracts.Identity;
using CA.Domain.Base;
using CA.Domain.Ent;
using CA.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace CA.Infrastructure.DbContexts
{
    public class CustomDbContext : AuditableDbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomDbContext).Assembly);
        }

        public virtual async Task<int> SaveChangesAsync(ICurrentUserService user)
        {
            try
            {
                var createList = new List<BaseEntity>();
                var logs = new List<EntityLog>();

                foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                    .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified || q.State == EntityState.Deleted))
                {
                    var entityLog = new EntityLog
                    {
                        LastEditDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreatorID = user.UserId,
                        LastEditorID = user.UserId,
                        ClassName = entry.Entity.GetType().Name,
                        Key = entry.Entity.Id.ToString()

                    };
                    if (entry.State == EntityState.Deleted)
                    {
                        entityLog.ChangeType = "D";
                        logs.Add(entityLog);
                    }
                    else
                    {
                        entry.Entity.LastEditDate = DateTime.Now;
                        entry.Entity.LastEditorID = user.UserId;

                        if (entry.State == EntityState.Added)
                        {
                            entry.Entity.CreateDate = DateTime.Now;
                            entry.Entity.CreatorID = user.UserId;

                            createList.Add(entry.Entity);
                        }
                        else
                        {
                            entityLog.ChangeType = "U";
                            logs.Add(entityLog);
                        }
                    }
                }

                var result = await base.SaveChangesAsync();

                foreach (var item in createList)
                {
                    var entityLog = new EntityLog
                    {
                        LastEditDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreatorID = user == null ? "1" : user.UserId,
                        LastEditorID = user == null ? "1" : user.UserId,
                        ClassName = item.GetType().Name,
                        Key = item.Id.ToString()

                    };
                    entityLog.ChangeType = "I";
                    logs.Add(entityLog);
                }

                var _unitOfWork = new UnitOfWork<EntityLog>(this, null);
                _unitOfWork.Repository().AddRangeEntity(logs);


                return result;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
        public DbSet<EntityLog> EntityLogDbSet { get; set; }
        public DbSet<Selection> SelectionDbSet { get; set; }
        public DbSet<TValue> TValueDbSet { get; set; }
    }
}
