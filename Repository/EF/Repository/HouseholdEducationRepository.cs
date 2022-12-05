using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class HouseholdEducationRepository : EFBaseRepository<HouseholdEducation>
    {

        public void CreateHouseholdEducation(HouseholdEducation newHouseholdEducation)
        {
            Add(newHouseholdEducation);
        }
        public void UpdateHouseholdEducation(HouseholdEducation updateableHouseholdEducation)
        {
            var oldHouseholdEducation = (from s in Context.HouseholdEducations where s.Id == updateableHouseholdEducation.Id select s).FirstOrDefault();

            oldHouseholdEducation.Name = updateableHouseholdEducation.Name;

            Update(oldHouseholdEducation);
        }
        public void DeleteHouseholdEducation(int HouseholdEducationId)
        {
            var oldHouseholdEducation = (from s in Context.HouseholdEducations where s.Id == HouseholdEducationId select s).FirstOrDefault();
            Delete(oldHouseholdEducation);
        }



        public IEnumerable<HouseholdEducation> HouseholdEducationList { get; set; }
        public int Count(Func<HouseholdEducation, bool> predicate)
        {
            return HouseholdEducationList.Count();
        }
        public IEnumerable<HouseholdEducation> Select(int index = 0, int count = int.MaxValue)
        {
            var householdEducationList = from householdEducation in Context.HouseholdEducations
                                  select householdEducation;

            return householdEducationList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<HouseholdEducation> Select(Func<HouseholdEducation, bool> predicate, int index, int count)
        {
            var householdEducationList = (from householdEducation in Context.HouseholdEducations
                                   select householdEducation).Where(predicate);

            return householdEducationList.Skip(index).Take(count).ToArray();
        }
        public HouseholdEducation GetHouseholdEducationById(int id)
        {
            var householdEducation = Context.HouseholdEducations.SingleOrDefault(a => a.Id == id);

            return householdEducation;
        }
        public IEnumerable<HouseholdEducation> Select(HouseholdEducation filterItem, int index, int count)
        {
            var householdEducationList = from householdEducation in Context.HouseholdEducations
                                  select householdEducation;

           
            if (string.IsNullOrWhiteSpace(filterItem.Name) == false)
            {
                householdEducationList = householdEducationList.Where(t => t.Name.Contains(filterItem.Name));
            }

            return householdEducationList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
