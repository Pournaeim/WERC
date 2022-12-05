using BLL.Base;
using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.SafetyItem;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLSafetyItem : BLBase
    {

        public IEnumerable<VmSelectListItem> GetSafetyItemDetailSelectListItem(int teamId, int safetyItemId)
        {

            var vmSelectListItem = (from cl in GetSafetyItemDetails(teamId, safetyItemId)
                                    select new VmSelectListItem
                                    {
                                        Value = cl.Id.ToString(),
                                        Text = cl.Name,
                                        Selected = cl.Value ?? false,
                                    });



            return vmSelectListItem;
        }
        public IEnumerable<VmSafetyItemDetail> GetSafetyItemDetails(int teamId, int safetyItemId)
        {
            try
            {
                var safetyItemDetailRepository = UnitOfWork.GetRepository<SafetyItemDetailRepository>();

                var safetyItemDetailList = safetyItemDetailRepository.GetSafetyItemDetail(safetyItemId);

                var vmSafetyItemDetailList = (from t in safetyItemDetailList
                                              select new VmSafetyItemDetail
                                              {
                                                  Id = t.Id,
                                                  Value = t.Value,
                                                  Comment = t.Comment ?? "",
                                                  Name = t.Name,
                                                  SafetyItemId = t.SafetyItemId
                                              }).ToList();

                var blTeamSafetyItem = new BLTeamSafetyItem();
                var teamSafetytemDetails = blTeamSafetyItem.GetTeamSafetyItemDetails(teamId, safetyItemId);

                foreach (var item in vmSafetyItemDetailList)
                {
                    var result = teamSafetytemDetails.FirstOrDefault(t => t.SafetyItemDetailId == item.Id);
                    item.Value = (result == null) ? false : result.Value;
                }
                return vmSafetyItemDetailList.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VmSafetyItem GetSafetyItem(int id)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItem = safetyItemRepository.GetSafetyItemById(id);

                var vmSafetyItemList = new VmSafetyItem
                {
                    Id = safetyItem.Id,
                    Name = safetyItem.Name,
                    Instruction = safetyItem.Instruction,
                    Priority = safetyItem.Priority,
                };

                return vmSafetyItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmSafetyItem> GetAllSafetyItems()
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItemList = safetyItemRepository.GetAllSafetyItems();

                var vmSafetyItemList = from si in safetyItemList
                                       select new VmSafetyItem
                                       {
                                           Id = si.Id,
                                           Name = si.Name,
                                           Instruction = si.Instruction,
                                           Priority = si.Priority,
                                           AttachmentRequired = si.AttachmentRequired,
                                           TextRequired = si.TextRequired,
                                           AttachedFileUrl = "",
                                           SubSafetyItems = si.SubSafetyItems.ToList(),
                                           SafetyItemDetails = si.SafetyItemDetails.ToList(),
                                       };

                return vmSafetyItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmSafetyItem> GetAllSafetyItems(int teamId, bool safetyAdmin = false)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItemList = safetyItemRepository.GetAllSafetyItems();

                var vmSafetyItemList = (from si in safetyItemList
                                        select new VmSafetyItem
                                        {
                                            Id = si.Id,
                                            Name = si.Name,
                                            Instruction = si.Instruction,
                                            Priority = si.Priority,
                                            SubSafetyItems = si.SubSafetyItems.ToList(),
                                            SafetyItemDetails = si.SafetyItemDetails.ToList(),
                                            AttachmentRequired = si.AttachmentRequired,
                                            TextRequired = si.TextRequired,
                                            AttachedFileUrl = "",
                                        }).ToList();


                var viewTeamSafetyItemRepository = UnitOfWork.GetRepository<ViewTeamSafetyItemRepository>();

                var teamSafetyItemAnswerList = viewTeamSafetyItemRepository.GetViewTeamSafetyItemByTeamId(teamId);

                if (teamSafetyItemAnswerList.Count() > 0)
                {
                    foreach (var safetyItem in vmSafetyItemList)
                    {
                        var teamSafetyItem = teamSafetyItemAnswerList.Where(s => s.SafetyItemId == safetyItem.Id && !string.IsNullOrWhiteSpace(s.AttachedFileUrl)).FirstOrDefault();
                        if (teamSafetyItem != null)
                        {
                            safetyItem.AttachedFileUrl = teamSafetyItem.AttachedFileUrl;
                        }
                        var teamSubSafetyItems = teamSafetyItemAnswerList.Where(s => s.SafetyItemId == safetyItem.Id);

                        if (teamSubSafetyItems.Count() < safetyItem.SubSafetyItems.Count())
                        {
                            safetyItem.SafetyItemStatus = null;
                        }
                        else
                        if (teamSubSafetyItems.Count(s => s.ItemStatus == 1) > 0 && safetyAdmin == true) // light with back gray (new submision)
                        {
                            safetyItem.SafetyItemStatus = 1;
                        }

                        else if (
                                    teamSubSafetyItems.Count() == safetyItem.SubSafetyItems.Count()
                                    &&
                                    teamSubSafetyItems.Count(s => s.ItemStatus == 2) == 0
                                    &&
                                    teamSubSafetyItems.Count(s => s.ItemStatus == 0) > 0)
                        {
                            safetyItem.SafetyItemStatus = 0;
                        }
                        else if (
                                    teamSubSafetyItems.Count() == safetyItem.SubSafetyItems.Count()
                                    &&
                                    teamSubSafetyItems.Count(s => s.ItemStatus == 2) > 0)
                        {
                            safetyItem.SafetyItemStatus = 2;
                        }
                        else if (safetyItem.SubSafetyItems.Count() == teamSubSafetyItems.Count(s => s.ItemStatus == 3))
                        {
                            safetyItem.SafetyItemStatus = 3;
                        }
                        else if (safetyItem.SubSafetyItems.Count() == teamSubSafetyItems.Count(s => s.ItemStatus == 1 || s.ItemStatus == 3))
                        {
                            safetyItem.SafetyItemStatus = 4;
                        }

                    }
                }


                return vmSafetyItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VmSafetyItem GeTeamParentSafetyItemStatus(int teamId, int safetyItemId, bool safetyAdmin = false)
        {
            try
            {
                int? parentStatus = null;

                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItem = safetyItemRepository.GetSafetyItemById(safetyItemId);

                var viewTeamSafetyItemRepository = UnitOfWork.GetRepository<ViewTeamSafetyItemRepository>();

                var teamSafetyItemAnswers = viewTeamSafetyItemRepository.GetViewTeamSafetyItemByTeamIdAndSafetyItemId(teamId, safetyItemId);

                var teamSafetyItem = teamSafetyItemAnswers.Where(s => s.SafetyItemId == safetyItem.Id && !string.IsNullOrWhiteSpace(s.AttachedFileUrl)).FirstOrDefault();

                var attachedFileUrl = "";

                if (teamSafetyItem != null)
                {
                    attachedFileUrl = teamSafetyItem.AttachedFileUrl;
                }

                if (teamSafetyItemAnswers.Count() < safetyItem.SubSafetyItems.Count())
                {
                    parentStatus = null;
                }
                else
                        if (teamSafetyItemAnswers.Count(s => s.ItemStatus == 1) > 0 && safetyAdmin == true) // light with back gray (new submision)
                {
                    parentStatus = 1;
                }
                else if (
                            safetyItem.SubSafetyItems.Count() == teamSafetyItemAnswers.Count()
                            &&
                            teamSafetyItemAnswers.Count(s => s.ItemStatus == 2) == 0
                            &&
                            teamSafetyItemAnswers.Count(s => s.ItemStatus == 0) > 0

                         )
                {
                    parentStatus = 0;
                }
                else if (
                            teamSafetyItemAnswers.Count() == safetyItem.SubSafetyItems.Count()
                            &&
                            teamSafetyItemAnswers.Count(s => s.ItemStatus == 2) > 0)
                {
                    parentStatus = 2;
                }
                else if (safetyItem.SubSafetyItems.Count() == teamSafetyItemAnswers.Count(s => s.ItemStatus == 3))
                {
                    parentStatus = 3;
                }
                else if (safetyItem.SubSafetyItems.Count() == teamSafetyItemAnswers.Count(s => s.ItemStatus == 1 || s.ItemStatus == 3))
                {
                    parentStatus = 4;
                }
                return new VmSafetyItem
                {
                    Name = safetyItem.Name,
                    SafetyItemStatus = parentStatus,
                    AttachedFileUrl = attachedFileUrl,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool CreateSafetyItem(VmSafetyItem vmSafetyItem)
        {
            try
            {

                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                safetyItemRepository.CreateSafetyItem(
                    new SafetyItem
                    {
                        Name = vmSafetyItem.Name,
                        Instruction = vmSafetyItem.Instruction,
                        Priority = vmSafetyItem.Priority,
                    });

                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateSafetyItem(VmSafetyItem vmSafetyItem)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();

                var safetyItem = new SafetyItem
                {
                    Id = vmSafetyItem.Id,
                    Name = vmSafetyItem.Name,
                    Instruction = vmSafetyItem.Instruction,
                    Priority = vmSafetyItem.Priority,
                };

                safetyItemRepository.UpdateSafetyItem(safetyItem);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteSafetyItem(int id)
        {
            try
            {
                var safetyItemRepository = UnitOfWork.GetRepository<SafetyItemRepository>();
                safetyItemRepository.DeleteSafetyItem(id);

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