using CA.Application.DTOs.Ent;
using CA.Domain.Base;
using CA.Domain.Ent;
using System.Linq.Expressions;

namespace CA.Application.Contracts.Ent
{
    public interface ISelectionRepository
    {
        Task<Selection> Get(int id);
        Task<IReadOnlyList<SelectionDto>> Get(Expression<Func<Selection, bool>> predicate = null,
                                       Func<IQueryable<Selection>, IOrderedQueryable<Selection>> orderBy = null,
                                       List<Expression<Func<Selection, object>>> includes = null,
                                       bool? disableTracking = true,
                                         Paging paging = null);
        Task<Selection> Add(SelectionCreateDto entity);
        Task<bool> Exists(int id);
        Task Update(SelectionUpdateDto entity);
        Task Delete(int id);
    }
}
