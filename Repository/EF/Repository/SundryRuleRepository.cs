using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class SundryRuleRepository : EFBaseRepository<SundryRule>
    {

        public void CreateSundryRule(SundryRule newSundryRule)
        {
            Add(newSundryRule);
        }
        public void UpdateSundryRule(SundryRule updateableSundryRule)
        {
            var oldSundryRule = (from s in Context.SundryRules where s.Id == updateableSundryRule.Id select s).FirstOrDefault();

            oldSundryRule.Description = updateableSundryRule.Description;
            oldSundryRule.DueDate = updateableSundryRule.DueDate;

            Update(oldSundryRule);
        }
        public void DeleteSundryRule(int SundryRuleId)
        {
            var oldSundryRule = (from s in Context.SundryRules where s.Id == SundryRuleId select s).FirstOrDefault();
            Delete(oldSundryRule);
        }

        public List<SundryRule> GetSundryRuleBySuitableDueDate()
        {
            return (from s in Context.SundryRules
                    orderby s.DueDate
                    select s).ToList();
        }
        public SundryRule GetSundryRuleByDueDate(DateTime dueDate)
        {
            return (from s in Context.SundryRules
                    where s.DueDate == dueDate
                    select s).FirstOrDefault();
        }

        public IEnumerable<SundryRule> EntityList { get; set; }
        public int Count(Func<SundryRule, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<SundryRule> Select(int index = 0, int count = int.MaxValue)
        {
            var sundryRuleList = from sundryRule in Context.SundryRules
                                  select sundryRule;

            return sundryRuleList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<SundryRule> Select(Func<SundryRule, bool> predicate, int index, int count)
        {
            var sundryRuleList = (from sundryRule in Context.SundryRules
                                   select sundryRule).Where(predicate);

            return sundryRuleList.Skip(index).Take(count).ToArray();
        }
        public SundryRule GetSundryRuleById(int id)
        {
            var sundryRule = Context.SundryRules.SingleOrDefault(a => a.Id == id);

            return sundryRule;
        }
        public IEnumerable<SundryRule> Select(SundryRule filterItem, int index, int count)
        {
            var sundryRuleList = from sundryRule in Context.SundryRules
                                  select sundryRule;

            if (filterItem.Description != null)
            {
                sundryRuleList = sundryRuleList.Where(t => t.Description.Contains(filterItem.Description));
            }

            if (filterItem.DueDate > DateTime.Parse("1/1/0001"))
            {
                sundryRuleList = sundryRuleList.Where(t => t.DueDate == filterItem.DueDate);
            }

            return sundryRuleList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
