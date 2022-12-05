using BLL.Base;
using Model;
using Model.ViewModels.TeamSafetyItem;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLTeamSafetyItem : BLBase
    {
        public IEnumerable<VmTeamSafetyItem> GetTeamSafetyItemByTeamId(int teamId)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItemList = safetyItemRepository.GetAllSafetyItems();

                var vmTeamSafetyItemList = new List<VmTeamSafetyItem>();

                foreach (var safetyItem in safetyItemList)
                {
                    foreach (var stsi in safetyItem.SubSafetyItems)
                    {
                        vmTeamSafetyItemList.Add(new VmTeamSafetyItem
                        {
                            TeamId = teamId,
                            SafetyItemId = safetyItem.Id,
                            SubSafetyItemId = stsi.Id,
                            LastContent = "",
                            LastComment = "",
                            ItemStatus = null,
                            AttachedFileUrl = "",
                            SafetyItemName = stsi.Name,
                            Priority = stsi.Priority,
                            Instruction = stsi.Instruction,
                            AttachmentRequired = stsi.AttachmentRequired,
                            TextRequired = stsi.TextRequired,

                        });
                    }
                }

                var viewTeamSafetyItemRepository = UnitOfWork.GetRepository<ViewTeamSafetyItemRepository>();

                var teamSafetyItemAnswerList = viewTeamSafetyItemRepository.GetViewTeamSafetyItemByTeamId(teamId);

                if (teamSafetyItemAnswerList.Count() > 0)
                {
                    foreach (var teamSafetyItem in vmTeamSafetyItemList)
                    {
                        var answer = teamSafetyItemAnswerList.Where(s => s.SafetyItemId == teamSafetyItem.SafetyItemId && s.SubSafetyItemId == teamSafetyItem.SubSafetyItemId).FirstOrDefault();

                        if (answer != null)
                        {
                            teamSafetyItem.Id = answer.Id;
                            teamSafetyItem.TeamId = answer.TeamId;
                            teamSafetyItem.LastContent = answer.LastContent ?? "";
                            teamSafetyItem.LastComment = answer.LastComment ?? "";
                            teamSafetyItem.ItemStatus = answer.ItemStatus;
                            teamSafetyItem.AttachedFileUrl = answer.AttachedFileUrl;
                            teamSafetyItem.TeamName = answer.TeamName;

                        }
                    }
                }

                return vmTeamSafetyItemList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmTeamSafetyItem> GetEspReportTeamSafetyItemByTeamId(int teamId)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItemList = safetyItemRepository.GetAllSafetyItems();

                var vmTeamSafetyItemList = new List<VmTeamSafetyItem>();

                foreach (var safetyItem in safetyItemList)
                {
                    foreach (var stsi in safetyItem.SubSafetyItems)
                    {
                        vmTeamSafetyItemList.Add(new VmTeamSafetyItem
                        {
                            TeamId = teamId,
                            SafetyItemId = safetyItem.Id,
                            SubSafetyItemId = stsi.Id,
                            LastContent = "",
                            LastComment = "",
                            ItemStatus = null,
                            AttachedFileUrl = "",
                            BaseSafetyItemName = safetyItem.Name,
                            SafetyItemName = stsi.Name,
                            SubSafetyItemName = stsi.Name,
                            Priority = stsi.Priority,
                            BasePriority = safetyItem.Priority,
                            Instruction = stsi.Instruction,
                            BaseInstruction = safetyItem.Instruction,
                            AttachmentRequired = stsi.AttachmentRequired,
                            TextRequired = stsi.TextRequired,

                        });
                    }
                }

                var viewTeamSafetyItemRepository = UnitOfWork.GetRepository<ViewTeamSafetyItemRepository>();

                var teamSafetyItemAnswerList = viewTeamSafetyItemRepository.GetViewTeamSafetyItemByTeamId(teamId);

                if (teamSafetyItemAnswerList.Count() > 0)
                {
                    foreach (var teamSafetyItem in vmTeamSafetyItemList)
                    {
                        var answer = teamSafetyItemAnswerList.Where(s => s.SafetyItemId == teamSafetyItem.SafetyItemId && s.SubSafetyItemId == teamSafetyItem.SubSafetyItemId).FirstOrDefault();

                        if (answer != null)
                        {
                            teamSafetyItem.Id = answer.Id;
                            teamSafetyItem.TeamId = answer.TeamId;
                            teamSafetyItem.LastContent = answer.LastContent ?? "";
                            teamSafetyItem.LastComment = answer.LastComment ?? "";
                            teamSafetyItem.ItemStatus = answer.ItemStatus;
                            teamSafetyItem.AttachedFileUrl = answer.AttachedFileUrl;
                            teamSafetyItem.TeamName = answer.TeamName;

                        }
                    }
                }

                //var grouptESPReportData = (from t in vmTeamSafetyItemList
                //                           group t by
                //                           new
                //                           {
                //                               t.BaseSafetyItemName,
                //                               t.SafetyItemId,
                //                               t.BaseInstruction,
                //                               t.BasePriority,
                //                           } into g

                //                           select new VmESPReportTeamSafetyItem
                //                           {
                //                               Name = g.Key.BaseSafetyItemName,
                //                               Id = g.Key.SafetyItemId,
                //                               Instruction = g.Key.BaseInstruction,
                //                               Priority = g.Key.BasePriority,

                //                               TeamSafetyItems = (from t in g.ToList()
                //                                                  select new VmTeamSafetyItem
                //                                                  {
                //                                                      Id = t.Id,
                //                                                      TeamId = t.TeamId,
                //                                                      LastContent = t.LastContent ?? "",
                //                                                      LastComment = t.LastComment ?? "",
                //                                                      ItemStatus = t.ItemStatus,
                //                                                      Instruction = t.Instruction,
                //                                                      AttachedFileUrl = t.AttachedFileUrl,
                //                                                      TeamName = t.TeamName,
                //                                                      SubSafetyItemName = t.SafetyItemName,
                //                                                      Priority = t.Priority,

                //                                                  }).ToList(),

                //                               TeamSafetyItemDetails = GetTeamSafetyItemDetails(teamId, g.Key.SafetyItemId),


                //                           }).ToList();
                return vmTeamSafetyItemList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmTeamSafetyItem> GetSafetyAdminTeamSafetyItemByTeamId(int teamId)
        {
            try
            {
                var viewTeamSafetyItemRepository = UnitOfWork.GetRepository<ViewTeamSafetyItemRepository>();

                var viewTeamSafetyItemList = viewTeamSafetyItemRepository.GetViewTeamSafetyItemByTeamId(teamId);

                var vmTeamSafetyItemList = from t in viewTeamSafetyItemList
                                           select new VmTeamSafetyItem
                                           {
                                               Id = t.Id,
                                               TeamId = t.TeamId,
                                               SafetyItemId = t.SafetyItemId,
                                               LastContent = t.LastContent ?? "",
                                               LastComment = t.LastComment ?? "",
                                               ItemStatus = t.ItemStatus,
                                               AttachedFileUrl = t.AttachedFileUrl,
                                               Instruction = t.Instruction,
                                               Priority = t.Priority,
                                               SafetyItemName = t.SafetyItemName,
                                               AttachmentRequired = t.AttachmentRequired,
                                               TextRequired = t.TextRequired,
                                               TeamName = t.TeamName,
                                           };

                return vmTeamSafetyItemList.OrderBy(s => s.Priority);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<VmTeamSafetyItemDetail> GetTeamSafetyItemDetails(int teamId, int safetyItemId)
        {
            try
            {
                var teamSafetyItemDetailRepository = UnitOfWork.GetRepository<TeamSafetyItemDetailRepository>();

                var teamSafetyItemDetailList = teamSafetyItemDetailRepository.GetTeamSafetyItemDetails(teamId, safetyItemId);

                var vmTeamSafetyItemDetailList = from t in teamSafetyItemDetailList
                                                 select new VmTeamSafetyItemDetail
                                                 {
                                                     Id = t.Id,
                                                     Name= t.Name,
                                                     TeamId = t.TeamId,
                                                     Value = t.Value,
                                                     Comment = t.Comment ?? "",
                                                     SafetyItemDetailId = t.SafetyItemDetailId,
                                                 };

                return vmTeamSafetyItemDetailList.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateTeamSafetyItem(VmTeamSafetyItem vmTeamSafetyItem)
        {
            try
            {
                var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();

                var oldTeamSafety = teamSafetyItemRepository.GetTeamSubSafetyItem(vmTeamSafetyItem.TeamId, vmTeamSafetyItem.SubSafetyItemId);

                if (oldTeamSafety != null)
                {
                    teamSafetyItemRepository.UpdateTeamSafetyItem(
                       new TeamSafetyItem
                       {
                           Id = oldTeamSafety.Id,
                           TeamId = vmTeamSafetyItem.TeamId,
                           SafetyItemId = oldTeamSafety.SafetyItemId,
                           SubSafetyItemId = oldTeamSafety.SubSafetyItemId,
                           LastContent = vmTeamSafetyItem.LastContent ?? "",
                           LastComment = vmTeamSafetyItem.LastComment ?? "",
                           ItemStatus = vmTeamSafetyItem.ItemStatus,
                           AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,

                       });

                    var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();
                    teamSafetyItemLogRepository.CreateTeamSafetyItemLog(
                                new TeamSafetyItemLog
                                {
                                    TeamSafetyItemId = oldTeamSafety.Id,
                                    AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,
                                    Content = (vmTeamSafetyItem.Type == false) ? vmTeamSafetyItem.LastContent ?? "" : vmTeamSafetyItem.LastComment ?? "",
                                    DateTime = DateTime.Now,
                                    Type = vmTeamSafetyItem.Type,
                                    UserId = vmTeamSafetyItem.UserId,
                                });
                }
                else
                {
                    teamSafetyItemRepository.CreateTeamSafetyItem(
                        new TeamSafetyItem
                        {
                            TeamId = vmTeamSafetyItem.TeamId,
                            SafetyItemId = vmTeamSafetyItem.SafetyItemId,
                            SubSafetyItemId = vmTeamSafetyItem.SubSafetyItemId,
                            LastContent = vmTeamSafetyItem.LastContent ?? "",
                            LastComment = vmTeamSafetyItem.LastComment ?? "",
                            ItemStatus = vmTeamSafetyItem.ItemStatus,
                            AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,

                            TeamSafetyItemLogs = new List<TeamSafetyItemLog>()
                            {
                                new TeamSafetyItemLog{
                                     AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,
                                     Content = (vmTeamSafetyItem.Type == false)? vmTeamSafetyItem.LastContent ?? "" : vmTeamSafetyItem.LastComment ?? "",
                                     DateTime = DateTime.Now,
                                     Type = vmTeamSafetyItem.Type,
                                     UserId = vmTeamSafetyItem.UserId,
                                }
                            }
                        });
                }

                if (!string.IsNullOrWhiteSpace(vmTeamSafetyItem.SafetyItemDetailIds))
                {
                    var blSafetyItem = new BLSafetyItem();
                    var safetyItemDetails = blSafetyItem.GetSafetyItemDetails(vmTeamSafetyItem.TeamId, vmTeamSafetyItem.SafetyItemId);

                    var newTeamSafetyItemDetails = (from s in safetyItemDetails
                                                    select new TeamSafetyItemDetail
                                                    {
                                                        Name = s.Name,
                                                        SafetyItemDetailId = s.Id,
                                                        SafetyItemId = s.SafetyItemId,
                                                        TeamId = vmTeamSafetyItem.TeamId,
                                                        Comment = s.Comment,
                                                    }).ToList();


                    var teamSafetyItemDetailRepository = UnitOfWork.GetRepository<TeamSafetyItemDetailRepository>();

                    var teamSafetyItemDetailList = teamSafetyItemDetailRepository.GetTeamSafetyItemDetails(vmTeamSafetyItem.TeamId, vmTeamSafetyItem.SafetyItemId);

                    if (teamSafetyItemDetailList != null && teamSafetyItemDetailList.Count > 0)
                    {
                        teamSafetyItemDetailRepository.DeleteTeamSafetyItemDetails(vmTeamSafetyItem.TeamId, vmTeamSafetyItem.SafetyItemId);
                    }

                    var safetyItemDetailIds = vmTeamSafetyItem.SafetyItemDetailIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var safetyItemDetailId in safetyItemDetailIds)
                    {
                        newTeamSafetyItemDetails.FirstOrDefault(t => t.SafetyItemDetailId == int.Parse(safetyItemDetailId)).Value = true;

                    }

                    teamSafetyItemDetailRepository.CreateBatchTeamSafetyItemDetail(newTeamSafetyItemDetails);

                }

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckSavedTeamSafety(int teamId)
        {
            var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();

            var teamSafetyItemList = teamSafetyItemRepository.GetTeamSafetyItem(teamId);

            var subSafetyItemRepository = UnitOfWork.GetRepository<SubSafetyItemRepository>();

            var subSafetyItemCount = subSafetyItemRepository.GetSubSafetyItemsCount();

            if (subSafetyItemCount != teamSafetyItemList.Count())
            {
                return false;
            }

            foreach (var item in teamSafetyItemList)
            {
                if (item.ItemStatus == null || item.ItemStatus == 2)
                {
                    return false;
                }
            }


            return true;
        }
        public IEnumerable<VmTeamSafetyItem> GetApproveAllTeamSafetyIteam(int teamId)
        {
            var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();

            var teamSafetyItemList = teamSafetyItemRepository.GetTeamSafetyItem(teamId);

            var vmTeamSafetyItemList = from t in teamSafetyItemList
                                       select new VmTeamSafetyItem
                                       {
                                           Id = t.Id,
                                           TeamId = t.TeamId,
                                           LastContent = t.LastContent ?? "",
                                           LastComment = t.LastComment ?? "",
                                           ItemStatus = t.ItemStatus,
                                           AttachedFileUrl = t.AttachedFileUrl,
                                           SafetyItemId = t.SafetyItemId,
                                           SubSafetyItemId = t.SubSafetyItemId.Value,
                                       };


            return vmTeamSafetyItemList;
        }
        public bool UpdateTeamSafetyItemStatus(int teamId, int temStatus)
        {
            var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();
            teamSafetyItemRepository.UpdateTeamSafetyItem(teamId, temStatus);

            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
            teamRepository.UpdateTeamSubmitStatus(teamId, true);

            return UnitOfWork.Commit();
        }
        public bool UpdateSubmitTeamSafetyItemStatus(int teamId, int temStatus)
        {
            var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();
            teamSafetyItemRepository.UpdateSubmitTeamSafetyItemStatus(teamId, temStatus);

            var teamRepository = UnitOfWork.GetRepository<TeamRepository>();
            teamRepository.UpdateTeamSubmitStatus(teamId, true);

            return UnitOfWork.Commit();
        }
        public bool UpdateTeamSafetyItemStatusAndComment(VmTeamSafetyItem vmTeamSafetyItem)
        {
            try
            {
                var teamSafetyItemRepository = UnitOfWork.GetRepository<TeamSafetyItemRepository>();

                var oldTeamSafety = teamSafetyItemRepository.GetTeamSubSafetyItem(vmTeamSafetyItem.TeamId, vmTeamSafetyItem.SubSafetyItemId);
                if (oldTeamSafety != null)
                {
                    teamSafetyItemRepository.UpdateTeamSafetyItem(
                       new TeamSafetyItem
                       {
                           Id = oldTeamSafety.Id,
                           TeamId = vmTeamSafetyItem.TeamId,
                           SafetyItemId = oldTeamSafety.SafetyItemId,
                           SubSafetyItemId = oldTeamSafety.SubSafetyItemId,
                           LastContent = vmTeamSafetyItem.LastContent ?? "",
                           LastComment = vmTeamSafetyItem.LastComment ?? "",
                           ItemStatus = vmTeamSafetyItem.ItemStatus,
                           AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,

                       });

                    var teamSafetyItemLogRepository = UnitOfWork.GetRepository<TeamSafetyItemLogRepository>();
                    teamSafetyItemLogRepository.CreateTeamSafetyItemLog(
                                new TeamSafetyItemLog
                                {
                                    TeamSafetyItemId = oldTeamSafety.Id,
                                    AttachedFileUrl = vmTeamSafetyItem.AttachedFileUrl,
                                    Content = (vmTeamSafetyItem.Type == false) ? vmTeamSafetyItem.LastContent ?? "" : vmTeamSafetyItem.LastComment ?? "",
                                    DateTime = DateTime.Now,
                                    Type = vmTeamSafetyItem.Type,
                                    UserId = vmTeamSafetyItem.UserId,

                                });
                }

                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}