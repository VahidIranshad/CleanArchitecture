using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Identity;
using CA.Domain.Ent;
using CA.Infrastructure.DbContexts;
using CA.Infrastructure.Repositories.Generic;

namespace CA.Infrastructure.Repositories.Ent
{
    internal class TValueRepository : GenericRepository<TValue>, ITValueRepository
    {

        private readonly ICurrentUserService _currentUserService;
        public TValueRepository(CustomDbContext dbContext, ICurrentUserService currentUserService) :
            base(dbContext)
        {
            _currentUserService = currentUserService;
        }
    }
}
