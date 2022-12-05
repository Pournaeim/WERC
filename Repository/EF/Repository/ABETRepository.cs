using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class ABETRepository : EFBaseRepository<ABET>
    {
        public IEnumerable<ABET> Select(int index, int count)
        {
            var ABETList = from ABET in Context.ABETs
                            select ABET;

            return ABETList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ABET> GetABETs(string abetName = "")
        {

            var abetList = from abet in Context.ABETs
                            select abet;

            if (abetName != "")
            {
                abetList = abetList.Where(t => t.Name.Contains(abetName));
            }

            return abetList.ToArray();
        }

        public void CreateABET(ABET newABET)
        {
            Add(newABET);
        }
        public void UpdateABET(ABET updateableABET)
        {
            var oldABET = (from s in Context.ABETs where s.Id == updateableABET.Id select s).FirstOrDefault();

            oldABET.Name = updateableABET.Name;
            Update(oldABET);
        }
        public bool DeleteABET(int abetId)
        {
            var oldABET = Context.ABETs.Find(abetId);          
            Delete(oldABET);

            return true;

        }

        public ABET GetABETById(int id)
        {
            return Context.ABETs.Find(id);

        }
    }
}
