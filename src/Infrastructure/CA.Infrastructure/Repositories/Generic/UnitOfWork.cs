using CA.Application.Contracts.Generic;
using CA.Application.Contracts.Identity;
using CA.Domain.Base;
using CA.Infrastructure.DbContexts;
using System.Collections;

namespace CA.Infrastructure.Repositories.Generic
{
    internal class UnitOfWork<T> : IUnitOfWork<T>
        where T : BaseEntity
    {

        private readonly CustomDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private Hashtable _repositories;


        internal UnitOfWork(CustomDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            this._currentUserService = currentUserService;
        }
        public IGenericRepository<T> Repository()
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }



        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            foreach (var cacheKey in cacheKeys)
            {
                //_cache.Remove(cacheKey);
            }
            return result;
        }

        public Task Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            //var username = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;

            return await _context.SaveChangesAsync(_currentUserService);
            //return x;
        }

    }
}
