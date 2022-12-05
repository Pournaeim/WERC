using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamGradeDetailRepository : EFBaseRepository<TeamGradeDetail>
    {
        public void CreateTeamGradeDetail(List<TeamGradeDetail> TeamGradeDetailList)
        {
            foreach (var item in TeamGradeDetailList)
            {
                Add(item);
            }

        }
        public IEnumerable<TeamGradeDetail> GetTeamGradeDetails(int teamId)
        {
            return (from s in Context.TeamGradeDetails where s.TeamId == teamId select s).ToArray();

        }
        public List<TeamGradeDetail> GetTeamGradeDetailsByGradeDetailId(int id)
        {
            return (from s in Context.TeamGradeDetails where s.GradeDetailId == id select s).ToList();

        }

        public int GetTeamJudgeResultsByUser(string userId, int[] teamIds)
        {
            return (from s in Context.TeamGradeDetails
                    where
                    s.JudgeUserId == userId && teamIds.Contains(s.TeamId)
                    select s).Count();
        }
        public void DeleteDetailsGrade(int id)
        {
            var deletableTeamGradeDetail = Context.TeamGradeDetails.Find(id);

            Delete(deletableTeamGradeDetail);

        }
        public void DeleteTeamGradeDetailsByTeam(int teamId)
        {
            var deleteList = from s in Context.TeamGradeDetails where s.TeamId == teamId select s;
            foreach (var item in deleteList)
            {
                Delete(item);
            }

        }
        public void DeleteTeamGradeDetailsByJudgeAndTeam(string judgeUserId, IEnumerable<int> teamIds)
        {
            var deleteList = from s in Context.TeamGradeDetails where s.JudgeUserId == judgeUserId && teamIds.Contains(s.TeamId) select s;
            foreach (var item in deleteList)
            {
                Delete(item);
            }

        }

        public void DeleteTeamGradeDetailsByJudgeAndTeam(string judgeUserId, IEnumerable<int> gradeDetailIds, int teamId)
        {
            var deleteList = from s in Context.TeamGradeDetails
                             where
                                s.JudgeUserId == judgeUserId &&
                                gradeDetailIds.Contains(s.GradeDetailId) &&
                                s.TeamId == teamId
                             select s;

            foreach (var item in deleteList)
            {
                Delete(item);
            }

        }

    }
}
