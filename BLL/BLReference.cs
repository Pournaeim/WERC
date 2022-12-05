using BLL.Base;
using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.Reference;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLReference : BLBase
    {
        public IEnumerable<VmSelectListItem> GetReferenceSelectListItem(int index, int count)
        {
            var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

            var referenceList = referenceRepository.Select(index, count);
            var vmSelectListItem = (from reference in referenceList
                                    select new VmSelectListItem
                                    {
                                        Value = reference.Id.ToString(),
                                        Text = reference.Title,
                                    });

            return vmSelectListItem;
        }

        public IEnumerable<VmReference> GetReferenceList(string referenceTitle = "")
        {
            var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

            var referenceList = referenceRepository.GetReferences(referenceTitle);

            var vmReferenceList = from reference in referenceList

                                  select new VmReference
                                  {
                                      Id = reference.Id,
                                      Title = reference.Title,
                                      ReferenceFileUrl = reference.ReferenceFileUrl,
                                  };

            return vmReferenceList;
        }
        public VmReference GetReferenceById(int id)
        {
            var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

            var reference = referenceRepository.GetReferenceById(id);

            var vmReference = new VmReference
            {
                Id = reference.Id,
                Title = reference.Title,
                ReferenceFileUrl = reference.ReferenceFileUrl,
            };

            return vmReference;
        }
        public VmReferenceCollection GetAllReference()
        {
            var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

            var referenceList = referenceRepository.Select(0, int.MaxValue);

            var vmReferenceList = from reference in referenceList

                                  select new VmReference
                                  {
                                      Id = reference.Id,
                                      Title = reference.Title,
                                      ReferenceFileUrl = reference.ReferenceFileUrl,
                                  };
            var vmReferenceCollection = new VmReferenceCollection
            {
                ReferenceList = vmReferenceList.ToList()
            };

            return vmReferenceCollection;
        }
        public int CreateReference(VmReference vmReference)
        {
            var result = -1;
            try
            {
                var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

                var newReference = new Reference
                {
                    Title = vmReference.Title,
                    ReferenceFileUrl = vmReference.ReferenceFileUrl,
                };

                referenceRepository.CreateReference(newReference);

                UnitOfWork.Commit();

                result = newReference.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateReference(VmReference vmReference)
        {
            try
            {
                var referenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();

                var updateableReference = new Reference
                {
                    Id = vmReference.Id,
                    Title = vmReference.Title,
                    ReferenceFileUrl = vmReference.ReferenceFileUrl,
                };

                referenceRepository.UpdateReference(updateableReference);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteReference(int referenceId)
        {
            try
            {
                var ReferenceRepository = UnitOfWork.GetRepository<ReferenceRepository>();


                if (ReferenceRepository.DeleteReference(referenceId) == true)
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
