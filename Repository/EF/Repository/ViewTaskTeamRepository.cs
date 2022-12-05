using Model;

using Repository.Core;
using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTaskTeamRepository : EFBaseRepository<ViewTaskTeam>, IModelPaged<ViewTaskTeam>
    {
        public IEnumerable<ViewTaskTeam> EntityList { get; set; }
        public int Count(Func<ViewTaskTeam, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTaskTeam> Select(int index = 0, int count = int.MaxValue)
        {
            var teamList = from team in Context.ViewTaskTeams
                           select team;

            return teamList.OrderBy(A => A.TaskName).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTaskTeam> Select(Func<ViewTaskTeam, bool> predicate, int index, int count)
        {
            var teamList = (from team in Context.ViewTaskTeams
                            select team).Where(predicate);

            return teamList.Skip(index).Take(count).ToArray();
        }
        public ViewTaskTeam GetTeamById(int id)
        {
            var team = Context.ViewTaskTeams.SingleOrDefault(a => a.Id == id);

            return team;
        }
        public IEnumerable<ViewTaskTeam> GetTeams(string name = "")
        {

            var teamList = from team in Context.ViewTaskTeams
                           select team;
            if (name != null)
            {
                teamList = teamList.Where(a => a.TeamName.Contains(name));
            }

            return teamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetMemberUserTeams(string userId, string name)
        {
            var viewTeamList = from eal in Context.ViewTaskTeams
                               where eal.UserId == userId && eal.TeamName.Contains(name)
                               select eal;

            return viewTeamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetMemberUserTeamsByTaskConfirm(string userId, bool confirm, string name = "")
        {
            var viewTeamList = from eal in Context.ViewTaskTeams
                               where eal.UserId == userId && eal.TeamName.Contains(name) && eal.Confirmed == confirm
                               select eal;

            return viewTeamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetTaskTeamByJudgesAndTeam(string[] userIds, int teamId)
        {
            var viewTeamList = from eal in Context.ViewTaskTeams
                               where userIds.Contains(eal.UserId) && eal.TeamId == teamId
                               select eal;

            return viewTeamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetTaskTeamByJudgesAndteamIds(string[] userIds, int[] teamIds)
        {
            var viewTeamList = from eal in Context.ViewTaskTeams
                               where userIds.Contains(eal.UserId) && teamIds.Contains(eal.TeamId)
                               select eal;

            return viewTeamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetMemberUserTeams(string memberUserId)
        {

            var teamList = from team in Context.ViewTaskTeams
                           where team.UserId == memberUserId
                           select team;


            return teamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetMemberUserTeamsByTaskConfirm(string memberUserId, bool confirm)
        {

            var teamList = from team in Context.ViewTaskTeams
                           where team.UserId == memberUserId &&
                           team.Confirmed == confirm
                           select team;


            return teamList.ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetTaskTeamByJudge(string judgeUserId)
        {

            var teamList = from team in Context.ViewTaskTeams
                           where team.UserId == judgeUserId
                           select team;


            return teamList.OrderBy(t => t.TaskName).ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetTaskTeamByJudgeIds(string[] judgeUserIds)
        {

            var teamList = from team in Context.ViewTaskTeams
                           where judgeUserIds.Contains(team.UserId)
                           select team;


            return teamList.OrderBy(t => t.TaskName).ToArray();
        }
        public IEnumerable<ViewTaskTeam> GetTeams(int[] teamIds)
        {
            return (from a in Context.ViewTaskTeams where teamIds.Contains(a.Id) select a).ToArray();
        }
        public IEnumerable<int> GetTeamIdsByTasks(int[] taskIds)
        {
            return (from a in Context.ViewTaskTeams where taskIds.Contains(a.TaskId) select a.TeamId).ToArray();
        }
    }
}
