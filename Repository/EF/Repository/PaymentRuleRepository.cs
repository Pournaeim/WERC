using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;
using Model.ViewModels.PaymentRule;

namespace Repository.EF.Repository
{
    public class PaymentRuleRepository : EFBaseRepository<PaymentRule>
    {

        public void CreatePaymentRule(PaymentRule newPaymentRule)
        {
            Add(newPaymentRule);
        }
        public void UpdatePaymentRule(PaymentRule updateablePaymentRule)
        {
            var oldPaymentRule = (from s in Context.PaymentRules where s.Id == updateablePaymentRule.Id select s).FirstOrDefault();

            oldPaymentRule.FirstTeamFee = updateablePaymentRule.FirstTeamFee;
            oldPaymentRule.TypeOfRegistration = updateablePaymentRule.TypeOfRegistration;
            oldPaymentRule.DueDatePrefix = updateablePaymentRule.DueDatePrefix;
            oldPaymentRule.ExtraTeamDiscount = updateablePaymentRule.ExtraTeamDiscount;
            oldPaymentRule.DueDate = updateablePaymentRule.DueDate;
            oldPaymentRule.PaymentTypeId = updateablePaymentRule.PaymentTypeId;

            Update(oldPaymentRule);
        }
        public void DeletePaymentRule(int PaymentRuleId)
        {
            var oldPaymentRule = (from s in Context.PaymentRules where s.Id == PaymentRuleId select s).FirstOrDefault();
            Delete(oldPaymentRule);
        }

        public List<PaymentRule> GetPaymentRuleBySuitableDueDate()
        {
            return (from s in Context.PaymentRules
                    orderby s.DueDate
                    select s).ToList();
        }
        public List<PaymentRule> GetPaymentRuleBySuitableDueDateByPaymentTypeId(int paymentTypeId)
        {
            return (from s in Context.PaymentRules
                    where s.PaymentTypeId == paymentTypeId
                    orderby s.DueDate
                    select s).ToList();
        }
        public PaymentRule GetPaymentRuleByDueDate(DateTime dueDate)
        {
            return (from s in Context.PaymentRules
                    where s.DueDate == dueDate
                    select s).FirstOrDefault();
        }
        #region 2021
        public PaymentRule GetPaymentRuleByDueDateByPaymentType(DateTime dueDate, int paymentTypeId)
        {
            return (from s in Context.PaymentRules
                    where s.DueDate == dueDate && s.PaymentTypeId == paymentTypeId
                    select s).FirstOrDefault();
        }
        #endregion 2021

        public IEnumerable<PaymentRule> EntityList { get; set; }
        public int Count(Func<PaymentRule, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<PaymentRule> Select(int index = 0, int count = int.MaxValue)
        {
            var paymentRuleList = from paymentRule in Context.PaymentRules
                                  select paymentRule;

            return paymentRuleList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<PaymentRule> Select(Func<PaymentRule, bool> predicate, int index, int count)
        {
            var paymentRuleList = (from paymentRule in Context.PaymentRules
                                   select paymentRule).Where(predicate);

            return paymentRuleList.Skip(index).Take(count).ToArray();
        }
        public PaymentRule GetPaymentRuleById(int id)
        {
            var paymentRule = Context.PaymentRules.SingleOrDefault(a => a.Id == id);

            return paymentRule;
        }
        public IEnumerable<VmPaymentRule> Select(PaymentRule filterItem, int index, int count)
        {
            var paymentRuleList = from paymentRule in Context.PaymentRules
                                  join paymentType in Context.PaymentTypes
                                  on paymentRule.PaymentTypeId equals paymentType.Id
                                  select new VmPaymentRule
                                  {
                                      Id = paymentRule.Id,
                                      TypeOfRegistration = paymentRule.TypeOfRegistration,
                                      FirstTeamFee = paymentRule.FirstTeamFee,
                                      ExtraTeamDiscount = paymentRule.ExtraTeamDiscount,
                                      DueDate = paymentRule.DueDate.ToString(),
                                      DueDatePrefix = paymentRule.DueDatePrefix,
                                      PaymentTypeId = paymentRule.PaymentTypeId,
                                      PaymentType = paymentType.Name,
                                      PaymentTypeDescription = paymentType.Description,
                                  };

            if (filterItem.TypeOfRegistration != null)
            {
                paymentRuleList = paymentRuleList.Where(t => t.TypeOfRegistration.Contains(filterItem.TypeOfRegistration));
            }

            if (filterItem.FirstTeamFee != 0)
            {
                paymentRuleList = paymentRuleList.Where(t => t.FirstTeamFee == filterItem.FirstTeamFee);
            }

            if (filterItem.ExtraTeamDiscount != 0)
            {
                paymentRuleList = paymentRuleList.Where(t => t.ExtraTeamDiscount == filterItem.ExtraTeamDiscount);
            }

            if (filterItem.DueDate > DateTime.Parse("1/1/0001"))
            {
                paymentRuleList = paymentRuleList.Where(t => DateTime.Parse(t.DueDate) == filterItem.DueDate);
            }

            return paymentRuleList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
