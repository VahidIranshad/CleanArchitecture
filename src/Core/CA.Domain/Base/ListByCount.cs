using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.Domain.Base
{
    public class ListByCount<T> where T : class
    {
        public IEnumerable<T> DataList { get; set; }
        public int TotalCount { get; set; }
    }
}
