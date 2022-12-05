using BLL.Base;

using Model;
using Model.ViewModels.Grade;
using Model.ViewModels.Grade.Grading;
using Model.ViewModels.Grade.Report;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLTeamGradeDetail : BLBase
    {


        public VmSingleGradingType GetSingleTeamGradeMetaData(string currentUserId, int gradeId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();
            IEnumerable<VmClientGrading> vmClientGradingList = null;

            if (viewTeamGradeDetailRepository.GetSingleTeamGradeDetailsCountByJudge(currentUserId, gradeId, teamId) > 0)
            {
                vmClientGradingList = GetSingleTeamGradeDetailByJudge(currentUserId, gradeId, teamId);
            }

            var viewTeamGradeMetaDataRepository = UnitOfWork.GetRepository<ViewTeamGradeMetaDataRepository>();

            var teamGradeMetaData = viewTeamGradeMetaDataRepository.GetSingleTeamGradeMetaData(currentUserId, gradeId, teamId);

            var gradeType = new VmSingleGradingType
            {
                GradeId = teamGradeMetaData.First().GradeId,
                GradeType = teamGradeMetaData.First().Grade,

                TeamGrading = new VmTeamGrading
                {
                    TeamId = teamGradeMetaData.First().Id,
                    TeamName = teamGradeMetaData.First().TeamName,

                }
            };

            gradeType.TeamGrading.GradingDetailList = from tg in teamGradeMetaData
                                                      select new VmGradingDetail
                                                      {
                                                          Id = tg.GradeDetailId,
                                                          GradeId = tg.GradeId,
                                                          EvaluationItem = tg.EvaluationItem,
                                                          Coefficient = tg.Coefficient,

                                                          Point = (vmClientGradingList != null && vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId) != null)
                                                            ? vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId).Point : null,

                                                          MaxPoint = tg.Point,

                                                          Description = (vmClientGradingList != null && vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId) != null)
                                                            ? vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId).Description : "",

                                                          ABETName = (vmClientGradingList != null && vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId) != null)
                                                            ? vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId).ABETName : "",

                                                          Signature = (vmClientGradingList != null && vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId) != null)
                                                            ? vmClientGradingList.SingleOrDefault(g => g.GradeDetailId == tg.GradeDetailId).Signature ?? "" : "",

                                                      };

            return gradeType;
        }

        public IEnumerable<VmGradingType> GetTeamGradeMetaData(string currentUserId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();
            IEnumerable<VmClientGrading> vmClientGradingList = null;

            if (viewTeamGradeDetailRepository.GetTeamGradeDetailsCountByJudge(currentUserId) > 0)
            {
                vmClientGradingList = GetTeamGradeDetailByJudge(currentUserId);
            }

            var viewTeamGradeMetaDataRepository = UnitOfWork.GetRepository<ViewTeamGradeMetaDataRepository>();

            var teamGradeMetaDataList = viewTeamGradeMetaDataRepository.GetTeamGradeMetaData(currentUserId);

            var gradeTypeList = from tg in teamGradeMetaDataList
                                group tg
                                by new { tg.GradeId, tg.Grade } into taskGroup

                                select new VmGradingType
                                {
                                    GradeId = taskGroup.Key.GradeId,
                                    GradeType = taskGroup.Key.Grade,

                                    TeamGradingList = from t in taskGroup
                                                      group t by new { t.Id, t.TeamName } into teamGroup
                                                      select new VmTeamGrading
                                                      {
                                                          TeamId = teamGroup.Key.Id,
                                                          TeamName = teamGroup.Key.TeamName,
                                                          GradingDetailList = from gd in teamGroup
                                                                              select new VmGradingDetail
                                                                              {
                                                                                  Id = gd.GradeDetailId,
                                                                                  GradeId = gd.GradeId,
                                                                                  EvaluationItem = gd.EvaluationItem,
                                                                                  Coefficient = gd.Coefficient,
                                                                                  MaxPoint = gd.Point,

                                                                                  Point = (vmClientGradingList != null)
                                                                                    ? vmClientGradingList.SingleOrDefault(
                                                                                        g => g.GradeDetailId == gd.GradeDetailId
                                                                                        && g.TeamId == gd.Id
                                                                                                ).Point : null,

                                                                                  Description = (vmClientGradingList != null)
                                                                                    ? vmClientGradingList.SingleOrDefault(
                                                                                        g => g.GradeDetailId == gd.GradeDetailId
                                                                                        && g.TeamId == gd.Id
                                                                                                ).Description : "",

                                                                                  Signature = (vmClientGradingList != null)
                                                                                    ? vmClientGradingList.SingleOrDefault(
                                                                                        g => g.GradeDetailId == gd.GradeDetailId
                                                                                        && g.TeamId == gd.Id
                                                                                                ).Signature ?? "" : "",
                                                                              }

                                                      }
                                };


            return gradeTypeList;
        }
        public VmTeamGradeDetail GetTeamGradeMetaData(int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();
            if (viewTeamGradeDetailRepository.GetTeamGradeDetailsCount(teamId) > 0)
            {
                return GetTeamGradeDetail(teamId);
            }
            else
            {

                var viewTeamGradeMetaDataRepository = UnitOfWork.GetRepository<ViewTeamGradeMetaDataRepository>();

                var teamGradeMetaDataList = viewTeamGradeMetaDataRepository.GetTeamGradeMetaData(teamId);

                var vmTeamGradeDetail = new VmTeamGradeDetail
                {
                    GradeDetailIds = string.Join(",", from e in teamGradeMetaDataList select e.GradeDetailId),
                    EvaluationItems = string.Join(",", from e in teamGradeMetaDataList select e.EvaluationItem),
                    Points = string.Join(",", from e in teamGradeMetaDataList select e.Point),
                    Coefficients = string.Join(",", from e in teamGradeMetaDataList select e.Coefficient),
                    MaxPoints = string.Join(",", from e in teamGradeMetaDataList select e.Point),
                    GradeId = teamGradeMetaDataList.First().GradeId,
                    Grade = teamGradeMetaDataList.First().Grade,
                    TeamName = teamGradeMetaDataList.First().TeamName,
                    TeamId = teamGradeMetaDataList.First().Id,
                };

                return vmTeamGradeDetail;
            }
        }

        public VmTeamGradeDetail GetTeamGradeDetail(int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.GetTeamGradeDetails(teamId);

            var vmTeamGradeDetail = new VmTeamGradeDetail
            {
                GradeDetailIds = string.Join(",", from e in teamGradeList select e.GradeDetailId),
                EvaluationItems = string.Join(",", from e in teamGradeList select e.EvaluationItem),
                Points = string.Join(",", from e in teamGradeList select e.Point),
                Coefficients = string.Join(",", from e in teamGradeList select e.Coefficient),
                MaxPoints = string.Join(",", from e in teamGradeList select e.MaxPoint),
                GradeId = teamGradeList.First().GradeId,
                Grade = teamGradeList.First().Grade,
                TeamName = teamGradeList.First().TeamName,
                TeamId = teamGradeList.First().TeamId,
            };

            return vmTeamGradeDetail;
        }
        public IEnumerable<VmClientGrading> GetTeamGradeDetailByJudge(string judgeUserId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.GetTeamGradeDetailsBuJudge(judgeUserId);

            var vmClientGradingList = from g in teamGradeList
                                      select new VmClientGrading
                                      {
                                          TeamId = g.TeamId,
                                          GradeDetailId = g.GradeDetailId,
                                          Point = g.Point,
                                          Description = g.Description,
                                          Signature = g.Signature
                                      };

            return vmClientGradingList;
        }
        public IEnumerable<VmTeamGradeDetail> GetAllTeamGradeDetail()
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.Select(0, int.MaxValue);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           GradeDetailId = g.GradeDetailId,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description ?? "",
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> GetAllTeamGradeDetailByTaskIds(int[] taskIds)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.Select(taskIds);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> GetAllTeamGradeDetailByTaskIdsWithoutGradeId(int[] taskIds, int gradeId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.SelectWithoutGradeId(taskIds, gradeId);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> BenchScaleGetAllTeamGradeDetailByTaskIds(int[] taskIds, int gradeId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.BenchScaleSelect(taskIds, gradeId);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> GetAllTeamGradeDetailByTaskIdsAndJudge(int[] taskIds, string judgeUserId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.SelectByJudge(taskIds, judgeUserId);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> GetAllTeamGradeDetailByTaskIdsAndJudgeWithoutGradeId(int[] taskIds, string judgeUserId, int gradeId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.SelectByJudgeWithoutGradeId(taskIds, judgeUserId, gradeId);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmTeamGradeDetail> BenchScaleGetAllTeamGradeDetailByTaskIdsAndJudge(int[] taskIds, string judgeUserId, int gradeId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.BenchScaleSelectByJudge(taskIds, judgeUserId, gradeId);

            var teamGradeDetailList = (from g in teamGradeList
                                       select new VmTeamGradeDetail
                                       {
                                           Id = g.Id,
                                           GradeId = g.GradeId,
                                           Grade = g.Grade,
                                           TeamId = g.TeamId,
                                           TeamName = g.TeamName,
                                           TaskId = g.TaskId,
                                           Date = g.Date,
                                           State = g.State,
                                           ImageUrl = g.ImageUrl,
                                           LabResultUrl = g.LabResultUrl,

                                           PreliminaryReportUrl = g.PreliminaryReportUrl,
                                           OpenTaskTestPlanUrl = g.OpenTaskTestPlanUrl,
                                           WrittenReportUrl = g.WrittenReportUrl,

                                           EvaluationItem = g.EvaluationItem,
                                           MaxPoint = g.MaxPoint,
                                           Point = g.Point,
                                           TeamNumber = g.TeamNumber,
                                           Description = g.Description,
                                           JudgeUserId = g.JudgeUserId,
                                           Coefficient = g.Coefficient,
                                           Signature = g.Signature,
                                           FirstName = g.FirstName,
                                           LastName = g.LastName,
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmGradeDetail> GetGradingEvaluationDetail(int taskId, int teamId, int gradeId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.GetGradingEvaluationDetail(taskId, teamId, gradeId);

            var teamGradeDetailList = (from t in teamGradeList
                                       group new
                                       {
                                           t.Id,
                                           t.GradeId,
                                           t.Grade,
                                           t.TeamId,
                                           t.TeamName,
                                           t.TaskId,
                                           t.Date,
                                           t.State,
                                           t.ImageUrl,
                                           t.LabResultUrl,

                                           t.PreliminaryReportUrl,
                                           t.OpenTaskTestPlanUrl,
                                           t.WrittenReportUrl,

                                           t.EvaluationItem,
                                           t.MaxPoint,
                                           t.Point,
                                           t.TeamNumber,
                                           t.Description,
                                           t.JudgeUserId,
                                           t.Coefficient,
                                           t.Signature,
                                           t.FirstName,
                                           t.LastName,
                                       } by new { t.GradeDetailId, t.EvaluationItem } into g
                                       select new VmGradeDetail
                                       {
                                           Id = g.Key.GradeDetailId,
                                           EvaluationItem = g.Key.EvaluationItem,
                                           JudgeList = (from j in g.ToList()
                                                        select new VmJudgeGrade
                                                        {
                                                            JudgeUserId = j.JudgeUserId,
                                                            Signature = j.Signature,
                                                            JudgeName = j.FirstName + " " + j.LastName,
                                                            Point = j.Point,
                                                            Description = j.Description?.Replace("\'", "\\'").Replace("\r", "").Replace("\n", "<br/>").Replace("\"", "\\\"")

                                                        }).ToList()
                                       }).ToList();

            return teamGradeDetailList;
        }
        public IEnumerable<VmClientGrading> GetSingleTeamGradeDetailByJudge(string judgeUserId, int gradeId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.GetSingleTeamGradeDetailsBuJudge(judgeUserId, gradeId, teamId);

            var vmClientGradingList = from g in teamGradeList
                                      select new VmClientGrading
                                      {
                                          TeamId = g.TeamId,
                                          GradeDetailId = g.GradeDetailId,
                                          Point = g.Point,
                                          Description = g.Description,
                                          GradeId = gradeId,
                                          Signature = g.Signature,
                                          ABETId = g.ABETId,
                                          ABETName = g.ABETName
                                      };

            return vmClientGradingList;
        }
        public IEnumerable<VmClientGrading> GetSingleTeamGradeDetailByJudgeAndTeam(string judgeUserId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var teamGradeList = viewTeamGradeDetailRepository.GetSingleTeamGradeDetailByJudgeAndTeam(judgeUserId, teamId);

            var vmClientGradingList = from g in teamGradeList
                                      select new VmClientGrading
                                      {
                                          TeamId = g.TeamId,
                                          GradeDetailId = g.GradeDetailId,
                                          Point = g.Point,
                                          Description = g.Description,
                                          Signature = g.Signature,
                                          ABETId = g.ABETId,
                                          ABETName = g.ABETName
                                      };

            return vmClientGradingList;
        }

        public double? GetSingleTeamTotalScoreWithoutGrade(string judgeUserId, int gradeId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            return viewTeamGradeDetailRepository.GetSingleTeamTotalScoreWithoutGrade(judgeUserId, gradeId, teamId);

        }

        public VmTotalScoreData GetSingleTeamTotalScoreWithGradeWithoutCurrentJudge(string judgeUserId, int gradeId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            var currentJudgeGradeDetailList = viewTeamGradeDetailRepository.GetSingleTeamGradeDetailByJudge(judgeUserId, gradeId, teamId);

            var gradeDetailList = viewTeamGradeDetailRepository.GetSingleTeamGradeDetailWithoutJudge(judgeUserId, gradeId, teamId);

            var vmTotalScoreData = new VmTotalScoreData()
            {
                GradeId = gradeId,
                TeamId = teamId,
                TotalScore = 0,
            };

            if (gradeDetailList.Count() > 0)
            {
                if (gradeDetailList.Where(t => t.Point == null).Count() == gradeDetailList.Count())
                {
                    vmTotalScoreData.JudgeCount = 0;
                }
                else
                {
                    vmTotalScoreData.JudgeCount = gradeDetailList.GroupBy(g => g.JudgeUserId).Count();
                }

                vmTotalScoreData.TotalScore = gradeDetailList.Sum(t => (t.Point ?? 0) * t.Coefficient);

            }

            if (currentJudgeGradeDetailList.Count() > 0)
            {
                if (currentJudgeGradeDetailList.Where(t => t.Point == null).Count() == currentJudgeGradeDetailList.Count())
                {
                    vmTotalScoreData.CurrentJudgeHasResult = false;
                }
                else
                {
                    vmTotalScoreData.CurrentJudgeHasResult = true;
                }
            }

            return vmTotalScoreData;
        }

        public double? GetTotalScore(string judgeUserId, int teamId)
        {
            var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();

            return viewTeamGradeDetailRepository.GetTotalScore(judgeUserId, teamId);
        }

        public bool CreateTeamGradeDetail(VmTeamGradeDetail vmTeamGradeDetail)
        {
            var Points = vmTeamGradeDetail.Points.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var Coefficients = vmTeamGradeDetail.Coefficients.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var gradeRepository = UnitOfWork.GetRepository<TeamGradeDetailRepository>();

            var newTeamGradeDetailList = new List<TeamGradeDetail>();
            var i = 0;
            foreach (var item in vmTeamGradeDetail.GradeDetailIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                newTeamGradeDetailList.Add(new TeamGradeDetail
                {
                    GradeDetailId = int.Parse(item),
                    TeamId = vmTeamGradeDetail.TeamId,
                    Point = double.Parse(Points[i]),
                });
                i++;
            }

            gradeRepository.DeleteTeamGradeDetailsByTeam(vmTeamGradeDetail.TeamId);

            gradeRepository.CreateTeamGradeDetail(newTeamGradeDetailList);

            return UnitOfWork.Commit();

        }
        public bool UpdateTeamGradeDetail(string judgeUserId, VmClientGrading[] clientGrading)
        {
            var gradeRepository = UnitOfWork.GetRepository<TeamGradeDetailRepository>();
            var teamIds = clientGrading.Select(t => t.TeamId);

            gradeRepository.DeleteTeamGradeDetailsByJudgeAndTeam(judgeUserId, teamIds);

            var teamGradeDetailList = new List<TeamGradeDetail>();

            foreach (var item in clientGrading)
            {
                teamGradeDetailList.Add(new TeamGradeDetail
                {
                    JudgeUserId = judgeUserId,
                    GradeDetailId = item.GradeDetailId,
                    TeamId = item.TeamId,
                    Point = item.Point,
                    Description = item.Description,
                    Signature = item.Signature,
                    ABETId = item.ABETId,
                });
            }

            gradeRepository.CreateTeamGradeDetail(teamGradeDetailList);

            return UnitOfWork.Commit();

        }

        public bool UpdateSingleTeamGradeDetail(string judgeUserId, int GradeId, int teamId, VmClientGrading[] clientGrading)
        {
            var gradeRepository = UnitOfWork.GetRepository<TeamGradeDetailRepository>();

            gradeRepository.DeleteTeamGradeDetailsByJudgeAndTeam(judgeUserId, clientGrading.Select(d => d.GradeDetailId).ToArray(), teamId);

            var teamGradeDetailList = new List<TeamGradeDetail>();

            foreach (var item in clientGrading)
            {
                teamGradeDetailList.Add(new TeamGradeDetail
                {
                    JudgeUserId = judgeUserId,
                    GradeDetailId = item.GradeDetailId,
                    TeamId = item.TeamId,
                    Point = item.Point,
                    Description = item.Description,
                    Signature = item.Signature,
                    ABETId = item.ABETId,
                });
            }

            gradeRepository.CreateTeamGradeDetail(teamGradeDetailList);

            return UnitOfWork.Commit();

        }

    }
}
