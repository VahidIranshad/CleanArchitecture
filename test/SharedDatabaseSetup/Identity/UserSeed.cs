using CA.Identity.DbContexts;
using CA.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace SharedDatabaseSetup.Identity
{
    internal class UserSeed
    {
        public async static Task SeedData(IdentityDbContext context)
        {
            context.Users.RemoveRange(context.Users.Where(p => p.Id != CA.Domain.Constants.Identity.UserConstants.AdministratorUserID).ToList());
            var hasher = new PasswordHasher<User>();
            var list = new List<User> {
                 new User
                 {
                     Id = "9e224968-33e4-4652-b7b7-8574d048cdb9",
                     Email = "user@localhost.com",
                     NormalizedEmail = "USER@LOCALHOST.COM",
                     FirstName = "System",
                     LastName = "User",
                     UserName = "user@localhost.com",
                     NormalizedUserName = "USER@LOCALHOST.COM",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true,
                     IsActive = true,
                     SecurityStamp = "412dfe9f-7794-4ba2-8672-74777bb1645d"
                 },  new User
                 {
                     Id = "9e224968-33e4-4652-b7b7-8574d048cdc9",
                     Email = "user2@localhost.com",
                     NormalizedEmail = "USER2@LOCALHOST.COM",
                     FirstName = "System",
                     LastName = "User2",
                     UserName = "user2@localhost.com",
                     NormalizedUserName = "USER2@LOCALHOST.COM",
                     PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                     EmailConfirmed = true,
                     IsActive = true,
                     SecurityStamp = "412dfe9f-7794-4ba2-8672-74777bb1645d"
                 },
            };
            context.AddRange(list);
            await context.SaveChangesAsync();
        }
    }
}
