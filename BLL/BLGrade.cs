
using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Exceldata;
using Model.ViewModels.Grade;
using Model.ViewModels.Grade.Report;
using Model.ViewModels.Team;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLGrade : BLBase
    {
        public List<VmExceldata> GetExcelData(int teamId)
        {
            var exceldataRepository = UnitOfWork.GetRepository<ExceldataRepository>();
            var blTeam = new BLTeam();
            var teamName = blTeam.GetTeamById(teamId).Name;

            var result = (from e in exceldataRepository.Select(teamName)
                          select new VmExceldata
                          {
                              a = e.a,
                              b = e.b,
                              c = e.c,
                              d = e.d,

                          }).ToList();

            return result;
        }
        public IEnumerable<VmSelectListItem> GetGradeSelectListItem(int index, int count)
        {
            var GradeRepository = UnitOfWork.GetRepository<GradeRepository>();

            var gradeList = GradeRepository.Select(index, count);
            var vmSelectListItem = (from grade in gradeList
                                    select new VmSelectListItem
                                    {
                                        Value = grade.Id.ToString(),
                                        Text = grade.Name,
                                    });

            return vmSelectListItem;
        }

        public IEnumerable<VmSelectListItem> GetABETSelectListItem(int index, int count)
        {
            var abetRepository = UnitOfWork.GetRepository<ABETRepository>();

            var abetList = abetRepository.Select(index, count);
            var vmSelectListItem = (from abet in abetList
                                    select new VmSelectListItem
                                    {
                                        Value = abet.Id.ToString(),
                                        Text = abet.Name,
                                    });

            return vmSelectListItem;
        }
        public IEnumerable<VmSelectListItem> GetGradeSelectListItemByTask(int taskId, int index, int count)
        {

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskById(taskId);
            var vmSelectListItem = (from grade in taskGradeList
                                    select new VmSelectListItem
                                    {
                                        Value = grade.GradeId.ToString(),
                                        Text = grade.Grade,
                                    });

            return vmSelectListItem;
        }
        public IEnumerable<VmSelectListItem> GetGradeSelectListItemByTaskWithoutGradeId(int taskId, int index, int count, int gradeId)
        {

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskByIdWithoutGradeId(taskId, gradeId);
            var vmSelectListItem = (from grade in taskGradeList
                                    select new VmSelectListItem
                                    {
                                        Value = grade.GradeId.ToString(),
                                        Text = grade.Grade,
                                    });

            return vmSelectListItem;
        }

        public IEnumerable<VmGrade> GetGradeList(string gradeName = "")
        {
            var gradeRepository = UnitOfWork.GetRepository<GradeRepository>();

            var gradeList = gradeRepository.GetGrades(gradeName);

            var vmGradeList = from grade in gradeList
                              select new VmGrade
                              {
                                  Id = grade.Id,
                                  Name = grade.Name,
                              };

            return vmGradeList;
        }

        public IEnumerable<VmTaskBaseGrade> GetCurrentJudgeGradeReportList(string judgeUserId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskByIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIdsAndJudge(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), judgeUserId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> GetCurrentJudgeGradeReportListWithoutGradeId(string judgeUserId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskByIdsWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIdsAndJudgeWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()
                , judgeUserId, gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> BenchScaleGetCurrentJudgeGradeReportList(string judgeUserId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.BenchScaleGetViewTaskByIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.BenchScaleGetAllTeamGradeDetailByTaskIdsAndJudge(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()
                , judgeUserId, gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();



            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in taskBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        item.TeamGradeList.RemoveAt(i);

                    }

                }
            }

            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> GetGradeReportList(string judgeUserId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskByIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> GetGradeReportListWithoutGradeId(string judgeUserId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.GetViewTaskByIdsWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIdsWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> BenchScaleGetGradeReportList(string judgeUserId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            if (!string.IsNullOrWhiteSpace(judgeUserId))
            {
                taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudge(judgeUserId).ToList();
            }
            else
            {
                taskTeamRepositoryList = viewTaskTeamRepository.Select(0, int.MaxValue).ToList();
            }

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var taskGradeList = viewTaskRepository.BenchScaleGetViewTaskByIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.BenchScaleGetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();



            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in taskBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        item.TeamGradeList.RemoveAt(i);

                    }

                }
            }


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmJudgeBaseGrade> GetNoneAnsweredGradeReportListBasedOnJudge()
        {
            var viewTeamGradeMetaDataRepository = UnitOfWork.GetRepository<ViewTeamGradeMetaDataRepository>();
            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();
            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var blTeamGradeDetail = new BLTeamGradeDetail();

            ///Load all Grade meta data
            var allTeamGradeMetaDataList = viewTeamGradeMetaDataRepository.GetAllTeamGradeMetaDatas();

            var judgeUserIds = (from allmetaData in allTeamGradeMetaDataList
                                group allmetaData by new { allmetaData.UserId } into allGrouped

                                select allGrouped.Key.UserId).ToList();

            ///Load all Grade result of teams
            var allTeamGradeDetailResultList = blTeamGradeDetail.GetAllTeamGradeDetail().ToList();

            var judgesPersonInfos = new BLPerson().GetPersonsByUserIdsAndRole(judgeUserIds.ToArray(), Model.ApplicationDomainModels.ConstantObjects.SystemRoles.Judge);

            foreach (var judge in judgesPersonInfos)
            {
                if (judgeUserIds.Contains(judge.UserId) == false)
                    judgeUserIds.Remove(judge.UserId);
            }

            var judgeBaseGradeList = (from allmetaData in allTeamGradeMetaDataList
                                      group allmetaData by new { allmetaData.UserId } into allGrouped

                                      // let judgePersonInfo = judgesPersonInfos.First(p => p.UserId == allGrouped.Key.UserId)

                                      select new VmJudgeBaseGrade
                                      {
                                          JudgeUserId = allGrouped.Key.UserId,
                                          // JudgeName = judgePersonInfo.FirstName + " " + judgePersonInfo.LastName,

                                          TeamGradeList = (from t in allGrouped.ToList()

                                                           group t by new { t.Id, t.TeamName } into groupedTeam

                                                           select new VmTeamGrade
                                                           {
                                                               TeamId = groupedTeam.Key.Id,
                                                               TeamName = groupedTeam.Key.TeamName,

                                                               GradeReportList = (from grade in groupedTeam

                                                                                  where grade.Id == groupedTeam.Key.Id //TeamId

                                                                                  group grade by new { grade.GradeId, grade.Grade } into gg

                                                                                  select new VmGradeReport
                                                                                  {
                                                                                      GradeId = gg.Key.GradeId,
                                                                                      GradeType = gg.Key.Grade,

                                                                                      TeamGradeDetails = (from gd in gg
                                                                                                          select new VmTeamGradeDetail
                                                                                                          {
                                                                                                              Id = gd.GradeDetailId,
                                                                                                              GradeId = gg.Key.GradeId,
                                                                                                              Grade = gg.Key.Grade,
                                                                                                              EvaluationItem = gd.EvaluationItem,
                                                                                                              Point = gd.Point,
                                                                                                              Coefficient = gd.Coefficient,
                                                                                                              TeamId = groupedTeam.Key.Id,
                                                                                                              TeamName = groupedTeam.Key.TeamName,
                                                                                                              JudgeUserId = allGrouped.Key.UserId,

                                                                                                          }).ToList()

                                                                                  }).ToList(),

                                                           }).ToList()
                                      }).ToList();


            var deleteJudgeBaseGradeList = new List<VmJudgeBaseGrade>();

            foreach (var judgeBaseGrade in judgeBaseGradeList)
            {
                var judgePersonInfoCount = judgesPersonInfos.Count(p => p.UserId == judgeBaseGrade.JudgeUserId);
                if (judgePersonInfoCount > 0)
                {
                    var judgePersonInfo = judgesPersonInfos.First(p => p.UserId == judgeBaseGrade.JudgeUserId);
                    judgeBaseGrade.JudgeName = judgePersonInfo.FirstName + " " + judgePersonInfo.LastName;
                }
                else
                {
                    deleteJudgeBaseGradeList.Add(judgeBaseGrade);
                }


                foreach (var teamGrade in judgeBaseGrade.TeamGradeList)
                {

                    int gradeReportCount = teamGrade.GradeReportList.Count();
                    do
                    {
                        var gradeReport = teamGrade.GradeReportList[--gradeReportCount];

                        int count = gradeReport.TeamGradeDetails.Count();

                        do
                        {

                            var teamGradeDetail = gradeReport.TeamGradeDetails[--count];

                            var result = allTeamGradeDetailResultList.Where(
                                                                        tgdr => tgdr.JudgeUserId == teamGradeDetail.JudgeUserId && tgdr.TeamId == teamGradeDetail.TeamId &&
                                                                        tgdr.GradeDetailId == teamGradeDetail.Id);

                            if (result != null && result.Count() > 0 && result.First().Point != null)
                            {
                                gradeReport.TeamGradeDetails.Remove(teamGradeDetail);
                                count = gradeReport.TeamGradeDetails.Count();
                            }

                        }
                        while (count > 0);

                        if (gradeReport.TeamGradeDetails.Count() == 0)
                        {
                            teamGrade.GradeReportList.Remove(gradeReport);

                        }
                    } while (gradeReportCount > 0);
                }
            }

            foreach (var deleteItem in deleteJudgeBaseGradeList)
            {
                judgeBaseGradeList.Remove(deleteItem);
            }

            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in judgeBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        for (int j = 0; j < item.TeamGradeList[i].GradeReportList.Count(); j++)
                        {
                            if (item.TeamGradeList[i].GradeReportList[j].GradeId == 24)
                            {
                                item.TeamGradeList[i].GradeReportList.RemoveAt(j);
                            }
                        }

                    }

                }
            }


            return judgeBaseGradeList;
        }
        public IEnumerable<VmJudgeBaseGrade> GetJudgeBaseTeamComments()
        {
            var viewTeamGradeMetaDataRepository = UnitOfWork.GetRepository<ViewTeamGradeMetaDataRepository>();
            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();
            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();
            var blTeamGradeDetail = new BLTeamGradeDetail();

            ///Load all Grade meta data
            var allTeamGradeMetaDataList = viewTeamGradeMetaDataRepository.GetAllTeamGradeMetaDatas();

            var judgeUserIds = (from allmetaData in allTeamGradeMetaDataList
                                group allmetaData by new { allmetaData.UserId } into allGrouped

                                select allGrouped.Key.UserId).ToList();

            ///Load all Grade result of teams
            var allTeamGradeDetailResultList = blTeamGradeDetail.GetAllTeamGradeDetail().ToList();

            var judgesPersonInfos = new BLPerson().GetPersonsByUserIdsAndRole(judgeUserIds.ToArray(),
                Model.ApplicationDomainModels.ConstantObjects.SystemRoles.Judge).ToList();

            foreach (var judge in judgesPersonInfos)
            {
                if (judgeUserIds.Contains(judge.UserId) == false)
                    judgeUserIds.Remove(judge.UserId);
            }

            var judgeBaseGradeList = (from allmetaData in allTeamGradeMetaDataList
                                      group allmetaData by new { allmetaData.UserId } into allGrouped

                                      select new VmJudgeBaseGrade
                                      {
                                          JudgeUserId = allGrouped.Key.UserId,

                                          TeamGradeList = (from t in allGrouped.ToList()

                                                           group t by new { t.Id, t.TeamName } into groupedTeam

                                                           select new VmTeamGrade
                                                           {
                                                               TeamId = groupedTeam.Key.Id,
                                                               TeamName = groupedTeam.Key.TeamName,

                                                               GradeReportList = (from grade in groupedTeam

                                                                                  where grade.Id == groupedTeam.Key.Id //TeamId

                                                                                  group grade by new { grade.GradeId, grade.Grade } into gg

                                                                                  select new VmGradeReport
                                                                                  {
                                                                                      GradeId = gg.Key.GradeId,
                                                                                      GradeType = gg.Key.Grade,

                                                                                  }).ToList(),

                                                           }).ToList()
                                      }).ToList();


            var deleteJudgeBaseGradeList = new List<VmJudgeBaseGrade>();

            foreach (var judgeBaseGrade in judgeBaseGradeList)
            {
                var judgePersonInfoCount = judgesPersonInfos.Count(p => p.UserId == judgeBaseGrade.JudgeUserId);
                if (judgePersonInfoCount > 0)
                {
                    var judgePersonInfo = judgesPersonInfos.First(p => p.UserId == judgeBaseGrade.JudgeUserId);
                    judgeBaseGrade.JudgeName = judgePersonInfo.FirstName + " " + judgePersonInfo.LastName;


                }
                else
                {
                    deleteJudgeBaseGradeList.Add(judgeBaseGrade);
                }
            }

            foreach (var deleteItem in deleteJudgeBaseGradeList)
            {
                judgeBaseGradeList.Remove(deleteItem);
            }


            foreach (var judge in judgeBaseGradeList)
            {
                foreach (var teamGrade in judge.TeamGradeList)
                {
                    foreach (var greadReport in teamGrade.GradeReportList)

                    {
                        var teamGradeDetail = allTeamGradeDetailResultList.Where(
                                                                        tgdr => tgdr.JudgeUserId == judge.JudgeUserId &&
                                                                        tgdr.TeamId == teamGrade.TeamId &&
                                                                        tgdr.GradeId == greadReport.GradeId)
                                                                    .FirstOrDefault();
                        if (teamGradeDetail != null)
                        {
                            greadReport.Comment = teamGradeDetail.Description;
                        }
                    }



                }
            }


            //int[] teamIds = { 45, 46, 47, 48, 52 };
            //foreach (var item in judgeBaseGradeList)
            //{
            //    for (int i = 0; i < item.TeamGradeList.Count(); i++)
            //    {
            //        if (teamIds.Contains(item.TeamGradeList[i].TeamId))
            //        {
            //            for (int j = 0; j < item.TeamGradeList[i].GradeReportList.Count(); j++)
            //            {
            //                if (item.TeamGradeList[i].GradeReportList[j].GradeId == 24)
            //                {
            //                    item.TeamGradeList[i].GradeReportList.RemoveAt(j);
            //                }
            //            }

            //        }

            //    }
            //}

            return judgeBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> GetStudentGradeReportList(string studentUserId, int teamId = -1)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            VmTeamMember teamMember = null;

            if (teamId == -1)
            {
                teamMember = blTeamMember.GetTeamMemberByUserId(studentUserId);
                teamId = teamMember.TeamId;
            }
            else
            {
                teamMember = blTeamMember.GetTeamMemberByUserAndTeamId(studentUserId, teamId);

            }

            var taskId = teamMember.TaskId.Value;

            var blUserTask = new BLUserTask();

            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);
            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndTeam(judgeUserTaskList.ToArray(), teamId).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.GetViewTaskByIds(new int[] { taskId }).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();

             
            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> GetStudentGradeReportListWithoutGradeId(string studentUserId, int gradeId, int teamId = -1)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            VmTeamMember teamMember = null;

            if (teamId == -1)
            {
                teamMember = blTeamMember.GetTeamMemberByUserId(studentUserId);
                teamId = teamMember.TeamId;
            }
            else
            {
                teamMember = blTeamMember.GetTeamMemberByUserAndTeamId(studentUserId, teamId);

            }

            var taskId = teamMember.TaskId.Value;

            var blUserTask = new BLUserTask();

            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);
            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndTeam(judgeUserTaskList.ToArray(), teamId).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.GetViewTaskByIdsWithoutGradeId(new int[] { taskId }, gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIdsWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public IEnumerable<VmTaskBaseGrade> BenchScaleGetStudentGradeReportList(string studentUserId, int gradeId, int teamId = -1)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            VmTeamMember teamMember = null;

            if (teamId == -1)
            {
                teamMember = blTeamMember.GetTeamMemberByUserId(studentUserId);
                teamId = teamMember.TeamId;
            }
            else
            {
                teamMember = blTeamMember.GetTeamMemberByUserAndTeamId(studentUserId, teamId);

            }

            var taskId = teamMember.TaskId.Value;

            var blUserTask = new BLUserTask();

            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);
            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndTeam(judgeUserTaskList.ToArray(), teamId).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.BenchScaleGetViewTaskByIds(new int[] { taskId }, gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.BenchScaleGetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in taskBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        item.TeamGradeList.RemoveAt(i);

                    }

                }
            }

            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public List<VmTaskBaseGrade> GetStudentOtherTeamGradeReportList(string studentUserId, int teamId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            var teamMember = blTeamMember.GetTeamMemberByUserIdAndTeamId(studentUserId, teamId);
            var taskId = teamMember.TaskId.Value;


            var blUserTask = new BLUserTask();
            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);

            var bTeam = new BLTeam();
            var teamIdList = bTeam.GetTeamIdsByTask(taskId);


            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndteamIds(judgeUserTaskList.ToArray(), teamIdList.ToArray()).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.GetViewTaskByIds(new int[] { taskId }).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray()).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();


            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public List<VmTaskBaseGrade> GetStudentOtherTeamGradeReportListWithoutId(string studentUserId, int teamId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            var teamMember = blTeamMember.GetTeamMemberByUserIdAndTeamId(studentUserId, teamId);
            var taskId = teamMember.TaskId.Value;


            var blUserTask = new BLUserTask();
            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);

            var bTeam = new BLTeam();
            var teamIdList = bTeam.GetTeamIdsByTask(taskId);


            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndteamIds(judgeUserTaskList.ToArray(), teamIdList.ToArray()).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.GetViewTaskByIdsWithoutGradeId(new int[] { taskId }, gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetailByTaskIdsWithoutGradeId(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();

            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in taskBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        item.TeamGradeList.RemoveAt(i);

                    }

                }
            }
            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }
        public List<VmTaskBaseGrade> BenchScaleGetStudentOtherTeamGradeReportList(string studentUserId, int teamId, int gradeId)
        {

            var viewTaskTeamRepository = UnitOfWork.GetRepository<ViewTaskTeamRepository>();

            IEnumerable<ViewTaskTeam> taskTeamRepositoryList;

            var blTeamMember = new BLTeamMember();
            var teamMember = blTeamMember.GetTeamMemberByUserIdAndTeamId(studentUserId, teamId);
            var taskId = teamMember.TaskId.Value;


            var blUserTask = new BLUserTask();
            var judgeUserTaskList = blUserTask.GetUsersByTask(taskId);

            var bTeam = new BLTeam();
            var teamIdList = bTeam.GetTeamIdsByTask(taskId);


            taskTeamRepositoryList = viewTaskTeamRepository.GetTaskTeamByJudgesAndteamIds(judgeUserTaskList.ToArray(), teamIdList.ToArray()).ToList();

            var viewTaskRepository = UnitOfWork.GetRepository<ViewTaskRepository>();

            var taskGradeList = viewTaskRepository.BenchScaleGetViewTaskByIds(new int[] { taskId }, gradeId).ToList();

            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetailList = blTeamGradeDetail.BenchScaleGetAllTeamGradeDetailByTaskIds(taskTeamRepositoryList.Select(t => t.TaskId).Distinct().ToArray(), gradeId).ToList();

            var taskBaseGradeList = (from task in taskTeamRepositoryList
                                     group task by new { task.TaskId, task.TaskName } into g
                                     select new VmTaskBaseGrade
                                     {
                                         TaskId = g.Key.TaskId,
                                         TaskName = g.Key.TaskName,

                                         GradeList = (from gt in taskGradeList
                                                      where gt.Id == g.Key.TaskId
                                                      select new VmGrade
                                                      {
                                                          Id = gt.GradeId,
                                                          Name = gt.Grade
                                                      }).ToList(),

                                         TeamGradeList = (from team in g.ToList()
                                                          group team by new { team.TeamId, team.TeamName } into tg
                                                          select new VmTeamGrade
                                                          {
                                                              TeamId = tg.Key.TeamId,
                                                              TeamName = tg.Key.TeamName,
                                                          }).ToList()
                                     }).ToList();

            int[] teamIds = { 45, 46, 47, 48, 52 };
            foreach (var item in taskBaseGradeList)
            {
                for (int i = 0; i < item.TeamGradeList.Count(); i++)
                {
                    if (teamIds.Contains(item.TeamGradeList[i].TeamId))
                    {
                        item.TeamGradeList.RemoveAt(i);

                    }

                }
            }
            foreach (var taskBaseGrade in taskBaseGradeList)
            {
                foreach (var team in taskBaseGrade.TeamGradeList)
                {
                    team.GradeReportList = new List<VmGradeReport>();

                    foreach (var grade in taskBaseGrade.GradeList)
                    {
                        team.GradeReportList.Add(new VmGradeReport
                        {
                            GradeId = grade.Id,
                            GradeType = grade.Name,
                            Average = GetAverageTotal(teamGradeDetailList, taskBaseGrade.TaskId, team.TeamId, grade.Id)
                        });
                    }
                }
            }

            return taskBaseGradeList;
        }

        private double GetAverageTotal(IEnumerable<VmTeamGradeDetail> teamGradeDetailList, int taskId, int teamId, int gradeId)
        {
            var jugeUserList = teamGradeDetailList.Select(t => t.JudgeUserId).Distinct().ToArray();
            var total = 0d;
            int evaluatorJudgeCount = 0;

            foreach (var judgeUser in jugeUserList)
            {
                var judgeUserAllData = teamGradeDetailList.Where(t => t.JudgeUserId == judgeUser && t.TaskId == taskId && t.TeamId == teamId && t.GradeId == gradeId);

                if (judgeUserAllData.Count() > 0)
                {
                    var judgeUserNullDataCount = teamGradeDetailList.Where(t => t.JudgeUserId == judgeUser && t.TaskId == taskId && t.TeamId == teamId && t.GradeId == gradeId && t.Point == null).Count();

                    if (judgeUserNullDataCount != judgeUserAllData.Count())
                    {
                        evaluatorJudgeCount++;
                        total += judgeUserAllData.Sum(t => (t.Point ?? 0) * t.Coefficient);
                    }
                }

            }
            return Math.Round(total / evaluatorJudgeCount, 2);
        }
        public VmGrade GetGradeById(int id)
        {
            var gradeRepository = UnitOfWork.GetRepository<GradeRepository>();

            var grade = gradeRepository.GetGradeById(id);

            var vmGrade = new VmGrade
            {
                Id = grade.Id,
                Name = grade.Name,
                ABET = grade.ABET,
            };

            return vmGrade;
        }
        public VmGrade GetGradeWithDetailsById(int id)
        {
            var viewGradeDetailRepository = UnitOfWork.GetRepository<ViewGradeDetailRepository>();

            var gradeDetails = viewGradeDetailRepository.GetGradeDetailsByGrade(id);

            var evaluationItems = string.Join("■", from e in gradeDetails select e.EvaluationItem);
            var evaluationItemIds = string.Join("■", from e in gradeDetails select e.Id);
            var points = string.Join("■", from e in gradeDetails select e.Point);
            var coefficients = string.Join("■", from e in gradeDetails select e.Coefficient);

            var vmGrade = new VmGrade
            {
                Id = gradeDetails.First().Id,
                Name = gradeDetails.First().Name,
                EvaluationItems = evaluationItems,
                EvaluationItemIds = evaluationItemIds,
                Points = points,
                Coefficients = coefficients,
                ABET = gradeDetails.First().ABET,
            };

            return vmGrade;
        }

        public IEnumerable<VmGrade> GetAllGrade()
        {
            var GradeRepository = UnitOfWork.GetRepository<GradeRepository>();

            var gradeList = GradeRepository.Select(0, int.MaxValue);
            var vmGradeList = from grade in gradeList
                              select new VmGrade
                              {
                                  Id = grade.Id,
                                  Name = grade.Name,
                              };

            return vmGradeList;
        }

        public int CreateGrade(VmGrade vmGrade)
        {
            var result = -1;
            try
            {
                var Points = vmGrade.Points.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries);
                var Coefficients = vmGrade.Coefficients.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries);

                var gradeRepository = UnitOfWork.GetRepository<GradeRepository>();

                var newGradeDetailList = new List<GradeDetail>();
                var i = 0;
                foreach (var item in vmGrade.EvaluationItems.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    newGradeDetailList.Add(new GradeDetail
                    {
                        EvaluationItem = item,
                        Point = double.Parse(Points[i]),
                        Coefficient = double.Parse(Coefficients[i]),
                    });
                    i++;
                }
                var newGrade = new Grade
                {
                    Id = vmGrade.Id,
                    Name = vmGrade.Name,
                    GradeDetails = newGradeDetailList,
                    ABET = vmGrade.ABET,
                };

                gradeRepository.CreateGrade(newGrade);

                UnitOfWork.Commit();

                result = newGrade.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateGrade(VmGrade vmGrade)
        {
            try
            {
                ////var viewTeamGradeDetailRepository = UnitOfWork.GetRepository<ViewTeamGradeDetailRepository>();
                ////if (viewTeamGradeDetailRepository.GetTeamGradeDetailsCountByGraeId(vmGrade.Id) > 0)
                ////{
                ////    return false;
                ////}

                var teamGradeDetailRepository = UnitOfWork.GetRepository<TeamGradeDetailRepository>();
                var gradeDetailRepository = UnitOfWork.GetRepository<GradeDetailRepository>();
                var gradeRepository = UnitOfWork.GetRepository<GradeRepository>();

                var i = 0;
                var Points = vmGrade.Points.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries);
                var Coefficients = vmGrade.Coefficients.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries);
                var EvaluationItemIds = vmGrade.EvaluationItemIds.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries);

                if (vmGrade.DeletableEvaluationItemIds != null)
                {
                    var DeletableEvaluationItemIds = vmGrade.DeletableEvaluationItemIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in DeletableEvaluationItemIds)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            //var result = teamGradeDetailRepository.GetTeamGradeDetailsByGradeDetailId(int.Parse(item));
                            //if (result == null || result.Count() == 0)
                            {
                                gradeDetailRepository.DeleteDetailsGrade(int.Parse(item));
                            }
                        }
                    }
                }

                foreach (var item in vmGrade.EvaluationItems.Split(new char[] { '■' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (i < EvaluationItemIds.Length)
                    {
                        if (!string.IsNullOrWhiteSpace(EvaluationItemIds[i]))
                        {
                            gradeDetailRepository.UpdateGradeDetailsByGrade(new GradeDetail
                            {
                                Id = int.Parse(EvaluationItemIds[i]),
                                EvaluationItem = item,
                                Point = double.Parse(Points[i]),
                                Coefficient = double.Parse(Coefficients[i]),
                            });
                        }

                    }
                    else
                    {
                        gradeDetailRepository.CreateGradeDetail(new GradeDetail
                        {
                            GradeId = vmGrade.Id,
                            EvaluationItem = item,
                            Point = double.Parse(Points[i]),
                            Coefficient = double.Parse(Coefficients[i]),
                        });
                    }

                    i++;
                }

                var updateableGrade = new Grade
                {
                    Id = vmGrade.Id,
                    Name = vmGrade.Name,
                    ABET = vmGrade.ABET,
                };

                gradeRepository.UpdateGrade(updateableGrade);

                return UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool DeleteGrade(int gradeId)
        {
            try
            {
                var gradeRepository = UnitOfWork.GetRepository<GradeRepository>();

                if (gradeRepository.DeleteGrade(gradeId) == true)
                {
                    return UnitOfWork.Commit();
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

    }
}
