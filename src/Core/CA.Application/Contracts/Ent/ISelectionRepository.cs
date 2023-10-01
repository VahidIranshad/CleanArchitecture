using CA.Application.DTOs.Ent;
using CA.Domain.Base;
using CA.Domain.Ent;
using System.Linq.Expressions;

namespace CA.Application.Contracts.Ent
{
    public interface ISelectionRepository
    {
        Task<Selection> Get(int id);
        Task<(IReadOnlyList<Selection>, int)> Get(string Filter, string Order, int? PageNumber, int? PageSize, bool? disableTracking = true);
        Task<Selection> Add(Selection entity);
        Task<bool> Exists(int id);
        Task Update(Selection entity);
        Task Delete(int id);
    }
}
