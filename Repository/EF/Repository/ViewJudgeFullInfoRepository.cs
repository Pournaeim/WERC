using Model;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewJudgeFullInfoRepository : EFBaseRepository<ViewJudgeFullInfo>, IModelPaged<ViewJudgeFullInfo>
    {
        public IEnumerable<ViewJudgeFullInfo> EntityList { get; set; }
        public int Count(Func<ViewJudgeFullInfo, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewJudgeFullInfo> Select(int index = 0, int count = int.MaxValue)
        {
            var judgeFullInfoList = from judgeFullInfo in Context.ViewJudgeFullInfoes
                                    select judgeFullInfo;

            return judgeFullInfoList.OrderBy(A => A.FirstName).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewJudgeFullInfo> Select(Func<ViewJudgeFullInfo, bool> predicate, int index, int count)
        {
            var judgeFullInfoList = (from judgeFullInfo in Context.ViewJudgeFullInfoes
                                     select judgeFullInfo).Where(predicate);

            return judgeFullInfoList.Skip(index).Take(count).ToArray();
        }
        public ViewJudgeFullInfo GetJudgeFullInfoById(int id)
        {
            var judgeFullInfo = Context.ViewJudgeFullInfoes.SingleOrDefault(a => a.Id == id);

            return judgeFullInfo;
        }

        public IEnumerable<ViewJudgeFullInfo> GetMemberUserJudgeFullInfos(string memberUserId)
        {

            var judgeFullInfoList = from judgeFullInfo in Context.ViewJudgeFullInfoes
                                    where judgeFullInfo.UserId == memberUserId
                                    select judgeFullInfo;


            return judgeFullInfoList.ToArray();
        }
        public IEnumerable<ViewJudgeFullInfo> GetMemberUserJudgeFullInfos(string[] userIds)
        {

            var judgeFullInfoList = from judgeFullInfo in Context.ViewJudgeFullInfoes
                                    where userIds.Contains(judgeFullInfo.UserId)
                                    select judgeFullInfo;


            return judgeFullInfoList.ToArray();
        }
        public IEnumerable<ViewJudgeFullInfo> GetJudgeFullInfos(int[] judgeFullInfoIds)
        {
            return (from a in Context.ViewJudgeFullInfoes where judgeFullInfoIds.Contains(a.Id) select a).ToArray();
        }

        public IEnumerable<ViewJudgeFullInfo> Select(ViewJudgeFullInfo filterItem, int index, int count)
        {
            var judgeFullInfoList = from judgeFullInfo in Context.ViewJudgeFullInfoes
                                    select judgeFullInfo;

            if (filterItem.FirstName != null)
            {
                judgeFullInfoList = judgeFullInfoList.Where(t => t.FirstName.Contains(filterItem.FirstName));
            }
            if (filterItem.LastName != null)
            {
                judgeFullInfoList = judgeFullInfoList.Where(t => t.LastName.Contains(filterItem.LastName));
            }

            if (filterItem.Email != null)
            {
                judgeFullInfoList = judgeFullInfoList.Where(t => t.Email.Contains(filterItem.Email));
            }

            if (filterItem.Tasks != null)
            {
                judgeFullInfoList = judgeFullInfoList.Where(t => t.Tasks.Contains(filterItem.Tasks));
            }

            if (filterItem.Teams != null)
            {
                judgeFullInfoList = judgeFullInfoList.Where(t => t.Teams.Contains(filterItem.Teams));
            }


            return judgeFullInfoList.OrderBy(t => t.LastName).Skip(index).Take(count).ToArray();

        }

    }
}
