using Model;
using Model.ViewModels.Test;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamTestResultRepository : EFBaseRepository<TeamTestResult>
    {
        public void CreateTeamTestResult(TeamTestResult teamTestResult)
        {
            Add(teamTestResult);
        }
        public IEnumerable<TeamTestResult> GetAllTeamTestResults()
        {
            return Context.TeamTestResults.ToArray();
        }
        public IEnumerable<TeamTestResult> GetTeamTestResults(int taskId)
        {
            return (from s in Context.TeamTestResults where s.TaskId == taskId select s).ToArray();
        }
        public IEnumerable<TeamTestResult> GetTeamTestResults(string userId, int taskId)
        {
            return (from s in Context.TeamTestResults
                    where
                    s.TaskId == taskId && s.UserId == userId
                    select s).ToArray();
        }
        public int GetTeamTestResultsByUser(string userId, int[] taskIds)
        {
            return (from s in Context.TeamTestResults
                    where
                    s.UserId == userId && taskIds.Contains(s.TaskId)
                    select s).Count();
        }
        public IEnumerable<TeamTestResult> GetTeamTestResultsByTeam(int teamId)
        {
            return (from s in Context.TeamTestResults
                    where
                    s.TeamId == teamId
                    select s).ToArray();
        }
        public int[] GetTeamTestResultIds(int teamId)
        {
            return (from s in Context.TeamTestResults where s.TeamId == teamId select s.Id).ToArray();
        }
        public void DeleteTeamTestResult(string userId, int taskId)
        {
            var deletableTeamTestResults = from s in Context.TeamTestResults
                                           where s.UserId == userId && s.TaskId == taskId
                                           select s;

            foreach (var item in deletableTeamTestResults)
            {
                Delete(item);
            }

        }
        public void CreateTeamTestResult(string labUserId, VmTeamTestResult[] teamTestResult)
        {
            foreach (var item in teamTestResult)
            {
                Add(new TeamTestResult
                {
                    TeamId = item.TeamId,
                    UserId = labUserId,
                    TaskId = item.TaskId,
                    TestId = item.TestId,
                    Score = item.Score ?? ""
                });
            }
        }
    }
}
