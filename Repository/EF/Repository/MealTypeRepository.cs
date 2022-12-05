using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class MealTypeRepository : EFBaseRepository<MealType>
    {

        public void CreateMealType(MealType newMealType)
        {
            Add(newMealType);
        }
        public void UpdateMealType(MealType updateableMealType)
        {
            var oldMealType = (from s in Context.MealTypes where s.Id == updateableMealType.Id select s).FirstOrDefault();

            oldMealType.Name = updateableMealType.Name;

            Update(oldMealType);
        }
        public void DeleteMealType(int MealTypeId)
        {
            var oldMealType = (from s in Context.MealTypes where s.Id == MealTypeId select s).FirstOrDefault();
            Delete(oldMealType);
        }



        public IEnumerable<MealType> MealTypeList { get; set; }
        public int Count(Func<MealType, bool> predicate)
        {
            return MealTypeList.Count();
        }
        public IEnumerable<MealType> Select(int index = 0, int count = int.MaxValue)
        {
            var mealTypeList = from mealType in Context.MealTypes
                                  select mealType;

            return mealTypeList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<MealType> Select(Func<MealType, bool> predicate, int index, int count)
        {
            var mealTypeList = (from mealType in Context.MealTypes
                                   select mealType).Where(predicate);

            return mealTypeList.Skip(index).Take(count).ToArray();
        }
        public MealType GetMealTypeById(int id)
        {
            var mealType = Context.MealTypes.SingleOrDefault(a => a.Id == id);

            return mealType;
        }
        public IEnumerable<MealType> Select(MealType filterItem, int index, int count)
        {
            var mealTypeList = from mealType in Context.MealTypes
                                  select mealType;

           
            if (string.IsNullOrWhiteSpace(filterItem.Name) == false)
            {
                mealTypeList = mealTypeList.Where(t => t.Name.Contains(filterItem.Name));
            }

            return mealTypeList.OrderBy(t => t.Name).Skip(index).Take(count).ToArray();

        }
    }
}
