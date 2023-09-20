using CA.Application.Contracts.Generic;
using CA.Application.Contracts.Identity;
using CA.Application.Exceptions;
using CA.Domain.Base;
using CA.Infrastructure.DbContexts;
using Fop;
using Fop.FopExpression;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CA.Infrastructure.Repositories.Generic
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly CustomDbContext _dbContext;
        public GenericRepository(CustomDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool? disableTracking = true,
            Paging paging = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            if (!string.IsNullOrWhiteSpace(includeString))
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

        public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool? disableTracking = true,
            Paging paging = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
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

        public async Task<T> Get(int id)
        {
            var data = await _dbContext.Set<T>().FindAsync(id);
            if (data != null)
            {
                return data;
            }
            throw new NotFoundException(nameof(T), id);
        }

        public async Task<(IQueryable<T>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true)
        {

            var fopRequest = FopExpressionBuilder<T>.Build(Filter, Order, PageNumber ?? 0, PageSize ?? 0);
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking.HasValue && disableTracking.Value)
            {
                query = query.AsNoTracking();
            }
            return query.ApplyFop(fopRequest);
        }

        public async Task<List<T>> Get(List<int> ids)
        {
            var data = await _dbContext.Set<T>().Where(p => ids.Contains(p.Id)).ToListAsync();
            if (data != null)
            {
                return data;
            }
            throw new NotFoundException(nameof(T), ids);
        }



        public async Task<T> Add(T entity)
        {
            await _dbContext.AddAsync(entity);
            return entity;
        }
        public async Task<List<T>> AddRange(List<T> list)
        {
            await _dbContext.AddRangeAsync(list);
            return list;
        }

        public async Task Delete(T entity)
        {
            var data = await _dbContext.Set<T>().FindAsync(entity.Id);
            if (data != null)
            {
                _dbContext.Set<T>().Remove(entity);
                return;
            }
            throw new NotFoundException(nameof(T), entity.Id);
        }

        public async Task Delete(int id)
        {
            var data = await _dbContext.Set<T>().FindAsync(id);
            if (data != null)
            {
                _dbContext.Set<T>().Remove(data);
                return;
            }
            throw new NotFoundException(nameof(T), id);
        }

        public async Task Delete(List<int> ids)
        {
            var datas = await _dbContext.Set<T>().Where(p => ids.Contains(p.Id)).ToListAsync();
            if (datas != null && datas.Any())
            {
                _dbContext.Set<T>().RemoveRange(datas);
                return;
            }
            //throw new NotFoundException(nameof(T), ids);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await Get(id);
            return entity != null;
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            var data = await _dbContext.Set<T>().AsNoTracking().Where(p => p.Id == entity.Id).FirstOrDefaultAsync();
            //var data = await _dbContext.Set<T>().FindAsync(entity.Id);
            if (data != null)
            {
                entity.CreateDate = data.CreateDate;
                entity.CreatorID = data.CreatorID;
                _dbContext.Entry(entity).State = EntityState.Modified;

            }
            else
            {
                throw new NotFoundException(nameof(T), entity.Id);
            }
        }

        public T AddEntity(T entity)
        {
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        public List<T> AddRangeEntity(List<T> list)
        {
            _dbContext.AddRange(list);
            _dbContext.SaveChanges();
            return list;
        }
        public async Task<List<T>> AddRangeEntityAsync(List<T> list)
        {
            await _dbContext.AddRangeAsync(list);
            return list;
        }
        public async Task SaveChangesAsync(ICurrentUserService currentUser)
        {
            await _dbContext.SaveChangesAsync(currentUser);
        }


    }
}
