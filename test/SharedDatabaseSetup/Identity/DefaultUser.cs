using CA.Application.Contracts.Identity;
using Moq;

namespace SharedDatabaseSetup.Identity
{
    public static class DefaultUser
    {
        public static Mock<ICurrentUserService> adminCurrentUserService
        {
            get
            {
                var x = new Mock<ICurrentUserService>();
                x.Setup(r => r.UserId).Returns(CA.Domain.Constants.Identity.RoleConstants.AdministratorRoleID);
                return x;
            }
        }
        public static Mock<ICurrentUserService> normalCurrentUserService
        {
            get
            {
                var x = new Mock<ICurrentUserService>();
                x.Setup(r => r.UserId).Returns("9e224968-33e4-4652-b7b7-8574d048cdb9");
                return x;
            }
        }
    }
}
