using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class SystemSettingRepository : EFBaseRepository<SystemSetting>
    {

        public void CreateSystemSetting(SystemSetting newSystemSetting)
        {
            Add(newSystemSetting);
        }
        public void UpdateSystemSetting(SystemSetting updateableSystemSetting)
        {
            var oldSystemSetting = (from s in Context.SystemSettings where s.Id == updateableSystemSetting.Id select s).FirstOrDefault();

            oldSystemSetting.Active = updateableSystemSetting.Active;
            oldSystemSetting.Name = updateableSystemSetting.Name;

            Update(oldSystemSetting);
        }
        public void DeleteSystemSetting(int SystemSettingId)
        {
            var oldSystemSetting = (from s in Context.SystemSettings where s.Id == SystemSettingId select s).FirstOrDefault();
            Delete(oldSystemSetting);
        }

        public IEnumerable<SystemSetting> EntityList { get; set; }
        public int Count(Func<SystemSetting, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<SystemSetting> Select(int index = 0, int count = int.MaxValue)
        {
            var systemSettingList = from systemSetting in Context.SystemSettings
                                    select systemSetting;

            return systemSettingList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<SystemSetting> Select(Func<SystemSetting, bool> predicate, int index, int count)
        {
            var systemSettingList = (from systemSetting in Context.SystemSettings
                                     select systemSetting).Where(predicate);

            return systemSettingList.Skip(index).Take(count).ToArray();
        }
        public SystemSetting GetSystemSettingById(int id)
        {
            var systemSetting = Context.SystemSettings.SingleOrDefault(a => a.Id == id);

            return systemSetting;
        }
        public IEnumerable<SystemSetting> Select(SystemSetting filterItem, int index, int count)
        {
            var systemSettingList = from systemSetting in Context.SystemSettings
                                    select systemSetting;

            return systemSettingList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
