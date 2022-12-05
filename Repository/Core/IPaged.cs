using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Core
{
    public interface IPaged<T> : IEnumerable<T>
    {
        int Count { get; }
        IEnumerable<T> GetRange(int index, int count);

    }

}
