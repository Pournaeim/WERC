using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamSafetyItemRepository : EFBaseRepository<TeamSafetyItem>
    {
        public void CreateTeamSafetyItem(TeamSafetyItem teamSafetyItem)
        {
            Add(teamSafetyItem);
        }
        public void CreateBatchTeamSafetyItem(List<TeamSafetyItem> teamSafetyItemList)
        {

            foreach (var item in teamSafetyItemList)
            {
                Add(item);
            }
        }
        public void UpdateAttachedFile(int teamSafetyId, string attachedFileUrl)
        {
            var oldTeamSafetyItem = (from s in Context.TeamSafetyItems where s.Id == teamSafetyId select s).FirstOrDefault();

            oldTeamSafetyItem.AttachedFileUrl = attachedFileUrl;

            Update(oldTeamSafetyItem);
        }
        public TeamSafetyItem GetTeamSafetyById(int id)
        {
            var teamSafetyItem = (from s in Context.TeamSafetyItems where s.Id == id select s).FirstOrDefault();
            return teamSafetyItem;


        }
        public List<TeamSafetyItem> GetTeamSafetyItem(int teamId, int safetyItemId)
        {
            var teamSafetyItem = (from s in Context.TeamSafetyItems
                                  where s.SafetyItemId == safetyItemId && s.TeamId == teamId
                                  select s).ToList();
            return teamSafetyItem;


        }
        public TeamSafetyItem GetTeamSubSafetyItem(int teamId, int subSafetyItemId)
        {
            var teamSafetyItem = (from s in Context.TeamSafetyItems
                                  where s.SubSafetyItemId == subSafetyItemId && s.TeamId == teamId
                                  select s).FirstOrDefault();
            return teamSafetyItem;


        }
        public List<TeamSafetyItem> GetTeamSafetyItem(int teamId)
        {
            var teamSafetyItemList = (from s in Context.TeamSafetyItems
                                      where s.TeamId == teamId
                                      select s).ToList();
            return teamSafetyItemList;


        }
        public bool UpdateTeamSafetyItem(int teamId, int itemStatus)
        {
            var teamSafetyItemList = (from s in Context.TeamSafetyItems
                                      where s.TeamId == teamId
                                      select s).ToList();

            foreach (var item in teamSafetyItemList)
            {
                item.ItemStatus = itemStatus;
                Update(item);
            }

            return true;

        }
        public bool UpdateSubmitTeamSafetyItemStatus(int teamId, int itemStatus)
        {
            var teamSafetyItemList = (from s in Context.TeamSafetyItems
                                      where s.TeamId == teamId
                                      select s).ToList();

            foreach (var item in teamSafetyItemList)
            {
                if (item.ItemStatus != 3)
                {
                    item.ItemStatus = itemStatus;
                    Update(item);
                }
            }

            return true;

        }

        public bool ApproveAllTeamSafetyItemStatus(int teamId)
        {
            var teamSafetyItemList = (from s in Context.TeamSafetyItems
                                      where s.TeamId == teamId
                                      select s).ToList();

            foreach (var item in teamSafetyItemList)
            {
                item.ItemStatus = 3;
                Update(item);

            }

            return true;

        }
        public void UpdateTeamSafetyItem(TeamSafetyItem teamSafetyItem)
        {
            var oldTeamSafetyItem = Context.TeamSafetyItems.Find(teamSafetyItem.Id);

            oldTeamSafetyItem.TeamId = teamSafetyItem.TeamId;
            oldTeamSafetyItem.SafetyItemId = teamSafetyItem.SafetyItemId;
            oldTeamSafetyItem.LastContent = teamSafetyItem.LastContent;
            oldTeamSafetyItem.LastComment = teamSafetyItem.LastComment;
            oldTeamSafetyItem.ItemStatus = teamSafetyItem.ItemStatus;
            oldTeamSafetyItem.AttachedFileUrl = teamSafetyItem.AttachedFileUrl;

            Update(oldTeamSafetyItem);
        }
    }
}
