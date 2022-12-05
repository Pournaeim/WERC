using Model;
using Model.ViewModels.Team;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewTeamMemberRepository : EFBaseRepository<ViewTeamMember>, IModelPaged<ViewTeamMember>
    {
        public IEnumerable<ViewTeamMember> EntityList { get; set; }
        public int Count(Func<ViewTeamMember, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTeamMember> Select(int index = 0, int count = int.MaxValue)
        {
            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 select teamMember;

            return teamMemberList.OrderBy(A => A.TeamName).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamMember> Select(Func<ViewTeamMember, bool> predicate, int index, int count)
        {
            var teamMemberList = (from teamMember in Context.ViewTeamMembers
                                  select teamMember).Where(predicate);

            return teamMemberList.Skip(index).Take(count).ToArray();
        }
        public ViewTeamMember GetTeamMemberById(int id)
        {
            var teamMember = Context.ViewTeamMembers.SingleOrDefault(a => a.Id == id);

            return teamMember;
        }
        public IEnumerable<ViewTeamMember> GetMemberTeam(string name, string memberUserId)
        {
            var viewTeamMemberList = from eal in Context.ViewTeamMembers
                                     where eal.MemberUserId == memberUserId && eal.TeamName.Contains(name)
                                     select eal;

            return viewTeamMemberList.ToArray();
        }
        public IEnumerable<ViewTeamMember> GetTeamMembersByRoles(int teamId, string[] roleList)
        {
            var viewTeamMemberList = from eal in Context.ViewTeamMembers
                                     where
                                     eal.TeamId == teamId &&
                                     roleList.Contains(eal.RoleName)
                                     select eal;

            return viewTeamMemberList.ToArray();
        }
        public IEnumerable<ViewTeamMember> GetTeamMembersByRoleNameAndTeam(int teamId, string roleName)
        {

            var viewTeamMemberList = from eal in Context.ViewTeamMembers
                                     where
                                     eal.TeamId == teamId &&
                                     roleName == eal.RoleName
                                     select eal;

            return viewTeamMemberList.ToArray();
        }

        public ViewTeamMember GetTeamMember(string memberUserId)
        {

            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.MemberUserId == memberUserId
                                 select teamMember;


            return teamMemberList.FirstOrDefault();
        }
        public IEnumerable<ViewTeamMember> GetTeamMembers(string[] memberUserIds)
        {

            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where memberUserIds.Contains(teamMember.MemberUserId)
                                 select teamMember;


            return teamMemberList.ToArray();
        }
        public ViewTeamMember GetTeamMemberByTeamId(string memberUserId, int teamId)
        {

            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.MemberUserId == memberUserId && teamMember.TeamId == teamId
                                 select teamMember;


            return teamMemberList.FirstOrDefault();
        }
        public ViewTeamMember GetTeamMemberByTask(string memberUserId, int taskId)
        {

            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.MemberUserId == memberUserId && teamMember.TaskId == taskId
                                 select teamMember;


            return teamMemberList.FirstOrDefault();
        }
        public ViewTeamMember GetTeamMember(string memberUserId, int teamId)
        {

            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where

                                 teamMember.MemberUserId == memberUserId &&
                                 teamMember.TeamId == teamId

                                 select teamMember;


            return teamMemberList.FirstOrDefault();
        }
        public IEnumerable<ViewTeamMember> GetMembersTeams(int[] teamMemberIds)
        {
            return (from a in Context.ViewTeamMembers where teamMemberIds.Contains(a.Id) select a).ToArray();
        }
        public IEnumerable<ViewTeamMember> GetTeamMembers(int teamId)
        {
            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.TeamId == teamId
                                 select teamMember;

            return teamMemberList.ToArray();
        }


        public string[] GetTeamMembersUserIds(int teamId)
        {
            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.TeamId == teamId
                                 select teamMember.MemberUserId;

            return teamMemberList.ToArray();
        }
        public string[] GetTeamMembersEmails(int teamId)
        {
            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.TeamId == teamId
                                 select teamMember.Email;

            return teamMemberList.ToArray();
        }
        public IEnumerable<VmRawTeamMemberUserId> GetAllTeamMembersUserIds()
        {
            var teamMemberList = from m in Context.ViewTeamMembers

                                 select new VmRawTeamMemberUserId
                                 {
                                     TeamId = m.TeamId,
                                     MemberUserId = m.MemberUserId
                                 };


            return teamMemberList.ToArray();
        }
        public IEnumerable<ViewTeamMember> GetAllTeamMembers()
        {
           return Context.ViewTeamMembers.ToArray();
           
        }
        public int GetTeamMembersCount(int teamId)
        {

            return Context.ViewTeamMembers.AsNoTracking().Count(tm => tm.TeamId == teamId);
        }
        public IEnumerable<ViewTeamMember> Select(ViewTeamMember filterItem, int index, int count, int teamId)
        {
            var teamMemberList = from teamMember in Context.ViewTeamMembers
                                 where teamMember.TeamId == teamId
                                 select teamMember;

            if (filterItem.Id != 0)
            {
                teamMemberList = teamMemberList.Where(j => filterItem.Id == j.Id);
            }

            if (filterItem.MemberName != null)
            {
                teamMemberList = teamMemberList.Where(t => t.MemberName.Contains(filterItem.MemberName));
            }

            if (filterItem.Email != null)
            {
                teamMemberList = teamMemberList.Where(t => t.Email.Contains(filterItem.Email));
            }

            if (filterItem.Survey != null)
            {
                teamMemberList = teamMemberList.Where(t => t.Survey == filterItem.Survey);
            }

            return teamMemberList.OrderBy(t => t.MemberName).Skip(index).Take(count).ToArray();

        }
        public bool CheckOtherTeamLeaderIsExist(int teamId, string teamMemberUserId)
        {

            return Context.ViewTeamMembers.Any(t => t.TeamId == teamId && t.MemberUserId != teamMemberUserId && t.RoleName == SystemRoles.Leader.ToString());
        }
        public bool CheckOtherTeamLeaderIsExist(int teamId)
        {

            return Context.ViewTeamMembers.Any(t => t.TeamId == teamId && t.RoleName == SystemRoles.Leader.ToString());
        }
    }
}
