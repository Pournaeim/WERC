using System.Collections.Generic;
using System;
using BLL.Base;
using System.Linq;
using Model.ViewModels.Team;
using Repository.EF.Repository;
using Model;
using Model.ViewModels.PaymentRule;

namespace BLL
{
    public class BLPaymentRule : BLBase
    {
        public VmPaymentRule GetPaymentRuleById(int id)
        {
            var paymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

            var paymentRule = paymentRuleRepository.GetPaymentRuleById(id);
            var vmPaymentRule = new VmPaymentRule
            {
                Id = paymentRule.Id,
                TypeOfRegistration = paymentRule.TypeOfRegistration,
                FirstTeamFee = paymentRule.FirstTeamFee,
                ExtraTeamDiscount = paymentRule.ExtraTeamDiscount,
                DueDate = paymentRule.DueDate.ToString(),
                DueDatePrefix = paymentRule.DueDatePrefix,
                PaymentTypeId = paymentRule.PaymentTypeId,
            };

            return vmPaymentRule;
        }
        public VmPaymentRule GetPaymentRuleBySuitableDueDate(DateTime dueDate)
        {
            var paymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

            PaymentRule paymentRule = paymentRuleRepository.GetPaymentRuleByDueDate(dueDate);

            if (paymentRule == null)
            {
                var paymentRuleList = paymentRuleRepository.GetPaymentRuleBySuitableDueDate();

                var maxDueDate = paymentRuleList.Max(i => i.DueDate);

                if (dueDate > maxDueDate)
                {
                    return null;
                }

                var minDueDate = paymentRuleList.Min(i => i.DueDate);

                if (dueDate <= minDueDate)
                {
                    return (from s in paymentRuleList
                            where s.DueDate == minDueDate
                            select new VmPaymentRule
                            {
                                Id = s.Id,
                                TypeOfRegistration = s.TypeOfRegistration,
                                FirstTeamFee = s.FirstTeamFee,
                                ExtraTeamDiscount = s.ExtraTeamDiscount,
                                DueDate = s.DueDate.ToString(),
                                DueDatePrefix = s.DueDatePrefix,
                            }).FirstOrDefault();

                }
                else
                {
                    paymentRule = BinarySearchRecursive(paymentRuleList, dueDate, 0, paymentRuleList.Count - 1);
                }
            }

            VmPaymentRule vmPaymentRule = null;

            if (paymentRule != null)
            {
                vmPaymentRule = new VmPaymentRule
                {
                    Id = paymentRule.Id,
                    TypeOfRegistration = paymentRule.TypeOfRegistration,
                    FirstTeamFee = paymentRule.FirstTeamFee,
                    ExtraTeamDiscount = paymentRule.ExtraTeamDiscount,
                    DueDate = paymentRule.DueDate.ToString(),
                    DueDatePrefix = paymentRule.DueDatePrefix,
                    PaymentTypeId = paymentRule.PaymentTypeId,
                };
            }

            return vmPaymentRule;
        }

        #region 2021
        public VmPaymentRule GetPaymentRuleBySuitableDueDateByPaymentType(DateTime dueDate, int paymentTypeId)
        {
            var paymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

            PaymentRule paymentRule = paymentRuleRepository.GetPaymentRuleByDueDateByPaymentType(dueDate, paymentTypeId);

            if (paymentRule == null)
            {
                #region 2021
                var paymentRuleList = paymentRuleRepository.GetPaymentRuleBySuitableDueDateByPaymentTypeId(paymentTypeId);
                #endregion 2021

                var maxDueDate = paymentRuleList.Max(i => i.DueDate);

                if (dueDate > maxDueDate)
                {
                    return null;
                }

                var minDueDate = paymentRuleList.Min(i => i.DueDate);

                if (dueDate <= minDueDate)
                {
                    return (from s in paymentRuleList
                            where s.DueDate == minDueDate
                            select new VmPaymentRule
                            {
                                Id = s.Id,
                                TypeOfRegistration = s.TypeOfRegistration,
                                FirstTeamFee = s.FirstTeamFee,
                                ExtraTeamDiscount = s.ExtraTeamDiscount,
                                DueDate = s.DueDate.ToString(),
                                DueDatePrefix = s.DueDatePrefix,
                            }).FirstOrDefault();

                }
                else
                {
                    paymentRule = BinarySearchRecursive(paymentRuleList, dueDate, 0, paymentRuleList.Count - 1);
                }
            }

            VmPaymentRule vmPaymentRule = null;

            if (paymentRule != null)
            {
                vmPaymentRule = new VmPaymentRule
                {
                    Id = paymentRule.Id,
                    TypeOfRegistration = paymentRule.TypeOfRegistration,
                    FirstTeamFee = paymentRule.FirstTeamFee,
                    ExtraTeamDiscount = paymentRule.ExtraTeamDiscount,
                    DueDate = paymentRule.DueDate.ToString(),
                    DueDatePrefix = paymentRule.DueDatePrefix,
                    PaymentTypeId = paymentRule.PaymentTypeId,
                };
            }

            return vmPaymentRule;
        }

        #endregion 2021
        public IEnumerable<VmPaymentRule> GetPaymentRulesByFilter(VmPaymentRule filterItem)
        {
            var paymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

            var viewFilterItem = new PaymentRule
            {
                Id = filterItem.Id,
                TypeOfRegistration = filterItem.TypeOfRegistration,
                FirstTeamFee = filterItem.FirstTeamFee,
                ExtraTeamDiscount = filterItem.ExtraTeamDiscount,
                DueDate = DateTime.Parse(filterItem.DueDate ?? "1/1/0001"),
                DueDatePrefix = filterItem.DueDatePrefix,

            };

            var paymentRuleList = paymentRuleRepository.Select(viewFilterItem, 0, int.MaxValue);
            var vmPaymentRuleList = from paymentRule in paymentRuleList
                                    select new VmPaymentRule
                                    {
                                        Id = paymentRule.Id,
                                        TypeOfRegistration = paymentRule.TypeOfRegistration,
                                        FirstTeamFee = paymentRule.FirstTeamFee,
                                        ExtraTeamDiscount = paymentRule.ExtraTeamDiscount,
                                        DueDate = DateTime.Parse(paymentRule.DueDate).ToString("MM/dd/yyyy HH:mm:ss"),
                                        DueDatePrefix = paymentRule.DueDatePrefix,
                                        PaymentTypeId = paymentRule.PaymentTypeId,
                                        PaymentType = paymentRule.PaymentType,
                                        PaymentTypeDescription = paymentRule.PaymentTypeDescription,
                                    };
            return vmPaymentRuleList;

        }

        public int CreatePaymentRule(VmPaymentRule vmPaymentRule)
        {
            var result = -1;
            try
            {
                var paymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

                var newPaymentRule = new PaymentRule
                {
                    Id = vmPaymentRule.Id,
                    TypeOfRegistration = vmPaymentRule.TypeOfRegistration,
                    FirstTeamFee = vmPaymentRule.FirstTeamFee,
                    ExtraTeamDiscount = vmPaymentRule.ExtraTeamDiscount,
                    DueDate = DateTime.Parse(vmPaymentRule.DueDate),
                    DueDatePrefix = vmPaymentRule.DueDatePrefix,
                    PaymentTypeId = vmPaymentRule.PaymentTypeId,
                };

                paymentRuleRepository.CreatePaymentRule(newPaymentRule);

                UnitOfWork.Commit();

                result = newPaymentRule.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdatePaymentRule(VmPaymentRule vmPaymentRule)
        {

            var PaymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();

            var updateablePaymentRule = new PaymentRule
            {
                Id = vmPaymentRule.Id,
                TypeOfRegistration = vmPaymentRule.TypeOfRegistration,
                FirstTeamFee = vmPaymentRule.FirstTeamFee,
                ExtraTeamDiscount = vmPaymentRule.ExtraTeamDiscount,
                DueDate = DateTime.Parse(vmPaymentRule.DueDate),
                DueDatePrefix = vmPaymentRule.DueDatePrefix,
                PaymentTypeId = vmPaymentRule.PaymentTypeId,
            };

            PaymentRuleRepository.UpdatePaymentRule(updateablePaymentRule);

            return UnitOfWork.Commit();

        }
        public bool DeletePaymentRule(int PaymentRuleId)
        {
            try
            {
                var PaymentRuleRepository = UnitOfWork.GetRepository<PaymentRuleRepository>();


                PaymentRuleRepository.DeletePaymentRule(PaymentRuleId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

        public PaymentRule BinarySearchRecursive(List<PaymentRule> inputArray, DateTime key, int min, int max)
        {
            if (max - min == 1)
            {
                return inputArray[max];
            }
            else
            {
                int mid = (min + max) / 2;

                if (key < inputArray[mid].DueDate)
                {
                    return BinarySearchRecursive(inputArray, key, min, mid);
                }
                else
                {
                    return BinarySearchRecursive(inputArray, key, mid, max);
                }
            }
        }
    }
}
