using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamSafetyItemDetailRepository : EFBaseRepository<TeamSafetyItemDetail>
    {
        public void CreateTeamSafetyItemDetail(TeamSafetyItemDetail TeamSafetyItemDetail)
        {
            Add(TeamSafetyItemDetail);
        }
        public void CreateBatchTeamSafetyItemDetail(List<TeamSafetyItemDetail> TeamSafetyItemDetailList)
        {

            foreach (var item in TeamSafetyItemDetailList)
            {
                Add(item);
            }
        }
        public TeamSafetyItemDetail GetTeamSafetyById(int id)
        {
            var TeamSafetyItemDetail = (from s in Context.TeamSafetyItemDetails where s.Id == id select s).FirstOrDefault();
            return TeamSafetyItemDetail;


        }
 
        public List<TeamSafetyItemDetail> GetTeamSafetyItemDetails(int teamId, int safetyItemId)
        {
            var TeamSafetyItemDetailList = (from s in Context.TeamSafetyItemDetails
                                      where s.TeamId == teamId && s.SafetyItemId == safetyItemId
                                            select s).ToList();
            return TeamSafetyItemDetailList;


        }
 
        public void UpdateTeamSafetyItemDetail(TeamSafetyItemDetail TeamSafetyItemDetail)
        {
            var oldTeamSafetyItemDetail = Context.TeamSafetyItemDetails.Find(TeamSafetyItemDetail.Id);

            oldTeamSafetyItemDetail.TeamId = TeamSafetyItemDetail.TeamId;
            oldTeamSafetyItemDetail.Comment= TeamSafetyItemDetail.Comment;
            oldTeamSafetyItemDetail.Name= TeamSafetyItemDetail.Name;
            oldTeamSafetyItemDetail.Value= TeamSafetyItemDetail.Value;
            oldTeamSafetyItemDetail.SafetyItemDetailId= TeamSafetyItemDetail.SafetyItemDetailId;

            Update(oldTeamSafetyItemDetail);
        }


        public void DeleteTeamSafetyItemDetails(int teamId, int safetyItemId)
        {
            var teamSafetyItemDetailList = (from s in Context.TeamSafetyItemDetails
                                            where s.TeamId == teamId && s.SafetyItemId == safetyItemId
                                            select s).ToList();
            foreach (var item in teamSafetyItemDetailList)
            {
                Delete(item);
            }
            
        }

    }
}
