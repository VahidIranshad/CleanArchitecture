using CA.Application.Contracts.Ent;
using CA.Application.Contracts.Identity;
using CA.Application.DTOs.Ent;
using CA.Application.Exceptions;
using CA.Domain.Base;
using CA.Domain.Ent;
using CA.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            await this._dbContext.SaveChangesAsync(_currentUserService);
            return entity;
        }

        public async Task Delete(int id)
        {
            var data = await _dbContext.Set<Selection>().FindAsync(id);
            if (data != null)
            {
                _dbContext.Set<Selection>().Remove(data);
                await this._dbContext.SaveChangesAsync(_currentUserService);
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

        public async Task<IReadOnlyList<Selection>> Get(Expression<Func<Selection, bool>> predicate = null, Func<IQueryable<Selection>, IOrderedQueryable<Selection>> orderBy = null, List<Expression<Func<Selection, object>>> includes = null, bool? disableTracking = true, Paging paging = null)
        {
            IQueryable<Selection> query = _dbContext.Set<Selection>();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            if (includes != null)
            {
                query = query.Where(predicate);
            }
            if (paging != null)
            {
                if (orderBy != null)
                {
                    return await orderBy(query).Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                }
                else
                {
                    return await query.Skip((paging.PageNumber - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();
                }
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
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
