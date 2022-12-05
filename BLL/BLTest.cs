using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Lab;
using Model.ViewModels.Task;
using Model.ViewModels.Test;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLTest : BLBase
    {
        public IEnumerable<VmSelectListItem> GetTestSelectListItem(int index, int count)
        {
            var testRepository = UnitOfWork.GetRepository<TestRepository>();

            var testList = testRepository.Select(index, count);
            var vmSelectListItem = (from test in testList
                                    select new VmSelectListItem
                                    {
                                        Value = test.Id.ToString(),
                                        Text = test.Name,
                                    });

            return vmSelectListItem;
        }
        public IEnumerable<VmSelectListItem> GetTestSelectListItemWithDescription(int index, int count)
        {
            var testRepository = UnitOfWork.GetRepository<TestRepository>();

            var testList = testRepository.Select(index, count);
            var vmSelectListItem = (from test in testList
                                    select new VmSelectListItem
                                    {
                                        Value = test.Id.ToString(),
                                        Text = test.Name + " : " + test.Description,
                                    });

            return vmSelectListItem;
        }
        public IEnumerable<VmTest> GetTestList(string testName = "")
        {
            var viewTestRepository = UnitOfWork.GetRepository<TestRepository>();

            var testList = viewTestRepository.GetTests(testName);

            var vmTestList = from test in testList
                             select new VmTest
                             {
                                 Id = test.Id,
                                 Name = test.Name,
                                 Description = test.Description,
                             };

            return vmTestList;
        }
        public VmTeamTestCollection GetTeamTaskTest(string labUserId, int taskId)
        {

            var teamTestResultRepository = UnitOfWork.GetRepository<TeamTestResultRepository>();
            var teamTestResultList = teamTestResultRepository.GetTeamTestResults(labUserId, taskId);

            var viewTeamTaskTestRepository = UnitOfWork.GetRepository<ViewTeamTaskTestRepository>();

            var teamTaskTestList = viewTeamTaskTestRepository.GetViewTeamTaskTests(labUserId, taskId);

            var vmTestList = (from test in teamTaskTestList
                              group new { test.Id, test.Name, test.Description }
                              by new { test.TaskId, test.TeamId, test.TeamName } into tg
                              select new VmTeamTest
                              {
                                  TaskId = tg.Key.TaskId,
                                  TeamId = tg.Key.TeamId,
                                  TeamName = tg.Key.TeamName,
                                  TestList = (from tl in tg.ToList()
                                              select new VmTest
                                              {
                                                  Id = tl.Id,
                                                  Name = tl.Name,
                                                  Description = tl.Description,
                                                  Score = (teamTestResultList != null && teamTestResultList.Count() > 0) ?
                                                  teamTestResultList.SingleOrDefault(t => t.TeamId == tg.Key.TeamId && t.TestId == tl.Id)?.Score : ""

                                              }).OrderBy(t => t.Name).ToList()
                              }).ToList();

            return new VmTeamTestCollection
            {
                TaskId = taskId,
                TeamTestList = vmTestList
            };
        }
        public List<VmLabUsers> GetTeamTaskTestCollections()
        {
            var labUsers = new List<VmLabUsers>();
            var userTasks = new List<VmUserTask>();

            try
            {
                var teamTestResultRepository = UnitOfWork.GetRepository<TeamTestResultRepository>();
                var teamTestResultList = teamTestResultRepository.GetAllTeamTestResults();

                var viewTeamTaskTestRepository = UnitOfWork.GetRepository<ViewTeamTaskTestRepository>();
                var teamTaskTestList = viewTeamTaskTestRepository.GetAllViewTeamTaskTests();

                var viewUserTaskRepository = UnitOfWork.GetRepository<ViewUserTaskRepository>();
                var vmUserTasks = viewUserTaskRepository.GetTasksByRole(SystemRoles.Lab);


                foreach (var userTask in vmUserTasks)
                {

                    var vmTestList = (from test in teamTaskTestList

                                      where test.UserId == userTask.UserId && test.TaskId == userTask.TaskId

                                      group new { test.Id, test.Name, test.Description }

                                      by new { test.TaskId, test.TeamId, test.TeamName, test.Task } into tg

                                      select new VmTeamTest
                                      {
                                          TaskId = tg.Key.TaskId,
                                          TeamId = tg.Key.TeamId,
                                          TeamName = tg.Key.TeamName,
                                          TaskName = userTask.TaskName,

                                          TestList = (from tl in tg.ToList()
                                                      select new VmTest
                                                      {
                                                          Id = tl.Id,
                                                          Name = tl.Name,
                                                          Description = tl.Description,
                                                          Score = (teamTestResultList != null && teamTestResultList.Count() > 0) ?
                                                          teamTestResultList.FirstOrDefault(t => t.UserId == userTask.UserId && t.TeamId == tg.Key.TeamId && t.TaskId == tg.Key.TaskId && t.TestId == tl.Id)?.Score : ""

                                                      }).OrderBy(t => t.Name).ToList()
                                      }).ToList();

                    userTasks.Add(new VmUserTask
                    {
                        UserId = userTask.UserId,
                        UserName = userTask.UserName,
                        Email = userTask.Email,
                        FirstName = userTask.FirstName,
                        LastName = userTask.LastName,
                        Name = userTask.Name,
                        TaskName = userTask.TaskName,
                        TeamTests = vmTestList
                    });

                }
                labUsers = (
                              from labUser in userTasks
                              group labUser
                              by new { labUser.UserName, labUser.Name } into userGroup
                              select new VmLabUsers
                              {
                                  Name = userGroup.Key.Name,
                                  Tasks = (from t in userGroup
                                           group t by new { t.TaskName } into taskGroup
                                           select new VmTask
                                           {
                                               Name = taskGroup.Key.TaskName,
                                               UserTasks = (from ut in taskGroup
                                                            select ut).ToList()

                                           }).ToList()
                              }

                             ).ToList();
                return labUsers;
            }
            catch (Exception ex)
            {

            }
            return labUsers;

        }
        public VmTeamTestCollection GetTeamTaskTestByTeam(int teamId)
        {

            var teamTestResultRepository = UnitOfWork.GetRepository<TeamTestResultRepository>();
            var teamTestResultList = teamTestResultRepository.GetTeamTestResultsByTeam(teamId);

            var viewTeamTestResultRepository = UnitOfWork.GetRepository<ViewTeamTestResultRepository>();

            var teamTaskTestList = viewTeamTestResultRepository.GetViewTeamTestResultsByTeam(teamId);
            if (teamTaskTestList.Count() > 0)
            {
                var vmTestList = new VmTeamTest
                {
                    TeamId = teamTaskTestList.First().TeamId,
                    TeamName = teamTaskTestList.First().TeamName,

                    TestList = (from tl in teamTaskTestList.ToList()
                                select new VmTest
                                {
                                    Id = tl.Id,
                                    Name = tl.Name,
                                    Description = tl.Description,
                                    Score = tl.Score,
                                    UserId = tl.UserId
                                    
                                }).OrderBy(u=>u.UserId).ToList()

                };

                return new VmTeamTestCollection
                {
                    TeamTestList = new List<VmTeamTest>() { vmTestList }
                };
            }
            else
            {
                return new VmTeamTestCollection
                {
                    TeamTestList = null
                };
            }
        }
        public VmTest GetTestById(int id)
        {
            var viewTestRepository = UnitOfWork.GetRepository<TestRepository>();

            var test = viewTestRepository.GetTestById(id);

            var vmTest = new VmTest
            {
                Id = test.Id,
                Name = test.Name,
                Description = test.Description,
            };

            return vmTest;
        }
        public IEnumerable<VmTest> GetAllTest()
        {
            var viewTestRepository = UnitOfWork.GetRepository<TestRepository>();

            var testList = viewTestRepository.Select(0, int.MaxValue);

            var vmTestList = from test in testList
                             select new VmTest
                             {
                                 Id = test.Id,
                                 Name = test.Name,
                                 Description = test.Description,
                             };

            return vmTestList;
        }
        public int CreateTest(VmTest vmTest)
        {
            var result = -1;
            try
            {
                var testRepository = UnitOfWork.GetRepository<TestRepository>();

                var newTest = new Test
                {
                    Id = vmTest.Id,
                    Name = vmTest.Name,
                    Description = vmTest.Description,
                };

                testRepository.CreateTest(newTest);

                UnitOfWork.Commit();

                result = newTest.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateTest(VmTest vmTest)
        {
            try
            {
                var testRepository = UnitOfWork.GetRepository<TestRepository>();

                var updateableTest = new Test
                {
                    Id = vmTest.Id,
                    Name = vmTest.Name,
                    Description = vmTest.Description,
                };

                testRepository.UpdateTest(updateableTest);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateTest(string labUserId, VmTeamTestResult[] vmTeamTestResult)
        {
            try
            {
                var teamTestResultRepository = UnitOfWork.GetRepository<TeamTestResultRepository>();
                teamTestResultRepository.DeleteTeamTestResult(labUserId, vmTeamTestResult.First().TaskId);

                teamTestResultRepository.CreateTeamTestResult(labUserId, vmTeamTestResult);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public string CheckDeleteTest(int testId)
        {

            var message = "";

            var taskTestRepository = UnitOfWork.GetRepository<TaskTestRepository>();

            if (taskTestRepository.GetTaskTestByTask(testId).Count() != 0)
            {
                message += "This test has assigned to task.\n";
                message += "You can't delete it... ";
            }

            return message;

        }

        public bool DeleteTest(int testId)
        {
            try
            {
                var testRepository = UnitOfWork.GetRepository<TestRepository>();

                testRepository.DeleteTest(testId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }


    }
}
