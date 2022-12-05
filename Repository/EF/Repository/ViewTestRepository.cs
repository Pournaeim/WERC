using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewTestRepository : EFBaseRepository<ViewTest>
    {
        public IEnumerable<ViewTest> Select(int index, int count)
        {
            var ViewTestList = from ViewTest in Context.ViewTests
                               select ViewTest;

            return ViewTestList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTest> GetViewTests(string taskName = "")
        {

            var taskList = from task in Context.ViewTests
                           select task;

            if (taskName != "")
            {
                taskList = taskList.Where(t => t.Name.Contains(taskName));
            }

            return taskList.ToArray();
        }


        public IEnumerable<ViewTest> GetViewTestById(int id)
        {

            return Context.ViewTests.Where(t => t.Id == id);
        }
        public IEnumerable<ViewTest> GetViewTestByTask(int taskId)
        {

            return Context.ViewTests.Where(t => t.TaskId == taskId);
        }
    }
}
