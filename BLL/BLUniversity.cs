using Repository.EF.Repository;
using System.Collections.Generic;
using System;
using Model;
using System.Linq.Expressions;
using BLL.Base;
using System.Linq;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Admin;

namespace BLL
{
    public class BLUniversity : BLBase
    {
        public IEnumerable<VmSelectListItem> GetUniversitySelectListItem(int index, int count)
        {
            var UniversityRepository = UnitOfWork.GetRepository<UniversityRepository>();

            var universityList = UniversityRepository.Select(index, count);
            var vmSelectListItem = (from university in universityList
                                    select new VmSelectListItem
                                    {
                                        Value = university.Id.ToString(),
                                        Text = university.Name,
                                    });

            return vmSelectListItem;
        }
        public bool UploadUniversityImage(int universityId, string universityPictureUrl)
        {
            try
            {

                var universityRepository = UnitOfWork.GetRepository<UniversityRepository>();
                universityRepository.UpdateUniversityImage(universityId, universityPictureUrl);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<VmUniversity> GetUniversitysByFilter(VmUniversity filterItem)
        {
            var universityRepository = UnitOfWork.GetRepository<UniversityRepository>();

            var viewFilterItem = new University
            {
                Id = filterItem.Id,
                Name= filterItem.Name,
            };

            var universityList = universityRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmUniversityList = from university in universityList
                                    select new VmUniversity
                                    {
                                        Id = university.Id,
                                        Name = university.Name,
                                    };
            return vmUniversityList;
        }

        public string GetUniversityPictureUrl(int id)
        {
            var UniversityRepository = UnitOfWork.GetRepository<UniversityRepository>();

            return UniversityRepository.GetUniversityById(id).UniversityPictureUrl;
        }

        public int CreateUniversity(VmUniversity vmUniversity)
        {
            var result = -1;
            try
            {
                var universityRepository = UnitOfWork.GetRepository<UniversityRepository>();

                var newUniversity = new University
                {
                    Id = vmUniversity.Id,
                    Name= vmUniversity.Name,
                };

                universityRepository.CreateUniversity(newUniversity);

                UnitOfWork.Commit();

                result = newUniversity.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateUniversity(VmUniversity vmUniversity)
        {

            var UniversityRepository = UnitOfWork.GetRepository<UniversityRepository>();

            var updateableUniversity = new University
            {
                Id = vmUniversity.Id,
                Name = vmUniversity.Name,
            };

            UniversityRepository.UpdateUniversity(updateableUniversity);

            return UnitOfWork.Commit();

        }
        public bool DeleteUniversity(int UniversityId)
        {
            try
            {
                var UniversityRepository = UnitOfWork.GetRepository<UniversityRepository>();


                UniversityRepository.DeleteUniversity(UniversityId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

    }
}
