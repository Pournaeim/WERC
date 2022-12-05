using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Repository.EF.Repository
{
    public class SubmissionRuleRepository : EFBaseRepository<SubmissionRule>
    {

        public void CreateSubmissionRule(SubmissionRule newSubmissionRule)
        {
            Add(newSubmissionRule);
        }
        public void UpdateSubmissionRule(SubmissionRule updateableSubmissionRule)
        {
            var oldSubmissionRule = (from s in Context.SubmissionRules where s.Id == updateableSubmissionRule.Id select s).FirstOrDefault();

            oldSubmissionRule.Name = updateableSubmissionRule.Name;
            oldSubmissionRule.Description = updateableSubmissionRule.Description;
            oldSubmissionRule.DueDate = updateableSubmissionRule.DueDate;
            oldSubmissionRule.TaskSubmissionRules = updateableSubmissionRule.TaskSubmissionRules;

            oldSubmissionRule.PayStatus = updateableSubmissionRule.PayStatus;
            oldSubmissionRule.RegistrationStatus = updateableSubmissionRule.RegistrationStatus;
            oldSubmissionRule.ShowLate = updateableSubmissionRule.ShowLate;
            oldSubmissionRule.ShowReport = updateableSubmissionRule.ShowReport;
            oldSubmissionRule.SendEmail = updateableSubmissionRule.SendEmail;

            Update(oldSubmissionRule);
        }
        public void DeleteSubmissionRule(int SubmissionRuleId)
        {
            var oldSubmissionRule = (from s in Context.SubmissionRules where s.Id == SubmissionRuleId select s).FirstOrDefault();
            Delete(oldSubmissionRule);
        }

        public List<SubmissionRule> GetSubmissionRuleBySuitableDueDate()
        {
            return (from s in Context.SubmissionRules
                    orderby s.DueDate
                    select s).ToList();
        }
        public SubmissionRule GetSubmissionRuleByDueDate(DateTime dueDate)
        {
            return (from s in Context.SubmissionRules.Include("TaskSubmissionRules")
                    where s.DueDate == dueDate
                    select s).FirstOrDefault();
        }

        public IEnumerable<SubmissionRule> EntityList { get; set; }
        public int Count(Func<SubmissionRule, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<SubmissionRule> Select(int index = 0, int count = int.MaxValue)
        {
            var submissionRuleList = (from submissionRule in Context.SubmissionRules.Include("TaskSubmissionRules")
                                     select submissionRule).ToList();

            return submissionRuleList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }

        public IEnumerable<SubmissionRule> Select(Func<SubmissionRule, bool> predicate, int index, int count)
        {
            var submissionRuleList = (from submissionRule in Context.SubmissionRules.Include("TaskSubmissionRules")
                                      select submissionRule).Where(predicate);

            return submissionRuleList.Skip(index).Take(count).ToArray();
        }

        public SubmissionRule GetSubmissionRuleById(int id)
        {
            var submissionRule = Context.SubmissionRules.Include("TaskSubmissionRules").SingleOrDefault(a => a.Id == id);

            return submissionRule;
        }

        public IEnumerable<SubmissionRule> Select(SubmissionRule filterItem, int index, int count)
        {
            var submissionRuleList = from submissionRule in Context.SubmissionRules.Include("TaskSubmissionRules")
                                     select submissionRule;

            if (filterItem.Description != null)
            {
                submissionRuleList = submissionRuleList.Where(t => t.Description.Contains(filterItem.Description));
            }

            if (filterItem.DueDate > DateTime.Parse("1/1/0001"))
            {
                submissionRuleList = submissionRuleList.Where(t => t.DueDate == filterItem.DueDate);
            }

            return submissionRuleList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
