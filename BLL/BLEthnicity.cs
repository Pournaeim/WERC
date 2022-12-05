using Repository.EF.Repository;
using System.Collections.Generic;
using System;
using Model;
using System.Linq.Expressions;
using BLL.Base;
using System.Linq;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Ethnicity;

namespace BLL
{
    public class BLEthnicity : BLBase
    {
        public List<VmEthnicity> GetAllEthnicity()
        {
            var viewEthnicityRepository = UnitOfWork.GetRepository<EthnicityRepository>();

            var ethnicityList = viewEthnicityRepository.Select(0, int.MaxValue);

            var vmEthnicityList =(from ethnicity in ethnicityList
                             
                             select new VmEthnicity
                             {
                                 Id = ethnicity.Id,
                                 Name = ethnicity.Name,
                                 OrderNo = ethnicity.OrderNo,
                                 Display = ethnicity.Display,

                             }).ToList();

            return vmEthnicityList;
        }
    }
}
