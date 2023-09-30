using CA.Identity.DbContexts;
using CA.Infrastructure.DbContexts;

namespace SharedDatabaseSetup
{

    public static class DatabaseSetup
    {
        private static object locker = new object();
        private static object test;
        public async static void SeedData(CustomDbContext customDbContextContext, IdentityDbContext identityDbContextContext)
        {
            if (test != null)
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            lock (locker)
            {
                if (test != null)
                {
                    return;
                }
                tasks.Add(Identity.UserSeed.SeedData(identityDbContextContext));
                tasks.Add(Identity.UserRoleSeed.SeedData(identityDbContextContext));


                Task.WhenAll(tasks);
                test = new object();
            }

        }
    }
}
