using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class EthnicityRepository : EFBaseRepository<Ethnicity>
    {
        public IEnumerable<Ethnicity> Select(int index, int count)
        {
            var ethnicityList = from Ethnicity in Context.Ethnicities
                               select Ethnicity;

            return ethnicityList.OrderBy(A => A.OrderNo).Skip(index).Take(count).ToArray();
        }
      
    }
}
