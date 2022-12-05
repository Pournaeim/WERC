using Model;
using Model.ViewModels.Team;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamFullInfoRepository : EFBaseRepository<ViewTeamFullInfo>, IModelPaged<ViewTeamFullInfo>
    {
        public IEnumerable<ViewTeamFullInfo> EntityList { get; set; }
        public int Count(Func<ViewTeamFullInfo, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTeamFullInfo> Select(int index = 0, int count = int.MaxValue)
        {
            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   select teamFullInfo;

            return teamFullInfoList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamFullInfo> Select(Func<ViewTeamFullInfo, bool> predicate, int index, int count)
        {
            var teamFullInfoList = (from teamFullInfo in Context.ViewTeamFullInfoes
                                    select teamFullInfo).Where(predicate);

            return teamFullInfoList.Skip(index).Take(count).ToArray();
        }
        public ViewTeamFullInfo GetTeamFullInfoById(int id)
        {
            var teamFullInfo = Context.ViewTeamFullInfoes.SingleOrDefault(a => a.Id == id);

            return teamFullInfo;
        }
        public IEnumerable<ViewTeamFullInfo> GetTeamFullInfos(string name = "")
        {

            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   select teamFullInfo;
            if (name != null)
            {
                teamFullInfoList = teamFullInfoList.Where(a => a.Name.Contains(name));
            }

            return teamFullInfoList.ToArray();
        }
        public IEnumerable<ViewTeamFullInfo> GetAdvisorTeamFullInfos(string userId, string name)
        {
            var viewTeamFullInfoList = from eal in Context.ViewTeamFullInfoes
                                       where eal.MemberUserId == userId && eal.Name.Contains(name)
                                       select eal;

            return viewTeamFullInfoList.ToArray();
        }
        public IEnumerable<ViewTeamFullInfo> GetMemberUserTeamFullInfos(string memberUserId)
        {

            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   where teamFullInfo.MemberUserId == memberUserId
                                   select teamFullInfo;


            return teamFullInfoList.ToArray();
        }
        public IEnumerable<ViewTeamFullInfo> GetMemberUserTeamFullInfos(string[] memberUserIds)
        {

            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   where memberUserIds.Contains(teamFullInfo.MemberUserId)
                                   select teamFullInfo;


            return teamFullInfoList.ToArray();
        }
        public IEnumerable<ViewTeamFullInfo> GetTeamFullInfos(int[] teamFullInfoIds)
        {
            return (from a in Context.ViewTeamFullInfoes where teamFullInfoIds.Contains(a.Id) select a).ToArray();
        }

        public IEnumerable<ViewTeamFullInfo> Select(VmTeamFullInfo filterItem, int index, int count)
        {
            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   select teamFullInfo;

            if (filterItem.Name != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Name.ToLower().Contains(filterItem.Name.ToLower()));
            }

            if (filterItem.TaskName != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.TaskName.ToLower().Contains(filterItem.TaskName.ToLower()));
            }

            if (filterItem.Leader != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Leader.ToLower().Contains(filterItem.Leader.ToLower()));
            }

            if (filterItem.Advisor != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Advisor.ToLower().Contains(filterItem.Advisor.ToLower()));
            }

            if (filterItem.Judges != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Judges.ToLower().ToLower().Contains(filterItem.Judges));
            }

            if (filterItem.RegistrationStatus != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.RegistrationStatus == filterItem.RegistrationStatus);
            }
            if (filterItem.PayStatus != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.PayStatus == filterItem.PayStatus);
            }
            

            //teamFullInfoList = teamFullInfoList.Where(t => t.PayStatus == filterItem.PayStatus);

            return teamFullInfoList.OrderBy(t => t.Name).Skip(index).Take(count).ToArray();

        }

        public IEnumerable<ViewTeamFullInfo> SelectByAdvisor(string advisorUserId, VmTeamFullInfo filterItem, int index, int count)
        {
            var teamFullInfoList = from teamFullInfo in Context.ViewTeamFullInfoes
                                   where teamFullInfo.MemberUserId == advisorUserId
                                   select teamFullInfo;

            if (filterItem.Name != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Name.Contains(filterItem.Name));
            }

            if (filterItem.Leader != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Leader.Contains(filterItem.Leader));
            }

            if (filterItem.Advisor != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Advisor.Contains(filterItem.Advisor));
            }

            if (filterItem.Judges != null)
            {
                teamFullInfoList = teamFullInfoList.Where(t => t.Judges.Contains(filterItem.Judges));
            }

            //teamFullInfoList = teamFullInfoList.Where(t => t.PayStatus == filterItem.PayStatus);


            return teamFullInfoList.OrderBy(t => t.Name).Skip(index).Take(count).ToArray();

        }

    }
}
