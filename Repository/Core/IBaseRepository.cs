using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Core
{
    interface IBaseRepository<E>
    {
        object Context { get; set; }

        E Get(object id);
        IEnumerable<E> Get();
        IEnumerable<E> Get(Expression<Func<E, bool>> predicate);
        IEnumerable<E> Get(Func<E, bool> predicate);
        E Add(E entity);
        void Update(E entity);
        void Delete(E entity);
    }

}
