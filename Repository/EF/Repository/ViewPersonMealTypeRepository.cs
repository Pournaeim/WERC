using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewPersonMealTypeRepository : EFBaseRepository<ViewPersonMealType>
    {

        public IEnumerable<ViewPersonMealType> GetViewPersonSubmissionRulesByPerson(int personId)
        {

            var viewPersonMealTypeList = from ts in Context.ViewPersonMealTypes
                                         where ts.PersonId == personId
                                         select ts;

            return viewPersonMealTypeList.ToArray();
        }

        public IEnumerable<ViewPersonMealType> GetViewPersonSubmissionRulesByPersons(int[] personIds)
        {

            var viewPersonMealTypeList = from ts in Context.ViewPersonMealTypes
                                         where personIds.Contains(ts.PersonId)
                                         select ts;

            return viewPersonMealTypeList.ToArray();
        }



    }
}
