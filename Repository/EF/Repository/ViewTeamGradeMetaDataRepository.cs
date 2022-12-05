using Model;
using Repository.Core;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamGradeMetaDataRepository : EFBaseRepository<ViewTeamGradeMetaData>, IModelPaged<ViewTeamGradeMetaData>
    {
        public IEnumerable<ViewTeamGradeMetaData> EntityList { get; set; }
        public int Count(Func<ViewTeamGradeMetaData, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTeamGradeMetaData> Select(int index, int count)
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                select tg;

            return gradePageList.OrderBy(A => A.EvaluationItem).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> Select(Func<ViewTeamGradeMetaData, bool> predicate, int index, int count)
        {
            var gradePageList = (from tg in Context.ViewTeamGradeMetaDatas
                                 select tg).Where(predicate);

            return gradePageList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetTeamGradeMetaDatasByName(string evaluationItem, bool like)
        {

            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                select tg;

            if (like)
            {
                gradePageList = gradePageList.Where(g => g.EvaluationItem.Contains(evaluationItem));
            }
            else
            {
                gradePageList = gradePageList.Where(g => g.EvaluationItem == evaluationItem);

            }
            return gradePageList.ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetTeamGradeMetaData(int[] taskIds)
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                where taskIds.Contains(tg.TaskId)
                                select tg;

            return gradePageList.ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetTeamGradeMetaData(string currentUserId)
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                where tg.UserId == currentUserId
                                select tg;

            return gradePageList.ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetSingleTeamGradeMetaData(string currentUserId, int gradeId, int teamId)
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                where tg.UserId == currentUserId && tg.Id == teamId
                                 && tg.GradeId == gradeId
                                select tg;

            return gradePageList.ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetTeamGradeMetaData(int teamId)
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                where tg.Id == teamId
                                select tg;


            return gradePageList.ToArray();
        }
        public IEnumerable<ViewTeamGradeMetaData> GetAllTeamGradeMetaDatas()
        {
            var gradePageList = from tg in Context.ViewTeamGradeMetaDatas
                                select tg;

            return gradePageList.OrderBy(t=>t.TeamName).ToArray();
        }
    }
}
