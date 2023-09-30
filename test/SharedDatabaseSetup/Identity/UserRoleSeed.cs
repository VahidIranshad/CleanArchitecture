using CA.Identity.DbContexts;
using Microsoft.AspNetCore.Identity;

namespace SharedDatabaseSetup.Identity
{
    internal class UserRoleSeed
    {

        public async static Task SeedData(IdentityDbContext context)
        {
            context.UserRoles.RemoveRange(context.UserRoles.Where(p => p.UserId != CA.Domain.Constants.Identity.UserConstants.AdministratorUserID).ToList());
            
            var list = new List<IdentityUserRole<string>> {
                 new IdentityUserRole<string>
                 {
                    RoleId = CA.Domain.Constants.Identity.RoleConstants.DefaultRoleID,
                    UserId = "9e224968-33e4-4652-b7b7-8574d048cdb9"
                 },
            };
            context.AddRange(list);
            await context.SaveChangesAsync();
        }
    }
}
