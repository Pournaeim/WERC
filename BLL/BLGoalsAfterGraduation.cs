using Repository.EF.Repository;
using System.Collections.Generic;
using System;
using Model;
using System.Linq.Expressions;
using BLL.Base;
using System.Linq;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.GoalsAfterGraduation;

namespace BLL
{
    public class BLGoalsAfterGraduation : BLBase
    {
        public List<VmGoalsAfterGraduation> GetAllGoalsAfterGraduation()
        {
            var viewGoalsAfterGraduationRepository = UnitOfWork.GetRepository<GoalsAfterGraduationRepository>();

            var goalsAfterGraduationList = viewGoalsAfterGraduationRepository.Select(0, int.MaxValue);

            var vmGoalsAfterGraduationList =(from goalsAfterGraduation in goalsAfterGraduationList
                             
                             select new VmGoalsAfterGraduation
                             {
                                 Id = goalsAfterGraduation.Id,
                                 Name = goalsAfterGraduation.Name,
                                 OrderNo = goalsAfterGraduation.OrderNo,
                                 Display = goalsAfterGraduation.Display,

                             }).ToList();

            return vmGoalsAfterGraduationList;
        }
    }
}
