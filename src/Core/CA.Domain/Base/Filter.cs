using System.Linq.Expressions;

namespace CA.Domain.Base
{
    public class Filter<T>
        where T : BaseEntity
    {
        public Expression<Func<T, bool>> predicate = null;
        public Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null;
        public List<Expression<Func<T, object>>> includes = null;
        public bool? disableTracking = true;
        public Paging paging = null;
    }
    public class FopFilter
    {
        public FopFilter()
        {

        }
        public FopFilter(string _filter, string _order, int _pageNumber, int _pageSize, bool? _disableTracking = true)
        {
            Filter = _filter;
            Order = _order;
            PageNumber = _pageNumber;
            PageSize = _pageSize;
            DisableTracking = _disableTracking;
        }
        public string Filter { get; set; }
        public string Order { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool? DisableTracking { get; set; }


    }
}
