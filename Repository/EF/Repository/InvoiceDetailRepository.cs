using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class InvoiceDetailRepository : EFBaseRepository<InvoiceDetail>
    {

        public void CreateInvoiceDetail(InvoiceDetail newInvoiceDetail)
        {
            Add(newInvoiceDetail);
        }
        public void UpdateInvoice(InvoiceDetail updateableInvoiceDetail)
        {
            var oldInvoice = (from s in Context.InvoiceDetails where s.Id == updateableInvoiceDetail.Id select s).FirstOrDefault();

            oldInvoice.Id = oldInvoice.Id;
            oldInvoice.IsFirstTeam = updateableInvoiceDetail.IsFirstTeam;
            oldInvoice.PaymentRuleId = updateableInvoiceDetail.PaymentRuleId;
            oldInvoice.TeamId = updateableInvoiceDetail.TeamId;
            oldInvoice.TeamUnitCost = updateableInvoiceDetail.TeamUnitCost;
            oldInvoice.ExtraTeamDiscount = updateableInvoiceDetail.ExtraTeamDiscount;
            oldInvoice.ExtraParticipantUnitCost = updateableInvoiceDetail.ExtraParticipantUnitCost;
            oldInvoice.ExtraParticipantCount = updateableInvoiceDetail.ExtraParticipantCount;
            oldInvoice.ExtraParticipantAmount = updateableInvoiceDetail.ExtraParticipantAmount;
            oldInvoice.Amount = updateableInvoiceDetail.Amount;

            Update(oldInvoice);
        }

        public IEnumerable<InvoiceDetail> EntityList { get; set; }
        public int Count(Func<InvoiceDetail, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<InvoiceDetail> Select(int index = 0, int count = int.MaxValue)
        {
            var invoiceDetailList = from invoiceDetail in Context.InvoiceDetails
                                    select invoiceDetail;

            return invoiceDetailList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<InvoiceDetail> GetInvoiceDetailByTeams(int[] teamIds)
        {
            var invoiceDetailList = from invoiceDetail in Context.InvoiceDetails
                                    where teamIds.Contains(invoiceDetail.TeamId)
                                    select invoiceDetail;

            return invoiceDetailList.OrderBy(A => A.Id).ToArray();
        }
        public IEnumerable<InvoiceDetail> Select(Func<InvoiceDetail, bool> predicate, int index, int count)
        {
            var InvoiceList = (from InvoiceDetail in Context.InvoiceDetails
                               select InvoiceDetail).Where(predicate);

            return InvoiceList.Skip(index).Take(count).ToArray();
        }
        public InvoiceDetail GetInvoiceById(int id)
        {
            var invoiceDetail = Context.InvoiceDetails.SingleOrDefault(a => a.Id == id);

            return invoiceDetail;
        }

        public IEnumerable<InvoiceDetail> GetInvoiceDetailByInvoiceId(int invoiceId)
        {
            var invoiceDetail = Context.InvoiceDetails.Where(a => a.InvoiceId == invoiceId);

            return invoiceDetail;
        }
        public void DeleteInvoiceDetail(int invoiceDetailId)
        {
            var oldInvoiceDetail = (from s in Context.InvoiceDetails where s.Id == invoiceDetailId select s).FirstOrDefault();
            Delete(oldInvoiceDetail);
        }
        public void DeleteInvoiceDetailsByInvoiceId(int invoiceId)
        {
            var oldInvoiceDetail = (from s in Context.InvoiceDetails where s.InvoiceId == invoiceId select s);
            foreach (var item in oldInvoiceDetail)
            {
                Delete(item);

            }
        }
    }
}
