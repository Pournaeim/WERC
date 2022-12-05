using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class GoalsAfterGraduationRepository : EFBaseRepository<GoalsAfterGraduation>
    {
        public IEnumerable<GoalsAfterGraduation> Select(int index, int count)
        {
            var goalsAfterGraduationList = from GoalsAfterGraduation in Context.GoalsAfterGraduations
                                           select GoalsAfterGraduation;

            return goalsAfterGraduationList.OrderBy(A => A.OrderNo).Skip(index).Take(count).ToArray();
        }

        public int GetGoalsAfterGraduationNewId()
        {
            return Context.GoalsAfterGraduations.Max(d => d.Id) + 1;

        }
        public int GetGoalsAfterGraduationNewOrderNmber()
        {
            return Context.GoalsAfterGraduations.Where(g => g.Id != 4).Max(d => d.OrderNo.Value) + 1;

        }

        public void CreateGoalsAfterGraduation(GoalsAfterGraduation newGoalsAfterGraduation)
        {
            Add(newGoalsAfterGraduation);
        }
    }
}
