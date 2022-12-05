using BLL.Base;

using Model;
using Model.ViewModels.SubmissionRule;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLSubmissionRule : BLBase
    {
        SubmissionRuleRepository submissionRuleRepository;
        TaskSubmissionRuleRepository taskSubmissionRuleRepository;
        ViewTeamSubmissionRuleRepository viewTeamSubmissionRuleRepository;
        ViewTaskSubmissionRuleRepository viewTaskSubmissionRuleRepository;
        public BLSubmissionRule()
        {
            submissionRuleRepository = UnitOfWork.GetRepository<SubmissionRuleRepository>();
            taskSubmissionRuleRepository = UnitOfWork.GetRepository<TaskSubmissionRuleRepository>();
            viewTeamSubmissionRuleRepository = UnitOfWork.GetRepository<ViewTeamSubmissionRuleRepository>();
            viewTaskSubmissionRuleRepository = UnitOfWork.GetRepository<ViewTaskSubmissionRuleRepository>();
        }
        public List<VmSubmissionRule> GetAllSubmissionRule()
        {

            var submissionRuleList = submissionRuleRepository.Select().ToList();
            
            string ids = "";

            foreach (var item in submissionRuleList)
            {
                ids += item.SendEmail + " , ";
            }

            var vmSubmissionRuleList = (from submissionRule in submissionRuleList
                                        select new VmSubmissionRule
                                        {
                                            Id = submissionRule.Id,
                                            Description = submissionRule.Description,
                                            Name = submissionRule.Name,
                                            DueDate = submissionRule.DueDate.ToString(),
                                            TaskSubmissionRuleList = (from t in submissionRule.TaskSubmissionRules
                                                                      select new VmTaskSubmissionRule
                                                                      {
                                                                          Id = t.Id,
                                                                          SubmissionRuleId = t.SubmissionRuleId,
                                                                          TaskId = t.TaskId,
                                                                      }).ToList(),

                                            PayStatus = submissionRule.PayStatus,
                                            RegistrationStatus = submissionRule.RegistrationStatus,
                                            ShowLate = submissionRule.ShowLate,
                                            ShowReport = submissionRule.ShowReport,
                                            SendEmail = submissionRule.SendEmail,


                                        }).ToList();
            return vmSubmissionRuleList;
        }
        public VmSubmissionRule GetSubmissionRuleById(int id)
        {
            var submissionRule = submissionRuleRepository.GetSubmissionRuleById(id);
            var vmSubmissionRule = new VmSubmissionRule
            {
                Id = submissionRule.Id,
                Description = submissionRule.Description,
                Name = submissionRule.Name,
                DueDate = submissionRule.DueDate.ToString(),
                TaskSubmissionRuleList = (from t in submissionRule.TaskSubmissionRules
                                          select new VmTaskSubmissionRule
                                          {
                                              Id = t.Id,
                                              SubmissionRuleId = t.SubmissionRuleId,
                                              TaskId = t.TaskId,
                                          }).ToList(),

                PayStatus = submissionRule.PayStatus,
                RegistrationStatus = submissionRule.RegistrationStatus,
                ShowLate = submissionRule.ShowLate,
                ShowReport = submissionRule.ShowReport,
                SendEmail = submissionRule.SendEmail,


            };

            return vmSubmissionRule;
        }

        public VmSubmissionRule GetSubmissionRuleBySuitableDueDate(DateTime dueDate)
        {
            SubmissionRule submissionRule = submissionRuleRepository.GetSubmissionRuleByDueDate(dueDate);

            if (submissionRule == null)
            {
                var submissionRuleList = submissionRuleRepository.GetSubmissionRuleBySuitableDueDate();

                var maxDueDate = submissionRuleList.Max(i => i.DueDate);

                if (dueDate > maxDueDate)
                {
                    return null;
                }

                var minDueDate = submissionRuleList.Min(i => i.DueDate);

                if (dueDate <= minDueDate)
                {
                    return (from s in submissionRuleList
                            where s.DueDate == minDueDate
                            select new VmSubmissionRule
                            {
                                Id = s.Id,
                                Description = s.Description,
                                DueDate = s.DueDate.ToString(),
                                TaskSubmissionRuleList = (from t in submissionRule.TaskSubmissionRules
                                                          select new VmTaskSubmissionRule
                                                          {
                                                              Id = t.Id,
                                                              SubmissionRuleId = t.SubmissionRuleId,
                                                              TaskId = t.TaskId,
                                                          }).ToList(),
                                PayStatus = s.PayStatus,
                                RegistrationStatus = s.RegistrationStatus,
                                ShowLate = submissionRule.ShowLate,
                                ShowReport = submissionRule.ShowReport,
                                SendEmail = submissionRule.SendEmail,


                            }).FirstOrDefault();

                }
                else
                {
                    submissionRule = BinarySearchRecursive(submissionRuleList, dueDate, 0, submissionRuleList.Count - 1);
                }
            }

            VmSubmissionRule vmSubmissionRule = null;

            if (submissionRule != null)
            {
                vmSubmissionRule = new VmSubmissionRule
                {
                    Id = submissionRule.Id,
                    Description = submissionRule.Description,
                    Name = submissionRule.Name,
                    DueDate = submissionRule.DueDate.ToString(),

                    PayStatus = submissionRule.PayStatus,
                    RegistrationStatus = submissionRule.RegistrationStatus,
                    ShowLate = submissionRule.ShowLate,
                    ShowReport = submissionRule.ShowReport,
                    SendEmail = submissionRule.SendEmail,


                };
            }

            return vmSubmissionRule;
        }

        public List<VmSubmissionRule> GetSubmissionRuleByTeam(int id)
        {
            var viewSubmissionRule = viewTaskSubmissionRuleRepository.GetViewTeamSubmissionRulesByTeam(id);
            var vmSubmissionRule = from submissionRule in viewSubmissionRule
                                   select new VmSubmissionRule
                                   {
                                       Id = submissionRule.Id,
                                       Description = submissionRule.Description,
                                       Name = "Submit " + submissionRule.Name,
                                       DueDate = submissionRule.DueDate.ToString(),
                                       DueDateOrder = submissionRule.DueDate,

                                       SubmissionRuleUrl = submissionRule.SubmissionRuleUrl,

                                       PayStatus = submissionRule.PayStatus,
                                       RegistrationStatus = submissionRule.RegistrationStatus,

                                       TeamPayStatus = submissionRule.TeamPayStatus,
                                       TeamRegistrationStatus = submissionRule.TeamRegistrationStatus,
                                       ShowLate = submissionRule.ShowLate,
                                       ShowReport = submissionRule.ShowReport,
                                       UploadDate = submissionRule.UploadDate,
                                       SendEmail = submissionRule.SendEmail,

                                   };

            return vmSubmissionRule.OrderBy(p => p.DueDateOrder).ToList();
        }
        public List<VmTeamSubmissionRule> GetTeamSubmissionRules()
        {
            var viewSubmissionRule = viewTaskSubmissionRuleRepository.GetTeamSubmissionRules();
            var vmTeamSubmissionRule = from teamSubmissionRule in viewSubmissionRule
                                       select new VmTeamSubmissionRule
                                       {
                                           Id = teamSubmissionRule.Id,
                                           TeamId = teamSubmissionRule.TeamId,
                                           UploadDate = teamSubmissionRule.UploadDate,
                                           SubmissionRuleId = teamSubmissionRule.SubmissionRuleId
                                       };

            return vmTeamSubmissionRule.ToList();
        }
        public List<VmSubmissionRule> GetSubmissionRuleByTeams(int[] ids)
        {
            var viewSubmissionRule = viewTaskSubmissionRuleRepository.GetViewTeamSubmissionRulesByTeams(ids);
            var vmSubmissionRule = from submissionRule in viewSubmissionRule
                                   select new VmSubmissionRule
                                   {
                                       Id = submissionRule.Id,
                                       TeamId = submissionRule.TeamId,
                                       Description = submissionRule.Description,
                                       Name = submissionRule.Name,
                                       DueDate = submissionRule.DueDate.ToString(),
                                       SubmissionRuleUrl = submissionRule.SubmissionRuleUrl,

                                       PayStatus = submissionRule.PayStatus,
                                       RegistrationStatus = submissionRule.RegistrationStatus,

                                       TeamPayStatus = submissionRule.TeamPayStatus,
                                       TeamRegistrationStatus = submissionRule.TeamRegistrationStatus,
                                       ShowLate = submissionRule.ShowLate,
                                       ShowReport = submissionRule.ShowReport,
                                       UploadDate = submissionRule.UploadDate,
                                       SendEmail = submissionRule.SendEmail,

                                   };

            return vmSubmissionRule.ToList();
        }
        public List<VmSubmissionRule> GetTeamsSubmissionRuleById(int id)
        {
            var viewSubmissionRule = viewTeamSubmissionRuleRepository.GetTeamsSubmissionRuleById(id);
            var vmSubmissionRule = from submissionRule in viewSubmissionRule
                                   select new VmSubmissionRule
                                   {
                                       Id = submissionRule.Id.Value,
                                       TeamId = submissionRule.TeamId.Value,
                                       TeamName = submissionRule.TeamName,
                                       Description = submissionRule.Description,
                                       Name = submissionRule.Name,
                                       DueDate = submissionRule.DueDate.ToString(),
                                       SubmissionRuleUrl = submissionRule.SubmissionRuleUrl,

                                       PayStatus = submissionRule.PayStatus,
                                       RegistrationStatus = submissionRule.RegistrationStatus,
                                       ShowLate = submissionRule.ShowLate,
                                       ShowReport = submissionRule.ShowReport,
                                       UploadDate = submissionRule.UploadDate,
                                       SendEmail = submissionRule.SendEmail,


                                   };

            return vmSubmissionRule.ToList();
        }
        public List<VmSubmissionRule> GetDownloadTeamsSubmissionRuleById(int id)
        {
            var viewSubmissionRule = viewTeamSubmissionRuleRepository.GetDownloadTeamsSubmissionRuleById(id);
            var vmSubmissionRule = from submissionRule in viewSubmissionRule
                                   select new VmSubmissionRule
                                   {
                                       Id = submissionRule.Id,
                                       TeamId = submissionRule.TeamId,
                                       TeamName = submissionRule.TeamName,
                                       Description = submissionRule.Description,
                                       Name = submissionRule.Name,
                                       DueDate = submissionRule.DueDate.ToString(),
                                       SubmissionRuleUrl = submissionRule.SubmissionRuleUrl,

                                       PayStatus = submissionRule.PayStatus,
                                       RegistrationStatus = submissionRule.RegistrationStatus,
                                       ShowLate = submissionRule.ShowLate,
                                       ShowReport = submissionRule.ShowReport,
                                       UploadDate = submissionRule.UploadDate,
                                       SendEmail = submissionRule.SendEmail,


                                   };

            return vmSubmissionRule.ToList();
        }
        public List<VmSubmissionRule> GetDownloadTeamsSubmissionRuleByTeamId(int[] teamIds)
        {
            var viewSubmissionRule = viewTeamSubmissionRuleRepository.GetDownloadTeamsSubmissionRuleByTeamId(teamIds);
            var vmSubmissionRule = from submissionRule in viewSubmissionRule
                                   select new VmSubmissionRule
                                   {
                                       Id = submissionRule.Id,
                                       TeamId = submissionRule.TeamId,
                                       TeamName = submissionRule.TeamName,
                                       Description = submissionRule.Description,
                                       Name = submissionRule.Name,
                                       DueDate = submissionRule.DueDate.ToString(),
                                       SubmissionRuleUrl = submissionRule.SubmissionRuleUrl,

                                       PayStatus = submissionRule.PayStatus,
                                       RegistrationStatus = submissionRule.RegistrationStatus,
                                       ShowLate = submissionRule.ShowLate,
                                       ShowReport = submissionRule.ShowReport,
                                       UploadDate = submissionRule.UploadDate,
                                       SendEmail = submissionRule.SendEmail,


                                   };

            return vmSubmissionRule.ToList();
        }

        public List<VmSubmissionRule> GetSubmissionRulesByFilter(VmSubmissionRule filterItem)
        {
            var viewFilterItem = new SubmissionRule
            {
                Id = filterItem.Id,
                Description = filterItem.Description,
                Name = filterItem.Name,
                DueDate = DateTime.Parse(filterItem.DueDate ?? "1/1/0001"),
            };

            var submissionRuleList = submissionRuleRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmSubmissionRuleList = (from submissionRule in submissionRuleList
                                        select new VmSubmissionRule
                                        {
                                            Id = submissionRule.Id,
                                            Description = submissionRule.Description,
                                            Name = submissionRule.Name,
                                            DueDate = submissionRule.DueDate.ToString(),
                                            DueDateOrder = submissionRule.DueDate,
                                            TaskSubmissionRuleList = (from t in submissionRule.TaskSubmissionRules
                                                                      select new VmTaskSubmissionRule
                                                                      {
                                                                          Id = t.Id,
                                                                          SubmissionRuleId = t.SubmissionRuleId,
                                                                          TaskId = t.TaskId,
                                                                      }).ToList(),

                                            PayStatus = submissionRule.PayStatus,
                                            RegistrationStatus = submissionRule.RegistrationStatus,
                                            ShowLate = submissionRule.ShowLate,
                                            ShowReport = submissionRule.ShowReport,
                                            SendEmail = submissionRule.SendEmail,

                                        }).ToList();

            foreach (var item in vmSubmissionRuleList)
            {
                item.ClientTaskIds = string.Join(",", (from t in item.TaskSubmissionRuleList select t.TaskId).ToArray());
                item.TaskIds = (from t in item.TaskSubmissionRuleList select t.TaskId).ToArray();
            }
            return vmSubmissionRuleList.OrderBy(p => p.DueDateOrder).ToList();
        }

        public int CreateSubmissionRule(VmSubmissionRule vmSubmissionRule)
        {
            var result = -1;
            try
            {
                var newSubmissionRule = new SubmissionRule
                {
                    Id = vmSubmissionRule.Id,
                    Description = vmSubmissionRule.Description,
                    Name = vmSubmissionRule.Name,
                    DueDate = DateTime.Parse(vmSubmissionRule.DueDate),

                    PayStatus = vmSubmissionRule.PayStatus,
                    RegistrationStatus = vmSubmissionRule.RegistrationStatus,
                    ShowLate = vmSubmissionRule.ShowLate,
                    ShowReport = vmSubmissionRule.ShowReport,
                    SendEmail = vmSubmissionRule.SendEmail,
                };

                submissionRuleRepository.CreateSubmissionRule(newSubmissionRule);

                var taskIdList = vmSubmissionRule.ClientTaskIds.Split(new char[] { ',' }, StringSplitOptions.None);
                vmSubmissionRule.TaskIds = new int[taskIdList.Length];

                for (var i = 0; i < taskIdList.Length; i++)
                {
                    vmSubmissionRule.TaskIds[i] = int.Parse(taskIdList[i]);
                }

                newSubmissionRule.TaskSubmissionRules = (from t in vmSubmissionRule.TaskIds
                                                         select new TaskSubmissionRule
                                                         {
                                                             TaskId = t,
                                                         }).ToList();
                UnitOfWork.Commit();

                result = newSubmissionRule.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateSubmissionRule(VmSubmissionRule vmSubmissionRule)
        {

            var SubmissionRuleRepository = UnitOfWork.GetRepository<SubmissionRuleRepository>();

            var updateableSubmissionRule = new SubmissionRule
            {
                Id = vmSubmissionRule.Id,
                Name = vmSubmissionRule.Name,
                Description = vmSubmissionRule.Description,
                DueDate = DateTime.Parse(vmSubmissionRule.DueDate),

                PayStatus = vmSubmissionRule.PayStatus,
                RegistrationStatus = vmSubmissionRule.RegistrationStatus,
                ShowLate = vmSubmissionRule.ShowLate,
                ShowReport = vmSubmissionRule.ShowReport,
                SendEmail = vmSubmissionRule.SendEmail,
            };

            var taskIdList = vmSubmissionRule.ClientTaskIds.Split(new char[] { ',' }, StringSplitOptions.None);
            vmSubmissionRule.TaskIds = new int[taskIdList.Length];

            for (var i = 0; i < taskIdList.Length; i++)
            {
                vmSubmissionRule.TaskIds[i] = int.Parse(taskIdList[i]);
            }

            SubmissionRuleRepository.UpdateSubmissionRule(updateableSubmissionRule);

            taskSubmissionRuleRepository.DeleteTaskSubmissionRules(vmSubmissionRule.Id);
            taskSubmissionRuleRepository.CreateTaskSubmissionRules(vmSubmissionRule.Id, vmSubmissionRule.TaskIds);


            return UnitOfWork.Commit();

        }
        public bool DeleteSubmissionRule(int SubmissionRuleId)
        {
            try
            {
                var SubmissionRuleRepository = UnitOfWork.GetRepository<SubmissionRuleRepository>();


                SubmissionRuleRepository.DeleteSubmissionRule(SubmissionRuleId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

        public SubmissionRule BinarySearchRecursive(List<SubmissionRule> inputArray, DateTime key, int min, int max)
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
