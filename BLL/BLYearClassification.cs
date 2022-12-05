using Repository.EF.Repository;
using System.Collections.Generic;
using System;
using Model;
using System.Linq.Expressions;
using BLL.Base;
using System.Linq;
using Model.ToolsModels.DropDownList;

namespace BLL
{
    public class BLYearClassification : BLBase
    {
        public IEnumerable<VmSelectListItem>  GetYearClassificationSelectListItem(int index, int count)
        {
                var YearClassificationRepository = UnitOfWork.GetRepository<YearClassificationRepository>();

                var yearClassificationList = YearClassificationRepository.Select(index, count);
                var vmSelectListItem = (from yearClassification in yearClassificationList
                                        select new VmSelectListItem
                                        {
                                            Value = yearClassification.Id.ToString(),
                                            Text = yearClassification.Name,
                                        });

                return vmSelectListItem;
            }
    }
}
