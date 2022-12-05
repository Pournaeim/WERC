using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TaskTestRepository : EFBaseRepository<TaskTest>
    {
        public void CreateTasksTest(TaskTest taskTest)
        {
            Add(taskTest);
        }
        public IEnumerable<TaskTest> GetTaskTests(int taskId)
        {
            return (from s in Context.TaskTests where s.TaskId == taskId select s).ToArray();

        }
        public IEnumerable<TaskTest> GetTaskTestByTask(int teskId)
        {
            return (from s in Context.TaskTests where s.Id == teskId select s).ToArray();

        }
        public int[] GetTaskTestIds(int taskId)
        {
            return (from s in Context.TaskTests where s.TaskId == taskId select s.TestId).ToArray();

        }
        public void DeleteTasksTests(int taskId)
        {
            var deletableTaskTests = from s in Context.TaskTests where s.TaskId == taskId select s;

            foreach (var item in deletableTaskTests)
            {
                Delete(item);
            }

        }
        public void CreateTasksTest(int taskId, int[] gradeIds)
        {
            foreach (var item in gradeIds)
            {
                Add(new TaskTest { TaskId = taskId, TestId = item });
            }
        }
    }
}
