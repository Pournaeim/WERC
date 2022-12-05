using BLL.Base;
using Model;
using Model.ViewModels.Task;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLTempUserTask : BLBase
    {

        public IEnumerable<VmUserTask> GetAlluserTasks()
        {
            var viewTempUserTaskRepository = UnitOfWork.GetRepository<ViewTempUserTaskRepository>();
            int i = 0;
            var viewTempUserTaskList = viewTempUserTaskRepository.GetAlluserTasks();
            var vmTaskList = from Task in viewTempUserTaskList
                             select new VmUserTask
                             {
                                 Id = Task.TaskId,
                                 Name = Task.Name,
                                 TaskName = Task.TaskName,
                                 UserId = Task.UserId,
                                 FirstName = Task.FirstName,
                                 LastName = Task.LastName,
                             };

            return vmTaskList;
        }
        public IEnumerable<VmTask> GetTempUserTasksByUser(string id)
        {
            var viewTempUserTaskRepository = UnitOfWork.GetRepository<ViewTempUserTaskRepository>();
            int i = 0;
            var viewTempUserTaskList = viewTempUserTaskRepository.GetTasksByUser(id);
            var vmTaskList = from Task in viewTempUserTaskList
                             let priority = ++i
                             select new VmTask
                             {
                                 Id = Task.TaskId,
                                 Name = Task.TaskName,
                                 ImageUrl = Task.ImageUrl,
                                 Priority = i 
                             };

            return vmTaskList;
        }

        public IEnumerable<string> GetUsersByTask(int taskId)
        {
            var viewTempUserTaskRepository = UnitOfWork.GetRepository<ViewTempUserTaskRepository>();
            var userIds = viewTempUserTaskRepository.GetUsersByTask(taskId);


            return userIds;
        }

        public IEnumerable<VmUserTask> GetTempUserTasksByUsers(string[] ids)
        {
            var viewTempUserTaskRepository = UnitOfWork.GetRepository<ViewTempUserTaskRepository>();

            var viewTempUserTaskList = viewTempUserTaskRepository.GetTasksByUsers(ids);
            var vmTaskList = from Task in viewTempUserTaskList
                             select new VmUserTask
                             {
                                 Id = Task.TaskId,
                                 Name = Task.Name,
                                 TaskName = Task.TaskName,
                                 UserId = Task.UserId
                             };

            return vmTaskList;
        }

        public bool AssignTasksToUser(string userId, int[] taskIds)
        {
            try
            {
                var tempUserTaskRepository = UnitOfWork.GetRepository<TempUserTaskRepository>();

                var newAssignedTasks = new List<int>();

                var oldTempUserTasks = tempUserTaskRepository.GetTempUserTasks(userId);

                if (taskIds != null)
                {
                    foreach (var item in taskIds)
                    {
                        if (oldTempUserTasks.Where(a => a.Id == item).Count() == 0)
                        {
                            newAssignedTasks.Add(item);
                        }
                    }
                }

                foreach (var item in oldTempUserTasks)
                {
                    if (taskIds != null && taskIds.Contains(item.Id) == false)
                    {
                        tempUserTaskRepository.DeleteTasksUser(item.Id);
                    }
                    else
                    if (taskIds == null)
                    {
                        tempUserTaskRepository.DeleteTasksUser(item.Id);
                    }
                }

                if (newAssignedTasks.Count > 0)
                {
                    foreach (var item in newAssignedTasks)
                    {
                        tempUserTaskRepository.CreateTasksUser(
                            new TempUserTask
                            {
                                TaskId = item,
                                UserId = userId,
                            });
                    }

                }

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}