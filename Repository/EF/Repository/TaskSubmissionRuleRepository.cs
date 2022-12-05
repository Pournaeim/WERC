using Model;

using Repository.EF.Base;

using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class TaskSubmissionRuleRepository : EFBaseRepository<TaskSubmissionRule>
    {
        public void CreateTasksUser(TaskSubmissionRule taskSubmissionRule)
        {
            Add(taskSubmissionRule);
        }
        public IEnumerable<TaskSubmissionRule> GetTaskSubmissionRules(int taskId)
        {

            return (from s in Context.TaskSubmissionRules where s.TaskId == taskId select s).ToArray();
        }
        public int[] GetTaskSubmissionRuleIds(int taskId)
        {
            return (from s in Context.TaskSubmissionRules
                    where s.TaskId == taskId
                    orderby s.Id
                    select s.TaskId).ToArray<int>();

        }

        public void DeleteTaskSubmissionRules(int submissionRuleId)
        {
            var deletableTaskSubmissionRules = from t in Context.TaskSubmissionRules
                                               where t.SubmissionRuleId == submissionRuleId
                                               select t;

            foreach (var item in deletableTaskSubmissionRules)
            {
                Delete(item);
            }
        }

        public void CreateTaskSubmissionRules(int submissionRuleId, int[] taskIds)
        {
            foreach (var task in taskIds)
            {
                Add(new TaskSubmissionRule { SubmissionRuleId = submissionRuleId, TaskId = task });
            }
        }
    }
}
