using BLL.Base;
using Model;
using Model.ViewModels.SundryRule;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLSundryRule : BLBase
    {
        public List<VmSundryRule> GetAllSundryRule()
        {
            var sundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

            var sundryRuleList = sundryRuleRepository.Select();
            var vmSundryRuleList = (from sundryRule in sundryRuleList
                                       select new VmSundryRule
                                       {
                                           Id = sundryRule.Id,
                                           Description = sundryRule.Description,
                                           Name = sundryRule.Name,
                                           DueDate = sundryRule.DueDate.ToString(),
                                       }).ToList();
            return vmSundryRuleList;
        }
        public VmSundryRule GetSundryRuleById(int id)
        {
            var sundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

            var sundryRule = sundryRuleRepository.GetSundryRuleById(id);
            var vmSundryRule = new VmSundryRule
            {
                Id = sundryRule.Id,
                Description = sundryRule.Description,
                Name = sundryRule.Name,
                DueDate = sundryRule.DueDate.ToString(),
            };

            return vmSundryRule;
        }
        public VmSundryRule GetSundryRuleBySuitableDueDate(DateTime dueDate)
        {
            var sundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

            SundryRule sundryRule = sundryRuleRepository.GetSundryRuleByDueDate(dueDate);

            if (sundryRule == null)
            {
                var sundryRuleList = sundryRuleRepository.GetSundryRuleBySuitableDueDate();

                var maxDueDate = sundryRuleList.Max(i => i.DueDate);

                if (dueDate > maxDueDate)
                {
                    return null;
                }

                var minDueDate = sundryRuleList.Min(i => i.DueDate);

                if (dueDate <= minDueDate)
                {
                    return (from s in sundryRuleList
                            where s.DueDate == minDueDate
                            select new VmSundryRule
                            {
                                Id = s.Id,
                                Description = s.Description,
                                DueDate = s.DueDate.ToString(),
                            }).FirstOrDefault();

                }
                else
                {
                    sundryRule = BinarySearchRecursive(sundryRuleList, dueDate, 0, sundryRuleList.Count - 1);
                }
            }

            VmSundryRule vmSundryRule = null;

            if (sundryRule != null)
            {
                vmSundryRule = new VmSundryRule
                {
                    Id = sundryRule.Id,
                    Description = sundryRule.Description,
                    Name = sundryRule.Name,
                    DueDate = sundryRule.DueDate.ToString(),
                };
            }

            return vmSundryRule;
        }
        public IEnumerable<VmSundryRule> GetSundryRulesByFilter(VmSundryRule filterItem)
        {
            var sundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

            var viewFilterItem = new SundryRule
            {
                Id = filterItem.Id,
                Description = filterItem.Description,
                Name = filterItem.Name,
                DueDate = DateTime.Parse(filterItem.DueDate ?? "1/1/0001"),
            };

            var sundryRuleList = sundryRuleRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmSundryRuleList = from sundryRule in sundryRuleList
                                       select new VmSundryRule
                                       {
                                           Id = sundryRule.Id,
                                           Description = sundryRule.Description,
                                           Name = sundryRule.Name,
                                           DueDate = sundryRule.DueDate.ToString(),
                                       };
            return vmSundryRuleList;
        }

        public int CreateSundryRule(VmSundryRule vmSundryRule)
        {
            var result = -1;
            try
            {
                var sundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

                var newSundryRule = new SundryRule
                {
                    Id = vmSundryRule.Id,
                    Description = vmSundryRule.Description,
                    Name = vmSundryRule.Name,
                    DueDate = DateTime.Parse(vmSundryRule.DueDate),
                };

                sundryRuleRepository.CreateSundryRule(newSundryRule);

                UnitOfWork.Commit();

                result = newSundryRule.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateSundryRule(VmSundryRule vmSundryRule)
        {

            var SundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();

            var updateableSundryRule = new SundryRule
            {
                Id = vmSundryRule.Id,
                Name = vmSundryRule.Name,
                Description = vmSundryRule.Description,
                DueDate = DateTime.Parse(vmSundryRule.DueDate),
            };

            SundryRuleRepository.UpdateSundryRule(updateableSundryRule);

            return UnitOfWork.Commit();

        }
        public bool DeleteSundryRule(int SundryRuleId)
        {
            try
            {
                var SundryRuleRepository = UnitOfWork.GetRepository<SundryRuleRepository>();


                SundryRuleRepository.DeleteSundryRule(SundryRuleId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

        public SundryRule BinarySearchRecursive(List<SundryRule> inputArray, DateTime key, int min, int max)
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
