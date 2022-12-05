using Model;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamRepository : EFBaseRepository<ViewTeam>, IModelPaged<ViewTeam>
    {
        public IEnumerable<ViewTeam> EntityList { get; set; }
        public int Count(Func<ViewTeam, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTeam> Select(int index = 0, int count = int.MaxValue)
        {
            var teamList = from team in Context.ViewTeams
                           select team;

            return teamList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeam> Select(Func<ViewTeam, bool> predicate, int index, int count)
        {
            var teamList = (from team in Context.ViewTeams
                            select team).Where(predicate);

            return teamList.Skip(index).Take(count).ToArray();
        }
        public ViewTeam GetTeamById(int id)
        {
            var team = Context.ViewTeams.SingleOrDefault(a => a.Id == id);

            return team;
        }
        public IEnumerable<ViewTeam> GetTeams(string name = "")
        {

            var teamList = from team in Context.ViewTeams
                           select team;
            if (name != null)
            {
                teamList = teamList.Where(a => a.Name.Contains(name));
            }

            return teamList.ToArray();
        }
        public IEnumerable<ViewTeam> GetMemberUserTeams(string userId, string name)
        {
            var viewTeamList = from eal in Context.ViewTeams
                               where eal.MemberUserId == userId && eal.Name.Contains(name)
                               select eal;

            return viewTeamList.ToArray();
        }
        public IEnumerable<ViewTeam> GetMemberUserTeams(string memberUserId)
        {

            var teamList = from team in Context.ViewTeams
                           where team.MemberUserId == memberUserId
                           select team;


            return teamList.ToArray();
        }
        public bool HasTeam(string userId, string roleId)
        {

            var teamList = from team in Context.ViewTeams
                           where team.MemberUserId == userId && team.RoleId == roleId
                           select team;

            return (teamList.Count() > 0);
        }
        public IEnumerable<ViewTeam> GetTeams(int[] teamIds)
        {
            return (from a in Context.ViewTeams where teamIds.Contains(a.Id) select a).ToArray();
        }
    }
}
