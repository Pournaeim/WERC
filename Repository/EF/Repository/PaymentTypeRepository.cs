using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class PaymentTypeRepository : EFBaseRepository<PaymentType>
    {

        public void CreatePaymentType(PaymentType newPaymentType)
        {
            Add(newPaymentType);
        }
        public void UpdatePaymentType(PaymentType updateablePaymentType)
        {
            var oldPaymentType = (from s in Context.PaymentTypes where s.Id == updateablePaymentType.Id select s).FirstOrDefault();

            oldPaymentType.Description = updateablePaymentType.Description;
            oldPaymentType.Name = updateablePaymentType.Name;

            Update(oldPaymentType);
        }
        public void DeletePaymentType(int PaymentTypeId)
        {
            var oldPaymentType = (from s in Context.PaymentTypes where s.Id == PaymentTypeId select s).FirstOrDefault();
            Delete(oldPaymentType);
        }



        public IEnumerable<PaymentType> EntityList { get; set; }
        public int Count(Func<PaymentType, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<PaymentType> Select(int index = 0, int count = int.MaxValue)
        {
            var paymentTypeList = from paymentType in Context.PaymentTypes
                                  select paymentType;

            return paymentTypeList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<PaymentType> Select(Func<PaymentType, bool> predicate, int index, int count)
        {
            var paymentTypeList = (from paymentType in Context.PaymentTypes
                                   select paymentType).Where(predicate);

            return paymentTypeList.Skip(index).Take(count).ToArray();
        }
        public PaymentType GetPaymentTypeById(int id)
        {
            var paymentType = Context.PaymentTypes.SingleOrDefault(a => a.Id == id);

            return paymentType;
        }
        public IEnumerable<PaymentType> Select(PaymentType filterItem, int index, int count)
        {
            var paymentTypeList = from paymentType in Context.PaymentTypes
                                  select paymentType;

            if (filterItem.Description != null)
            {
                paymentTypeList = paymentTypeList.Where(t => t.Description.Contains(filterItem.Description));
            }

            if (string.IsNullOrWhiteSpace(filterItem.Name) == false)
            {
                paymentTypeList = paymentTypeList.Where(t => t.Name.Contains(filterItem.Name));
            }

            return paymentTypeList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
