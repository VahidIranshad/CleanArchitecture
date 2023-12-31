﻿using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Identity;
using CA.Application.Exceptions;
using CA.Domain.Ent;
using CA.Infrastructure.DbContexts;
using Fop;
using Fop.FopExpression;
using Microsoft.EntityFrameworkCore;

namespace CA.Infrastructure.Repositories.Ent
{
    internal class SelectionRepository : ISelectionRepository
    {
        private readonly ICurrentUserService _currentUserService;
        protected readonly CustomDbContext _dbContext;
        public SelectionRepository(CustomDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;

            _currentUserService = currentUserService;
        }
        public async Task<Selection> Add(Selection entity)
        {
            await _dbContext.AddAsync(entity);
            //await this._dbContext.SaveChangesAsync(_currentUserService);
            return entity;
        }

        public async Task Delete(int id)
        {
            var data = await _dbContext.Set<Selection>().FindAsync(id);
            if (data != null)
            {
                _dbContext.Set<Selection>().Remove(data);
                //await this._dbContext.SaveChangesAsync(_currentUserService);
                return;
            }
            throw new NotFoundException(nameof(Selection), id);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await Get(id);
            return entity != null;
        }

        public async Task<Selection> Get(int id)
        {
            var data = await _dbContext.Set<Selection>().FindAsync(id);
            if (data != null)
            {
                return data;
            }
            throw new NotFoundException(nameof(Selection), id);
        }

        public async Task<(IReadOnlyList<Selection>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true)
        {
            
            var fopRequest = FopExpressionBuilder<Selection>.Build(Filter, Order, PageNumber ?? 0, PageSize ?? 0);
            IQueryable<Selection> query = _dbContext.Set<Selection>();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            var (result, count) = query.ApplyFop(fopRequest);
            return (await result.ToListAsync(), count);
        }

        public async Task Update(Selection entity)
        {
            var data = await _dbContext.Set<Selection>().AsNoTracking().Where(p => p.Id == entity.Id).FirstOrDefaultAsync();
            if (data != null)
            {
                entity.CreateDate = data.CreateDate;
                entity.CreatorID = data.CreatorID;
                _dbContext.Entry(entity).State = EntityState.Modified;

            }
            else
            {
                throw new NotFoundException(nameof(Selection), entity.Id);
            }
        }
    }
}
