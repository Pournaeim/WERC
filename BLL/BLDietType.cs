using Repository.EF.Repository;
using System.Collections.Generic;
using System;
using Model;
using System.Linq.Expressions;
using BLL.Base;
using System.Linq;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.DietType;

namespace BLL
{
    public class BLDietType : BLBase
    {
        public IEnumerable<VmSelectListItem> GetDietTypeSelectListItem(int index, int count, bool display = true)
        {
            var DietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

            var DietTypeList = DietTypeRepository.Select(index, count, display);
            var vmSelectListItem = (from DietType in DietTypeList
                                    select new VmSelectListItem
                                    {
                                        Value = DietType.Id.ToString(),
                                        Text = DietType.Name,
                                    });

            return vmSelectListItem;
        }

        public VmDietType GetDietTypeById(int id)
        {
            var DietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

            var DietType = DietTypeRepository.GetDietTypeById(id);

            var vmDietType = new VmDietType
            {
                Id = DietType.Id,
                Name = DietType.Name,
            };

            return vmDietType;
        }

        public IEnumerable<VmDietType> GetAllDietType()
        {
            var DietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

            var DietTypeList = DietTypeRepository.Select(0, int.MaxValue, true);
            var vmDietTypeList = from DietType in DietTypeList
                                 select new VmDietType
                                 {
                                     Id = DietType.Id,
                                     Name = DietType.Name,
                                 };

            return vmDietTypeList;
        }

        public int CreateDietType(VmDietType vmDietType)
        {
            var result = -1;
            try
            {
                var dietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

                var newDietType = new DietType
                {
                    Id = dietTypeRepository.GetDietTypeNewId(),
                    Name = vmDietType.Name,
                    Display = vmDietType.Display,
                };

                dietTypeRepository.CreateDietType(newDietType);

                UnitOfWork.Commit();

                result = newDietType.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateDietType(VmDietType vmDietType)
        {
            try
            {
                var DietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();

                var updateableDietType = new DietType
                {
                    Id = vmDietType.Id,
                    Name = vmDietType.Name,
                    Display = vmDietType.Display,
                };

                DietTypeRepository.UpdateDietType(updateableDietType);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteDietType(int DietTypeId)
        {
            try
            {
                var DietTypeRepository = UnitOfWork.GetRepository<DietTypeRepository>();


                if (DietTypeRepository.DeleteDietType(DietTypeId) == true)
                {
                    return UnitOfWork.Commit();
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

    }
}
