using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using Repository.Core;

namespace Repository.EF.Repository
{
    public class ViewGradeDetailRepository : EFBaseRepository<GradeDetail>, IModelPaged<ViewGradeDetail>
    {
        public IEnumerable<ViewGradeDetail> EntityList { get; set; }
        public int Count(Func<ViewGradeDetail, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewGradeDetail> Select(int index, int count)
        {
            var gradePageList = from gradePage in Context.ViewGradeDetails
                                  select gradePage;

            return gradePageList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewGradeDetail> Select(Func<ViewGradeDetail, bool> predicate, int index, int count)
        {
            var gradePageList = (from gradePage in Context.ViewGradeDetails
                                   select gradePage).Where(predicate);

            return gradePageList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewGradeDetail> GetGradeDetailsByName(string name, bool like)
        {

            var gradePageList = from gradePage in Context.ViewGradeDetails
                                  select gradePage;

            if (like)
            {
                gradePageList = gradePageList.Where(g => g.Name.Contains(name));
            }
            else
            {
                gradePageList = gradePageList.Where(g => g.Name == name);

            }
            return gradePageList.ToArray();
        }
        public IEnumerable<ViewGradeDetail> Select(ViewGradeDetail filterItem, int index, int count)
        {
            var gradePageList = from gradePage in Context.ViewGradeDetails
                                  select gradePage;


            if (filterItem.Name != null)
            {
                gradePageList = gradePageList.Where(g => g.Name.Contains(filterItem.Name));
            }
            if (filterItem.Name != null)
            {
                gradePageList = gradePageList.Where(g => g.Name.Contains(filterItem.Name));
            }

            if (filterItem.EvaluationItem != null)
            {
                gradePageList = gradePageList.Where(g => g.EvaluationItem.Contains(filterItem.EvaluationItem));
            }

            if (filterItem.Point != 0)
            {
                gradePageList = gradePageList.Where(g => g.Point == filterItem.Point);
            }
            if (filterItem.Coefficient != 0)
            {
                gradePageList = gradePageList.Where(g => g.Coefficient == filterItem.Coefficient);
            }

            return gradePageList.OrderBy(g => g.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewGradeDetail> GetGradeDetailsByGrade(int gradeId)
        {
            var gradePageList = from gradePage in Context.ViewGradeDetails
                                  where gradePage.GradeId == gradeId
                                  select gradePage;


            return gradePageList.ToArray();
        }
        public IEnumerable<ViewGradeDetail> GetAllGradeDetails()
        {
            var gradePageList = from gradePage in Context.ViewGradeDetails
                                  select gradePage;

            return gradePageList.ToArray();
        }
    }
}
