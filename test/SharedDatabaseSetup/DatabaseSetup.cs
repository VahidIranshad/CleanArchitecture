using CA.Identity.DbContexts;
using CA.Infrastructure.DbContexts;

namespace SharedDatabaseSetup
{

    public static class DatabaseSetup
    {
        private static object locker = new object();
        private static object customLocker;
        private static object identityLocker;
        public async static void SeedDataCustomDbContext(CustomDbContext context)
        {
            if (customLocker != null)
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            lock (locker)
            {
                if (customLocker != null)
                {
                    return;
                }


                tasks.Add(Ent.SelectionSeedData.SeedData(context));
                Task.WhenAll(tasks);
                customLocker = new object();
            }

        }

        public static void SeedDataIdentityDbContext(IdentityDbContext context)
        {
            if (identityLocker != null)
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            lock (locker)
            {
                if (identityLocker != null)
                {
                    return;
                }
                tasks.Add(Identity.UserSeed.SeedData(context));
                tasks.Add(Identity.UserRoleSeed.SeedData(context));


                Task.WhenAll(tasks);
                identityLocker = new object();
            }
        }
    }
}
