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
    public class BLSize : BLBase
    {
        public IEnumerable<VmSelectListItem> GetSizeSelectListItem(int index, int count)
        {
            var SizeRepository = UnitOfWork.GetRepository<SizeRepository>();

            var sizeList = SizeRepository.Select(index, count);
            var vmSelectListItem = (from size in sizeList
                                    select new VmSelectListItem
                                    {
                                        Value = size.Id.ToString(),
                                        Text = size.Name,
                                    });

            return vmSelectListItem;
        }
     public IEnumerable<VmSelectListItem> GetTShirtSizeSelectListItem(int index, int count)
        {
            var SizeRepository = UnitOfWork.GetRepository<SizeRepository>();

            var sizeList = SizeRepository.Select(index, count);
            var vmSelectListItem = (from size in sizeList
                                    where size.Id != 8
                                    select new VmSelectListItem
                                    {
                                        Value = size.Id.ToString(),
                                        Text = size.Name,
                                    });

            return vmSelectListItem;
        }
    }
}
