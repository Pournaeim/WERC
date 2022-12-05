using Model;

using Repository.Core;
using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamGradeDetailRepository : EFBaseRepository<ViewTeamGradeDetail>, IModelPaged<ViewTeamGradeDetail>
    {
        public IEnumerable<ViewTeamGradeDetail> EntityList { get; set; }
        public int Count(Func<ViewTeamGradeDetail, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTeamGradeDetail> Select(int index, int count)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  select gradeDetail;

            return gradeDetailList.OrderBy(A => A.EvaluationItem).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetGradingEvaluationDetail(int taskId, int teamId, int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where gradeDetail.TaskId == taskId && gradeDetail.TeamId == teamId && gradeDetail.GradeId == gradeId
                                  select gradeDetail;

            return gradeDetailList.OrderBy(A => A.EvaluationItem).ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> Select(int[] taskIds)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId)
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> SelectWithoutGradeId(int[] taskIds, int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId) && gradeDetail.GradeId != gradeId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
         public IEnumerable<ViewTeamGradeDetail> BenchScaleSelect(int[] taskIds, int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId) && gradeDetail.GradeId == gradeId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> SelectByJudge(int[] taskIds, string judgeUserId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId) && gradeDetail.JudgeUserId == judgeUserId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> SelectByJudgeWithoutGradeId(int[] taskIds, string judgeUserId, int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId) && gradeDetail.JudgeUserId == judgeUserId
                                  && gradeDetail.GradeId != gradeId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
          public IEnumerable<ViewTeamGradeDetail> BenchScaleSelectByJudge(int[] taskIds, string judgeUserId, int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where taskIds.Contains(gradeDetail.TaskId) && gradeDetail.JudgeUserId == judgeUserId
                                  && gradeDetail.GradeId == gradeId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }

        public IEnumerable<ViewTeamGradeDetail> Select(Func<ViewTeamGradeDetail, bool> predicate, int index, int count)
        {
            var gradeDetailList = (from gradeDetail in Context.ViewTeamGradeDetails
                                   select gradeDetail).Where(predicate);

            return gradeDetailList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetTeamGradeDetailsByEvaluationItem(string evaluationItem, bool like)
        {

            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  select gradeDetail;

            if (like)
            {
                gradeDetailList = gradeDetailList.Where(g => g.EvaluationItem.Contains(evaluationItem));
            }
            else
            {
                gradeDetailList = gradeDetailList.Where(g => g.EvaluationItem == evaluationItem);

            }
            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> Select(ViewTeamGradeDetail filterItem, int index, int count)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  select gradeDetail;


            if (filterItem.EvaluationItem != null)
            {
                gradeDetailList = gradeDetailList.Where(g => g.EvaluationItem.Contains(filterItem.EvaluationItem));
            }
            if (filterItem.EvaluationItem != null)
            {
                gradeDetailList = gradeDetailList.Where(g => g.EvaluationItem.Contains(filterItem.EvaluationItem));
            }

            if (filterItem.EvaluationItem != null)
            {
                gradeDetailList = gradeDetailList.Where(g => g.EvaluationItem.Contains(filterItem.EvaluationItem));
            }

            if (filterItem.Point != 0)
            {
                gradeDetailList = gradeDetailList.Where(g => g.Point == filterItem.Point);
            }
            if (filterItem.Coefficient != 0)
            {
                gradeDetailList = gradeDetailList.Where(g => g.Coefficient == filterItem.Coefficient);
            }

            return gradeDetailList.OrderBy(g => g.EvaluationItem).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetTeamGradeDetailsByGrade(int gradeId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where gradeDetail.GradeId == gradeId
                                  select gradeDetail;


            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetTeamGradeDetails(int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where gradeDetail.TeamId == teamId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetTeamGradeDetailsBuJudge(string judgeUserId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where gradeDetail.JudgeUserId == judgeUserId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetSingleTeamGradeDetailsBuJudge(string judgeUserId, int gradeId, int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where
                                    gradeDetail.JudgeUserId == judgeUserId &&
                                    gradeDetail.GradeId == gradeId &&
                                    gradeDetail.TeamId == teamId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public IEnumerable<ViewTeamGradeDetail> GetSingleTeamGradeDetailByJudgeAndTeam(string judgeUserId, int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where
                                    gradeDetail.JudgeUserId == judgeUserId &&
                                    gradeDetail.TeamId == teamId
                                  select gradeDetail;

            return gradeDetailList.ToArray();
        }
        public double? GetSingleTeamTotalScoreWithoutGrade(string judgeUserId, int gradeId, int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where
                                    gradeDetail.JudgeUserId == judgeUserId &&
                                    gradeDetail.GradeId != gradeId &&
                                    gradeDetail.TeamId == teamId
                                  select gradeDetail;

            if (gradeDetailList.Count() == 0)
            {
                return 0d;
            }

            return gradeDetailList.Sum(t => t.Point * t.Coefficient);
        }
        public IEnumerable<ViewTeamGradeDetail> GetSingleTeamGradeDetailWithoutJudge(string judgeUserId, int gradeId, int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where
                                    gradeDetail.JudgeUserId != judgeUserId &&
                                    gradeDetail.GradeId == gradeId &&
                                    gradeDetail.TeamId == teamId
                                  select gradeDetail;

            return gradeDetailList.ToList();
        }
        public IEnumerable<ViewTeamGradeDetail> GetSingleTeamGradeDetailByJudge(string judgeUserId, int gradeId, int teamId)
        {
            var gradeDetailList = from gradeDetail in Context.ViewTeamGradeDetails
                                  where
                                    gradeDetail.JudgeUserId == judgeUserId &&
                                    gradeDetail.GradeId == gradeId &&
                                    gradeDetail.TeamId == teamId
                                  select gradeDetail;

            return gradeDetailList.ToList();
        }
        public int GetTeamGradeDetailsCount(int teamId)
        {
            return (from gradeDetail in Context.ViewTeamGradeDetails
                    where gradeDetail.TeamId == teamId
                    select gradeDetail).Count();
        }
        public int GetTeamGradeDetailsCountByJudge(string judgeUserId)
        {
            return (from gradeDetail in Context.ViewTeamGradeDetails
                    where gradeDetail.JudgeUserId == judgeUserId
                    select gradeDetail).Count();
        }
        public int GetSingleTeamGradeDetailsCountByJudge(string judgeUserId, int gradeId, int teamId)
        {
            return (from gradeDetail in Context.ViewTeamGradeDetails
                    where gradeDetail.JudgeUserId == judgeUserId &&
                    gradeDetail.TeamId == teamId &&
                    gradeDetail.GradeId == gradeId
                    select gradeDetail).Count();
        }
        public double? GetTotalScore(string judgeUserId, int teamId)
        {
            var result = Context.ViewTeamGradeDetails.Where(g => g.JudgeUserId == judgeUserId && g.TeamId == teamId).Select(s => s.Point);
            if (result.Count() > 0)
            {
                return result.Sum();
            }
            return 0;
        }
        public int GetTeamGradeDetailsCountByGraeId(int graeId)
        {
            return (from gradeDetail in Context.ViewTeamGradeDetails
                    where gradeDetail.GradeId == graeId
                    select gradeDetail).Count();
        }
    }
}
