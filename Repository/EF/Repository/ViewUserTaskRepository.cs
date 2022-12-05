using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewUserTaskRepository : EFBaseRepository<ViewUserTask>
    {
        public IEnumerable<ViewUserTask> GetAllUserTasks()
        {
            var viewUserTaskList = from ut in Context.ViewUserTasks
                                   select ut;

            return viewUserTaskList.OrderBy(u=>u.Id).ToArray();
        }
        public IEnumerable<ViewUserTask> GetTasksByUser(string userId)
        {
            var viewUserTaskList = from ut in Context.ViewUserTasks
                                   where ut.UserId == userId
                                   select ut;

            return viewUserTaskList.ToArray();
        }
         public IEnumerable<ViewUserTask> GetTasksByRole(SystemRoles roleName)
        {
            var rolesearchName = GetSystemRolesString(roleName);
            var viewUserTaskList = from ut in Context.ViewUserTasks
                                   where ut.RoleName == rolesearchName
                                   select ut;

            return viewUserTaskList.ToArray();
        }

        public IEnumerable<string> GetUsersByTask(int taskId)
        {
            var viewUserTaskList = from ut in Context.ViewUserTasks
                                   where ut.TaskId == taskId
                                   select ut;

            return viewUserTaskList.Select(u => u.UserId).ToArray();
        }

        public IEnumerable<ViewUserTask> GetTasksByUsers(string[] userIds)
        {
            var viewUserTaskList = from ut in Context.ViewUserTasks
                                   where userIds.Contains(ut.UserId)
                                   select ut;

            return viewUserTaskList.ToArray();
        }

    }
}
