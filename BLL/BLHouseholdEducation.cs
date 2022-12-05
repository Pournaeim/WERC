using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.HouseholdEducation;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLHouseholdEducation : BLBase
    {

        public IEnumerable<VmSelectListItem> GetHouseholdEducationSelectListItem(int index, int count)
        {
            var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

            var householdEducationList = householdEducationRepository.Select(index, count);
            var vmSelectListItem = (from householdEducation in householdEducationList
                                    select new VmSelectListItem
                                    {
                                        Value = householdEducation.Id.ToString(),
                                        Text = householdEducation.Name,
                                    });

            return vmSelectListItem;
        }

        public List<VmHouseholdEducation> GetAllHouseholdEducation()
        {
            var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

            var householdEducationList = householdEducationRepository.Select();
            var vmHouseholdEducationList = (from householdEducation in householdEducationList
                                  select new VmHouseholdEducation
                                  {
                                      Id = householdEducation.Id,

                                      Name = householdEducation.Name,
                                  }).ToList();
            return vmHouseholdEducationList;
        }
        public VmHouseholdEducation GetHouseholdEducationById(int id)
        {
            var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

            var householdEducation = householdEducationRepository.GetHouseholdEducationById(id);
            var vmHouseholdEducation = new VmHouseholdEducation
            {
                Id = householdEducation.Id,

                Name = householdEducation.Name,
            };

            return vmHouseholdEducation;
        }
        public IEnumerable<VmHouseholdEducation> GetHouseholdEducationsByFilter(VmHouseholdEducation filterItem)
        {
            var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

            var viewFilterItem = new HouseholdEducation
            {
                Id = filterItem.Id,

                Name = filterItem.Name,
            };

            var householdEducationList = householdEducationRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmHouseholdEducationList = from householdEducation in householdEducationList
                                 select new VmHouseholdEducation
                                 {
                                     Id = householdEducation.Id,

                                     Name = householdEducation.Name,
                                 };
            return vmHouseholdEducationList;
        }

        public int CreateHouseholdEducation(VmHouseholdEducation vmHouseholdEducation)
        {
            var result = -1;
            try
            {
                var householdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

                var newHouseholdEducation = new HouseholdEducation
                {
                    Id = vmHouseholdEducation.Id,

                    Name = vmHouseholdEducation.Name,
                };

                householdEducationRepository.CreateHouseholdEducation(newHouseholdEducation);

                UnitOfWork.Commit();

                result = newHouseholdEducation.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateHouseholdEducation(VmHouseholdEducation vmHouseholdEducation)
        {

            var HouseholdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();

            var updateableHouseholdEducation = new HouseholdEducation
            {
                Id = vmHouseholdEducation.Id,
                Name = vmHouseholdEducation.Name,

            };

            HouseholdEducationRepository.UpdateHouseholdEducation(updateableHouseholdEducation);

            return UnitOfWork.Commit();

        }
        public bool DeleteHouseholdEducation(int HouseholdEducationId)
        {
            try
            {
                var HouseholdEducationRepository = UnitOfWork.GetRepository<HouseholdEducationRepository>();


                HouseholdEducationRepository.DeleteHouseholdEducation(HouseholdEducationId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

    }
}
