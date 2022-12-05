using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class DietTypeRepository : EFBaseRepository<DietType>
    {
        public IEnumerable<DietType> Select(int index, int count, bool display)
        {
            var DietTypeList = from dietType in Context.DietTypes
                               where dietType.Display == display
                       
                               select dietType;

            return DietTypeList.OrderBy(A => A.OrderNo).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<DietType> GetDietTypes(string dietTypeName = "")
        {

            var dietTypeList = from dietType in Context.DietTypes
                               select dietType;

            if (dietTypeName != "")
            {
                dietTypeList = dietTypeList.Where(t => t.Name.Contains(dietTypeName));
            }

            return dietTypeList.OrderBy(d=>d.OrderNo).ToArray();
        }

        public void CreateDietType(DietType newDietType)
        {
            Add(newDietType);
        }
        public void UpdateDietType(DietType updateableDietType)
        {
            var oldDietType = (from s in Context.DietTypes where s.Id == updateableDietType.Id select s).FirstOrDefault();

            oldDietType.Name = updateableDietType.Name;
            oldDietType.Display = updateableDietType.Display;

            Update(oldDietType);
        }
        public bool DeleteDietType(int DietTypeId)
        {
            var oldDietType = (from s in Context.DietTypes where s.Id == DietTypeId select s).FirstOrDefault();

            Delete(oldDietType);

            return true;
        }

        public DietType GetDietTypeById(int id)
        {
            return Context.DietTypes.Find(id);

        }
        public int GetDietTypeNewId()
        {
            return Context.DietTypes.Max(d => d.Id) + 1;

        }
    }
}
