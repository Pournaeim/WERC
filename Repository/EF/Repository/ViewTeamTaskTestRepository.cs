using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTeamTaskTestRepository : EFBaseRepository<ViewTeamTaskTest>
    {
        public IEnumerable<ViewTeamTaskTest> Select(int index, int count)
        {
            var ViewTeamTaskTestList = from ViewTeamTaskTest in Context.ViewTeamTaskTests
                                       select ViewTeamTaskTest;

            return ViewTeamTaskTestList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTeamTaskTest> GetViewTeamTaskTests(string taskName = "")
        {

            var taskList = from task in Context.ViewTeamTaskTests
                           select task;

            if (taskName != "")
            {
                taskList = taskList.Where(t => t.Name.Contains(taskName));
            }

            return taskList.ToArray();
        }
          public IEnumerable<ViewTeamTaskTest> GetAllViewTeamTaskTests()
        {

            var taskList = from task in Context.ViewTeamTaskTests
                           select task;

            return taskList.ToArray();
        }

        public IEnumerable<ViewTeamTaskTest> GetViewTeamTaskTests(string labUserId, int taskId)
        {

            var taskList = from task in Context.ViewTeamTaskTests
                           where task.UserId == labUserId && task.TaskId == taskId
                           select task;

            return taskList.ToArray();
        }

        public IEnumerable<ViewTeamTaskTest> GetViewTeamTaskTestsByTeam(int teamId)
        {

            var taskList = from task in Context.ViewTeamTaskTests
                           where task.TeamId == teamId
                           select task;

            return taskList.ToArray();
        }


        public IEnumerable<ViewTeamTaskTest> GetViewTeamTaskTestById(int id)
        {

            return Context.ViewTeamTaskTests.Where(t => t.Id == id);
        }
    }
}
