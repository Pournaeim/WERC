using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class YearClassificationRepository : EFBaseRepository<YearClassification>
    {
        public IEnumerable<YearClassification> Select(int index, int count)
        {
            var yearClassificationList = from YearClassification in Context.YearClassifications
                               select YearClassification;

            return yearClassificationList.OrderBy(A => A.OrderNo).Skip(index).Take(count).ToArray();
        }
      
    }
}
