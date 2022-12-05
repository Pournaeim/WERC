using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.SystemSetting;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLSystemSetting : BLBase
    {

        public IEnumerable<VmSelectListItem> GetSystemSettingSelectListItem(int index, int count)
        {
            var systemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

            var systemSettingList = systemSettingRepository.Select(index, count);
            var vmSelectListItem = (from systemSetting in systemSettingList
                                    select new VmSelectListItem
                                    {
                                        Value = systemSetting.Id.ToString(),
                                        Text = systemSetting.Name,
                                    });

            return vmSelectListItem;
        }

        public List<VmSystemSetting> GetAllSystemSetting()
        {
            var systemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

            var systemSettingList = systemSettingRepository.Select();
            var vmSystemSettingList = (from systemSetting in systemSettingList
                                       select new VmSystemSetting
                                       {
                                           Id = systemSetting.Id,
                                           Active = systemSetting.Active,
                                           Name = systemSetting.Name,
                                       }).ToList();
            return vmSystemSettingList;
        }
        public VmSystemSetting GetSystemSettingById(int id)
        {
            var systemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

            var systemSetting = systemSettingRepository.GetSystemSettingById(id);
            var vmSystemSetting = new VmSystemSetting
            {
                Id = systemSetting.Id,
                Active = systemSetting.Active,
                Name = systemSetting.Name,
            };

            return vmSystemSetting;
        }
        public IEnumerable<VmSystemSetting> GetSystemSettingsByFilter(VmSystemSetting filterItem)
        {
            var systemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

            var viewFilterItem = new SystemSetting
            {
                Id = filterItem.Id,
                Active = filterItem.Active,
                Name = filterItem.Name,
            };

            var systemSettingList = systemSettingRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmSystemSettingList = from systemSetting in systemSettingList
                                      select new VmSystemSetting
                                      {
                                          Id = systemSetting.Id,
                                          Active = systemSetting.Active,
                                          Name = systemSetting.Name,
                                      };
            return vmSystemSettingList;
        }

        public int CreateSystemSetting(VmSystemSetting vmSystemSetting)
        {
            var result = -1;
            try
            {
                var systemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

                var newSystemSetting = new SystemSetting
                {
                    Id = vmSystemSetting.Id,
                    Active = vmSystemSetting.Active,
                    Name = vmSystemSetting.Name,
                };

                systemSettingRepository.CreateSystemSetting(newSystemSetting);

                UnitOfWork.Commit();

                result = newSystemSetting.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateSystemSetting(VmSystemSetting vmSystemSetting)
        {

            var SystemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();

            var updateableSystemSetting = new SystemSetting
            {
                Id = vmSystemSetting.Id,
                Name = vmSystemSetting.Name,
                Active = vmSystemSetting.Active,
            };

            SystemSettingRepository.UpdateSystemSetting(updateableSystemSetting);

            return UnitOfWork.Commit();

        }
        public bool DeleteSystemSetting(int SystemSettingId)
        {
            try
            {
                var SystemSettingRepository = UnitOfWork.GetRepository<SystemSettingRepository>();


                SystemSettingRepository.DeleteSystemSetting(SystemSettingId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

    }
}
