using AutoMapper;
using CA.Application.Contracts.Identity;
using CA.Infrastructure.Repositories.Generic;

namespace IntegrationTests.Ent
{
    public class SelectionRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private SharedDatabaseFixture Fixture { get; }
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUser;
        public SelectionRepositoryTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;
            _mapper = SharedDatabaseFixture.mapper;
            _currentUser = SharedDatabaseSetup.Identity.DefaultUser.adminCurrentUserService.Object;

        }
        [Fact]
        public async Task GetData_ReturnsAllBook()
        {
            using (var context = Fixture.CreateSDAContext())
            {
                var _unitOfWork = new UnitOfWork<CA.Domain.Ent.Selection>(context, _currentUser);
                var repository = _unitOfWork.Repository();

                var list = await repository.GetAll();

                Assert.Equal(1, list.Count);
            }
        }
    }
}
