using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class PersonMealTypeRepository : EFBaseRepository<PersonMealType>
    {
        public void CreatePersonsUser(PersonMealType personMealType)
        {
            Add(personMealType);
        }
        public IEnumerable<PersonMealType> GetPersonMealTypes(int personId)
        {

            return (from s in Context.PersonMealTypes where s.PersonId == personId select s).ToArray();
        }
        public int[] GetPersonMealTypeIds(int personId)
        {
            var mealTypeIds = (from s in Context.PersonMealTypes
                               where s.PersonId == personId
                               orderby s.MealTypeId
                               select s.MealTypeId).ToArray<int>();

            if (mealTypeIds == null)
            {
                mealTypeIds = new int[0];
            }
            return mealTypeIds;
        }

        public void DeletePersonMealTypes(int personId)
        {
            var deletablePersonMealTypes = from t in Context.PersonMealTypes
                                           where t.PersonId == personId
                                           select t;

            foreach (var item in deletablePersonMealTypes)
            {
                Delete(item);
            }
        }

        public void CreatePersonMealTypes(int personId, int[] mealTypeIds)
        {
            foreach (var mealTypeId in mealTypeIds)
            {
                Add(new PersonMealType { PersonId = personId, MealTypeId = mealTypeId });
            }
        }
    }
}
