using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TempUserTaskRepository : EFBaseRepository<TempUserTask>
    {
        public void CreateTasksUser(TempUserTask tempUserTask)
        {
            Add(tempUserTask);
        }
        public IEnumerable<TempUserTask> GetTempUserTasks(string userId)
        {

            return (from s in Context.TempUserTasks where s.UserId == userId select s).ToArray();
        }
        public int[] GetTempUserTaskIds(string userId)
        {

            return (from s in Context.TempUserTasks where s.UserId == userId orderby s.Id select s.TaskId).ToArray<int>();
        }
        public string GetUserIdByTask(int taskId)
        {

            return (from s in Context.TempUserTasks where s.TaskId == taskId select s).SingleOrDefault().UserId;
        }
        public IEnumerable<TempUserTask> GetTempUserIdByTask(int taskId)
        {

            return (from s in Context.TempUserTasks where s.TaskId == taskId select s).ToArray();
        }

        public void DeleteTasksUser(int id)
        {
            var deletableTempUserTask = Context.TempUserTasks.Find(id);

            Delete(deletableTempUserTask);

        }

        public void DeleteTempUserTasks(string userId)
        {
            var deletableTempUserTasks = from t in Context.TempUserTasks where t.UserId == userId select t;

            foreach (var item in deletableTempUserTasks)
            {
                Delete(item);
            }
        }

        public void CreateTasksUser(string userId, int[] taskIds)
        {
            foreach (var item in taskIds)
            {
                Add(new TempUserTask { TaskId = item, UserId = userId });
            }
        }
    }
}
