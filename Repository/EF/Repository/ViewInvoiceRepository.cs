using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewInvoiceRepository : EFBaseRepository<ViewInvoice>
    {
        public IEnumerable<ViewInvoice> EntityList { get; set; }
        public int Count(Func<ViewInvoice, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewInvoice> Select(int index = 0, int count = int.MaxValue)
        {
            var ViewInvoiceList = from ViewInvoice in Context.ViewInvoices.AsNoTracking()
                                  select ViewInvoice;

            return ViewInvoiceList.OrderBy(A => A.InvoiceId).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewInvoice> Select(Func<ViewInvoice, bool> predicate, int index, int count)
        {
            var ViewInvoiceList = (from ViewInvoice in Context.ViewInvoices.AsNoTracking()
                                   select ViewInvoice).Where(predicate);

            return ViewInvoiceList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewInvoice> GetViewInvoiceByUserId(string userId, bool payStatus)
        {
            var viewInvoice = from a in Context.ViewInvoices.AsNoTracking()
                              where a.UserId == userId && a.PayStatus == payStatus
                              select a;

            return viewInvoice.ToArray();
        }
        public IEnumerable<ViewInvoice> PayChequeGetViewInvoiceByUserId(string userId, bool payStatus)
        {
            var viewInvoice = from a in Context.ViewInvoices.AsNoTracking()
                              where a.UserId == userId && a.PayStatus == payStatus && a.Finished == false
                              select a;

            return viewInvoice.ToArray();
        }
        public IEnumerable<ViewInvoice> GetViewInvoiceByUserId(string userId, bool payStatus, int invoiceId)
        {
            var viewInvoice = from a in Context.ViewInvoices.AsNoTracking()
                              where a.UserId == userId && a.PayStatus == payStatus && a.InvoiceId == invoiceId
                              select a;

            return viewInvoice.ToArray();
        }

        public IEnumerable<ViewInvoice> GetViewInvoiceByUserId(string userId)
        {
            var viewInvoice = from a in Context.ViewInvoices.AsNoTracking()
                              where a.UserId == userId
                              select a;

            return viewInvoice.ToArray();
        }
        public IEnumerable<ViewInvoice> GetViewInvoiceById(int invoiceId)
        {
            var viewInvoice = from a in Context.ViewInvoices.AsNoTracking()
                              where a.InvoiceId == invoiceId
                              select a;

            return viewInvoice.ToArray();
        }
        public int GetPayedTeamCount(string userId)
        {
            return Context.ViewInvoices.AsNoTracking().Where(a => a.UserId == userId && a.PayStatus == true).Count();
        }

    }
}
