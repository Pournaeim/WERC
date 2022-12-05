using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TeamSafetyItemLogRepository : EFBaseRepository<TeamSafetyItemLog>
    {
        public void CreateTeamSafetyItemLog(TeamSafetyItemLog TeamSafetyItemLog)
        {
            Add(TeamSafetyItemLog);
        }

        public TeamSafetyItemLog GetTeamSafetyItemLogById(int id)
        {
            var teamSafetyItemLog = Context.TeamSafetyItemLogs.SingleOrDefault(a => a.Id == id);

            return teamSafetyItemLog;
        }
        public IEnumerable<TeamSafetyItemLog> GetLastTeamSafetyItemLog(int teamSafetyItemId)
        {

            var teamSafetyItemLog = from li in Context.TeamSafetyItemLogs
                                    where li.TeamSafetyItemId == teamSafetyItemId
                                    group li.DateTime
                                           by new
                                           {
                                               li.Id,
                                               li.UserId,
                                               li.TeamSafetyItemId,
                                               li.Content,
                                               li.AttachedFileUrl,
                                               li.Type,

                                           } into g

                                    select new TeamSafetyItemLog
                                    {
                                        Id = g.Key.Id,
                                        UserId = g.Key.UserId,
                                        TeamSafetyItemId = g.Key.TeamSafetyItemId,
                                        Content = g.Key.Content,
                                        AttachedFileUrl = g.Key.AttachedFileUrl,
                                        DateTime = g.Max(),
                                        Type = g.Key.Type,
                                    };

            return teamSafetyItemLog;
        }
        public IEnumerable<TeamSafetyItemLog> GetLastTeamSafetyItemLogList(int[] teamSafetyItemIds)
        {
            var teamSafetyItemLogList = Context.TeamSafetyItemLogs.Where(a => teamSafetyItemIds.Contains(a.TeamSafetyItemId));
            var teamSafetyItemLog = from li in teamSafetyItemLogList
                                    group li.DateTime
                                    by new
                                    {
                                        li.Id,
                                        li.UserId,
                                        li.TeamSafetyItemId,
                                        li.Content,
                                        li.AttachedFileUrl,
                                        li.Type,

                                    } into g

                                    select new TeamSafetyItemLog
                                    {
                                        Id = g.Key.Id,
                                        UserId = g.Key.UserId,
                                        TeamSafetyItemId = g.Key.TeamSafetyItemId,
                                        Content = g.Key.Content,
                                        AttachedFileUrl = g.Key.AttachedFileUrl,
                                        DateTime = g.Max(),
                                        Type = g.Key.Type,
                                    };

            return teamSafetyItemLog;
        }
        public IEnumerable<ViewTeamSafetyItemLog> GetTeamSafetyItemLog(int subSafetyItemId, int teamId, bool type)
        {
            var teamSafetyItemLogList = Context.ViewTeamSafetyItemLogs.Where(
                a => 
                a.SubSafetyItemId == subSafetyItemId && 
                a.TeamId== teamId && 
                a.Type == type);

            return teamSafetyItemLogList.OrderByDescending(s=>s.DateTime);
        }
        public void UpdateTeamSafetyItemLog(TeamSafetyItemLog teamSafetyItemLog)
        {
            var oldTeamSafetyItemLog = Context.TeamSafetyItemLogs.Find(teamSafetyItemLog.Id);

            oldTeamSafetyItemLog.Id = teamSafetyItemLog.Id;
            oldTeamSafetyItemLog.UserId = oldTeamSafetyItemLog.UserId;
            oldTeamSafetyItemLog.TeamSafetyItemId = teamSafetyItemLog.TeamSafetyItemId;
            oldTeamSafetyItemLog.Content = teamSafetyItemLog.Content;
            oldTeamSafetyItemLog.AttachedFileUrl = teamSafetyItemLog.AttachedFileUrl;
            oldTeamSafetyItemLog.DateTime = oldTeamSafetyItemLog.DateTime;
            oldTeamSafetyItemLog.Type = teamSafetyItemLog.Type;

            Update(oldTeamSafetyItemLog);
        }
    }
}
