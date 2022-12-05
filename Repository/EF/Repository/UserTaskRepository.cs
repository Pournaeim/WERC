using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class UserTaskRepository : EFBaseRepository<UserTask>
    {
        public void CreateTasksUser(UserTask userTask)
        {
            Add(userTask);
        }
        public IEnumerable<UserTask> GetUserTasks(string userId)
        {
            return (from s in Context.UserTasks where s.UserId == userId select s).ToArray();

        }
        public int[] GetUserTaskIds(string userId)
        {
            return (from s in Context.UserTasks where s.UserId == userId orderby s.Id select s.TaskId).ToArray<int>();

        }
        public string GetUserIdByTask(int taskId)
        {
            return (from s in Context.UserTasks where s.TaskId == taskId select s).SingleOrDefault().UserId;

        }
         public IEnumerable<UserTask> GetUserTaskById(int taskId)
        {
            return (from s in Context.UserTasks where s.TaskId == taskId select s).ToArray();

        }

        public void DeleteTasksUser(int id)
        {
            var deletableUserTask = Context.UserTasks.Find(id);

            Delete(deletableUserTask);

        }

        public void DeleteUserTasks(string userId)
        {
            var deletableUserTasks = from t in Context.UserTasks where t.UserId == userId select t;

            foreach (var item in deletableUserTasks)
            {
                Delete(item);
            }
        }

        public void CreateTasksUser(string userId, int[] taskIds)
        {
            foreach (var item in taskIds)
            {
                Add(new UserTask { TaskId = item, UserId = userId });
            }
        }
    }
}
