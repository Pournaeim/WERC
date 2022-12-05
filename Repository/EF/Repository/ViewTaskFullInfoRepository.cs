using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using Repository.Core;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewTaskFullInfoRepository : EFBaseRepository<ViewTaskFullInfo>, IModelPaged<ViewTaskFullInfo>
    {
        public IEnumerable<ViewTaskFullInfo> EntityList { get; set; }
        public int Count(Func<ViewTaskFullInfo, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewTaskFullInfo> Select(int index = 0, int count = int.MaxValue)
        {
            var TaskFullInfoList = from TaskFullInfo in Context.ViewTaskFullInfoes
                                    select TaskFullInfo;

            return TaskFullInfoList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewTaskFullInfo> Select(Func<ViewTaskFullInfo, bool> predicate, int index, int count)
        {
            var TaskFullInfoList = (from TaskFullInfo in Context.ViewTaskFullInfoes
                                     select TaskFullInfo).Where(predicate);

            return TaskFullInfoList.Skip(index).Take(count).ToArray();
        }
        public ViewTaskFullInfo GetTaskFullInfoById(int id)
        {
            var TaskFullInfo = Context.ViewTaskFullInfoes.SingleOrDefault(a => a.Id == id);

            return TaskFullInfo;
        }
        public IEnumerable<ViewTaskFullInfo> GetTaskFullInfos(int[] TaskFullInfoIds)
        {
            return (from a in Context.ViewTaskFullInfoes where TaskFullInfoIds.Contains(a.Id) select a).ToArray();
        }

        public IEnumerable<ViewTaskFullInfo> Select(ViewTaskFullInfo filterItem, int index, int count)
        {
            var TaskFullInfoList = from TaskFullInfo in Context.ViewTaskFullInfoes
                                    select TaskFullInfo;

            if (filterItem.Name != null)
            {
                TaskFullInfoList = TaskFullInfoList.Where(t => t.Name.ToLower().Contains(filterItem.Name.ToLower()));
            }

            if (filterItem.Grades != null)
            {
                TaskFullInfoList = TaskFullInfoList.Where(t => t.Grades.ToLower().Contains(filterItem.Grades.ToLower()));
            }

            if (filterItem.Judges != null)
            {
                TaskFullInfoList = TaskFullInfoList.Where(t => t.Judges.ToLower().Contains(filterItem.Judges.ToLower()));
            }

            if (filterItem.Description != null)
            {
                TaskFullInfoList = TaskFullInfoList.Where(t => t.Description.ToLower().Contains(filterItem.Description.ToLower()));
            }

            return TaskFullInfoList.OrderBy(t => t.Name).Skip(index).Take(count).ToArray();

        }

    }
}
