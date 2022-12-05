using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class SizeRepository : EFBaseRepository<Size>
    {
        public IEnumerable<Size> Select(int index, int count)
        {
            var sizeList = from Size in Context.Sizes
                               select Size;

            return sizeList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
      
    }
}
