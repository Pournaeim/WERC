using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewTempUserTaskRepository : EFBaseRepository<ViewTempUserTask>
    {
        public IEnumerable<ViewTempUserTask> GetAlluserTasks()
        {
            var viewTempUserTaskList = from ut in Context.ViewTempUserTasks
                                       select ut;

            return viewTempUserTaskList.OrderBy(u => u.Id).ToArray();
        }
        public IEnumerable<ViewTempUserTask> GetTasksByUser(string userId)
        {
            var viewTempUserTaskList = from ut in Context.ViewTempUserTasks
                                       where ut.UserId == userId
                                       select ut;

            return viewTempUserTaskList.ToArray();
        }
        public IEnumerable<ViewTempUserTask> GetTasksByRole(SystemRoles roleName)
        {
            var rolesearchName = GetSystemRolesString(roleName);
            var viewTempUserTaskList = from ut in Context.ViewTempUserTasks
                                       where ut.RoleName == rolesearchName
                                       select ut;

            return viewTempUserTaskList.ToArray();
        }

        public IEnumerable<string> GetUsersByTask(int taskId)
        {
            var viewTempUserTaskList = from ut in Context.ViewTempUserTasks
                                       where ut.TaskId == taskId
                                       select ut;

            return viewTempUserTaskList.Select(u => u.UserId).ToArray();
        }

        public IEnumerable<ViewTempUserTask> GetTasksByUsers(string[] userIds)
        {
            var viewTempUserTaskList = from ut in Context.ViewTempUserTasks
                                       where userIds.Contains(ut.UserId)
                                       select ut;

            return viewTempUserTaskList.ToArray();
        }

    }
}
