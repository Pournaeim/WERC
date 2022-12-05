using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TaskRepository : EFBaseRepository<Task>
    {
        public int GetTaskCount()
        {
            return Context.Tasks.Count();
        }
        public IEnumerable<Task> Select(int index, int count)
        {
            var TaskList = from Task in Context.Tasks
                           select Task;

            return TaskList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }

        public IEnumerable<Task> GetTasks(string taskName = "")
        {

            var taskList = from task in Context.Tasks
                           select task;

            if (taskName != "")
            {
                taskList = taskList.Where(t => t.Name.Contains(taskName));
            }

            return taskList.ToArray();
        }

        public void CreateTask(Task newTask)
        {
            Add(newTask);
        }
        public void UpdateTask(Task updateableTask)
        {
            var oldTask = (from s in Context.Tasks where s.Id == updateableTask.Id select s).FirstOrDefault();

            oldTask.Name = updateableTask.Name;
            if (!string.IsNullOrEmpty(updateableTask.ImageUrl))
            {
                oldTask.ImageUrl = updateableTask.ImageUrl;
            }

            oldTask.TaskGrades = updateableTask.TaskGrades;
            oldTask.TaskTests = updateableTask.TaskTests;
            oldTask.Description = updateableTask.Description;
            oldTask.Preliminary = updateableTask.Preliminary;
            oldTask.RegisterForFlashTalk = updateableTask.RegisterForFlashTalk;
            oldTask.OpenTaskTestPlan = updateableTask.OpenTaskTestPlan;
            oldTask.PaymentTypeId = updateableTask.PaymentTypeId;

            Update(oldTask);
        }
        public bool DeleteTask(int taskId)
        {
            var oldTask = (from s in Context.Tasks where s.Id == taskId select s).FirstOrDefault();
            if (oldTask.UserTasks.Count() > 0)
            {
                return false;
            }

            Delete(oldTask);

            return true;
        }
        public bool DeleteAllTask()
        {
            var allTask = Context.Tasks;

            foreach (var item in allTask)
            {
                Delete(item);
            }

            return true;
        }

        public Task GetTaskById(int id)
        {
            return Context.Tasks.Find(id);

        }
    }
}
