using BLL.Base;

using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.LevelOfConfidence;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLLevelOfConfidence : BLBase
    {

        public IEnumerable<VmSelectListItem> GetLevelOfConfidenceSelectListItem(int index, int count)
        {
            var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

            var levelOfConfidenceList = levelOfConfidenceRepository.Select(index, count);
            var vmSelectListItem = (from levelOfConfidence in levelOfConfidenceList
                                    select new VmSelectListItem
                                    {
                                        Value = levelOfConfidence.Id.ToString(),
                                        Text = levelOfConfidence.Name,
                                    });

            return vmSelectListItem;
        }

        public List<VmLevelOfConfidence> GetAllLevelOfConfidence()
        {
            var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

            var levelOfConfidenceList = levelOfConfidenceRepository.Select();
            var vmLevelOfConfidenceList = (from levelOfConfidence in levelOfConfidenceList
                                  select new VmLevelOfConfidence
                                  {
                                      Id = levelOfConfidence.Id,

                                      Name = levelOfConfidence.Name,
                                  }).ToList();
            return vmLevelOfConfidenceList;
        }
        public VmLevelOfConfidence GetLevelOfConfidenceById(int id)
        {
            var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

            var levelOfConfidence = levelOfConfidenceRepository.GetLevelOfConfidenceById(id);
            var vmLevelOfConfidence = new VmLevelOfConfidence
            {
                Id = levelOfConfidence.Id,

                Name = levelOfConfidence.Name,
            };

            return vmLevelOfConfidence;
        }
        public IEnumerable<VmLevelOfConfidence> GetLevelOfConfidencesByFilter(VmLevelOfConfidence filterItem)
        {
            var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

            var viewFilterItem = new LevelOfConfidence
            {
                Id = filterItem.Id,

                Name = filterItem.Name,
            };

            var levelOfConfidenceList = levelOfConfidenceRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmLevelOfConfidenceList = from levelOfConfidence in levelOfConfidenceList
                                 select new VmLevelOfConfidence
                                 {
                                     Id = levelOfConfidence.Id,

                                     Name = levelOfConfidence.Name,
                                 };
            return vmLevelOfConfidenceList;
        }

        public int CreateLevelOfConfidence(VmLevelOfConfidence vmLevelOfConfidence)
        {
            var result = -1;
            try
            {
                var levelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

                var newLevelOfConfidence = new LevelOfConfidence
                {
                    Id = vmLevelOfConfidence.Id,

                    Name = vmLevelOfConfidence.Name,
                };

                levelOfConfidenceRepository.CreateLevelOfConfidence(newLevelOfConfidence);

                UnitOfWork.Commit();

                result = newLevelOfConfidence.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateLevelOfConfidence(VmLevelOfConfidence vmLevelOfConfidence)
        {

            var LevelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();

            var updateableLevelOfConfidence = new LevelOfConfidence
            {
                Id = vmLevelOfConfidence.Id,
                Name = vmLevelOfConfidence.Name,

            };

            LevelOfConfidenceRepository.UpdateLevelOfConfidence(updateableLevelOfConfidence);

            return UnitOfWork.Commit();

        }
        public bool DeleteLevelOfConfidence(int LevelOfConfidenceId)
        {
            try
            {
                var LevelOfConfidenceRepository = UnitOfWork.GetRepository<LevelOfConfidenceRepository>();


                LevelOfConfidenceRepository.DeleteLevelOfConfidence(LevelOfConfidenceId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

    }
}
