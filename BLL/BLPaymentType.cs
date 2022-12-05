using BLL.Base;
using Model;
using Model.ToolsModels.DropDownList;
using Model.ViewModels.PaymentType;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLPaymentType : BLBase
    {

        public IEnumerable<VmSelectListItem> GetPaymentTypeSelectListItem(int index, int count)
        {
            var paymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

            var paymentTypeList = paymentTypeRepository.Select(index, count);
            var vmSelectListItem = (from paymentType in paymentTypeList
                                    select new VmSelectListItem
                                    {
                                        Value = paymentType.Id.ToString(),
                                        Text = paymentType.Name,
                                    });

            return vmSelectListItem;
        }

        public List<VmPaymentType> GetAllPaymentType()
        {
            var paymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

            var paymentTypeList = paymentTypeRepository.Select();
            var vmPaymentTypeList = (from paymentType in paymentTypeList
                                       select new VmPaymentType
                                       {
                                           Id = paymentType.Id,
                                           Description = paymentType.Description,
                                           Name = paymentType.Name,
                                       }).ToList();
            return vmPaymentTypeList;
        }
        public VmPaymentType GetPaymentTypeById(int id)
        {
            var paymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

            var paymentType = paymentTypeRepository.GetPaymentTypeById(id);
            var vmPaymentType = new VmPaymentType
            {
                Id = paymentType.Id,
                Description = paymentType.Description,
                Name = paymentType.Name,
            };

            return vmPaymentType;
        }
         public IEnumerable<VmPaymentType> GetPaymentTypesByFilter(VmPaymentType filterItem)
        {
            var paymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

            var viewFilterItem = new PaymentType
            {
                Id = filterItem.Id,
                Description = filterItem.Description,
                Name = filterItem.Name,
            };

            var paymentTypeList = paymentTypeRepository.Select(viewFilterItem, 0, int.MaxValue);

            var vmPaymentTypeList = from paymentType in paymentTypeList
                                       select new VmPaymentType
                                       {
                                           Id = paymentType.Id,
                                           Description = paymentType.Description,
                                           Name = paymentType.Name,
                                       };
            return vmPaymentTypeList;
        }

        public int CreatePaymentType(VmPaymentType vmPaymentType)
        {
            var result = -1;
            try
            {
                var paymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

                var newPaymentType = new PaymentType
                {
                    Id = vmPaymentType.Id,
                    Description = vmPaymentType.Description,
                    Name = vmPaymentType.Name,
                };

                paymentTypeRepository.CreatePaymentType(newPaymentType);

                UnitOfWork.Commit();

                result = newPaymentType.Id;

            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdatePaymentType(VmPaymentType vmPaymentType)
        {

            var PaymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();

            var updateablePaymentType = new PaymentType
            {
                Id = vmPaymentType.Id,
                Name = vmPaymentType.Name,
                Description = vmPaymentType.Description,
            };

            PaymentTypeRepository.UpdatePaymentType(updateablePaymentType);

            return UnitOfWork.Commit();

        }
        public bool DeletePaymentType(int PaymentTypeId)
        {
            try
            {
                var PaymentTypeRepository = UnitOfWork.GetRepository<PaymentTypeRepository>();


                PaymentTypeRepository.DeletePaymentType(PaymentTypeId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

     }
}
