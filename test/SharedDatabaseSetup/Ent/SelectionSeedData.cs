using CA.Domain.Ent;
using CA.Infrastructure.DbContexts;
using SharedDatabaseSetup.Identity;

namespace SharedDatabaseSetup.Ent
{
    internal class SelectionSeedData
    {
        public async static Task SeedData(CustomDbContext context)
        {
            var data = context.SelectionDbSet.FirstOrDefault(p => p.Id == 0);
            if (data == null)
            {
                var list = new List<Selection> {
                new Selection{Id =0 , SelectionType = "A", Title = "A"}
            };
                context.AddRange(list);
                await context.SaveChangesAsync(DefaultUser.adminCurrentUserService.Object);
            }


        }
    }
}
