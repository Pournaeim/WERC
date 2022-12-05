using Repository.EF.Base;
using System.Linq;
using Model;
using System.Collections.Generic;
using System;

namespace Repository.EF.Repository
{
    public class TaskGradeRepository : EFBaseRepository<TaskGrade>
    {
        public void CreateTasksGrade(TaskGrade taskGrade)
        {
            Add(taskGrade);
        }
        public IEnumerable<TaskGrade> GetTaskGrades(int taskId)
        {
            return (from s in Context.TaskGrades where s.TaskId == taskId select s).ToArray();

        }
        public int[] GetTaskGradeIds(int taskId)
        {
            return (from s in Context.TaskGrades where s.TaskId == taskId select s.GradeId).ToArray();

        }
        public void DeleteTasksGrades(int taskId)
        {
            var deletableTaskGrades = from s in Context.TaskGrades where s.TaskId == taskId select s;

            foreach (var item in deletableTaskGrades)
            {
                Delete(item);
            }

        }
        public void CreateTasksGrade(int taskId, int[] gradeIds)
        {
            foreach (var item in gradeIds)
            {
                Add(new TaskGrade { TaskId = taskId, GradeId = item });
            }
        }
    }
}
