using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.MealType;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLMealType : BLBase
    {

        public IEnumerable<VmSelectListItem> GetMealTypeSelectListItem(int index, int count)
        {
            var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

            var mealTypeList = mealTypeRepository.Select(index, count);
            var vmSelectListItem = (from mealType in mealTypeList
                                    select new VmSelectListItem
                                    {
                                        Value = mealType.Id.ToString(),
                                        Text = mealType.Name,
                                    });

            return vmSelectListItem;
        }

        public List<VmMealType> GetAllMealType()
        {
            var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

            var mealTypeList = mealTypeRepository.Select();
            var vmMealTypeList = (from mealType in mealTypeList
                                  select new VmMealType
                                  {
                                      Id = mealType.Id,

                                      Name = mealType.Name,
                                  }).ToList();
            return vmMealTypeList;
        }
        public VmMealType GetMealTypeById(int id)
        {
            var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

            var mealType = mealTypeRepository.GetMealTypeById(id);
            var vmMealType = new VmMealType
            {
                Id = mealType.Id,

                Name = mealType.Name,
            };

            return vmMealType;
        }
        public IEnumerable<VmMealType> GetMealTypesByFilter(VmMealType filterItem)
        {
            var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

            var viewFilterItem = new MealType
            {
                Id = filterItem.Id,

                Name = filterItem.Name,
            };

            var mealTypeList = mealTypeRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmMealTypeList = from mealType in mealTypeList
                                 select new VmMealType
                                 {
                                     Id = mealType.Id,

                                     Name = mealType.Name,
                                 };
            return vmMealTypeList;
        }

        public int CreateMealType(VmMealType vmMealType)
        {
            var result = -1;
            try
            {
                var mealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

                var newMealType = new MealType
                {
                    Id = vmMealType.Id,

                    Name = vmMealType.Name,
                };

                mealTypeRepository.CreateMealType(newMealType);

                UnitOfWork.Commit();

                result = newMealType.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateMealType(VmMealType vmMealType)
        {

            var MealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();

            var updateableMealType = new MealType
            {
                Id = vmMealType.Id,
                Name = vmMealType.Name,

            };

            MealTypeRepository.UpdateMealType(updateableMealType);

            return UnitOfWork.Commit();

        }
        public bool DeleteMealType(int MealTypeId)
        {
            try
            {
                var MealTypeRepository = UnitOfWork.GetRepository<MealTypeRepository>();


                MealTypeRepository.DeleteMealType(MealTypeId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

    }
}
