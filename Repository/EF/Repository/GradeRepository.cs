using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class GradeRepository : EFBaseRepository<Grade>
    {
        public IEnumerable<Grade> Select(int index, int count)
        {
            var GradeList = from Grade in Context.Grades
                            select Grade;

            return GradeList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Grade> GetGrades(string gradeName = "")
        {

            var gradeList = from grade in Context.Grades
                            select grade;

            if (gradeName != "")
            {
                gradeList = gradeList.Where(t => t.Name.Contains(gradeName));
            }

            return gradeList.ToArray();
        }

        public void CreateGrade(Grade newGrade)
        {
            Add(newGrade);
        }
        public void UpdateGrade(Grade updateableGrade)
        {
            var oldGrade = (from s in Context.Grades where s.Id == updateableGrade.Id select s).FirstOrDefault();

            oldGrade.Name = updateableGrade.Name;
            oldGrade.ABET = updateableGrade.ABET;
            Update(oldGrade);
        }
        public bool DeleteGrade(int gradeId)
        {
            var oldGrade = Context.Grades.Find(gradeId);          
            Delete(oldGrade);

            return true;

        }

        public Grade GetGradeById(int id)
        {
            return Context.Grades.Find(id);

        }
    }
}
