using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class ExceldataRepository : EFBaseRepository<Exceldata>
    {
        public List<Exceldata> Select(string teamName)
        {
            var ExceldataList = from e in Context.Exceldatas
                                where e.a == teamName
                                select e;

            return ExceldataList.ToList();
        }
    }
}
