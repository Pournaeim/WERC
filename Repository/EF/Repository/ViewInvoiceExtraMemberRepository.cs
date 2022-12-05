using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewInvoiceExtraMemberRepository : EFBaseRepository<ViewInvoiceExtraMember>
    {
        public IEnumerable<ViewInvoiceExtraMember> EntityList { get; set; }
        public int Count(Func<ViewInvoiceExtraMember, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<ViewInvoiceExtraMember> Select(int index = 0, int count = int.MaxValue)
        {
            var ViewInvoiceExtraMemberList = from ViewInvoiceExtraMember in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                             select ViewInvoiceExtraMember;

            return ViewInvoiceExtraMemberList.OrderBy(A => A.InvoiceId).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> Select(Func<ViewInvoiceExtraMember, bool> predicate, int index, int count)
        {
            var ViewInvoiceExtraMemberList = (from ViewInvoiceExtraMember in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                              select ViewInvoiceExtraMember).Where(predicate);

            return ViewInvoiceExtraMemberList.Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserId(string userId, bool payStatus)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId && a.PayStatus == payStatus
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserId(string userId, bool payStatus, bool finished)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId && a.PayStatus == payStatus && a.Finished == finished
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserIdFinished(string userId, bool finished)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId && a.Finished == finished
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserId(string userId, bool payStatus, int invoiceId)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId && a.PayStatus == payStatus && a.InvoiceId == invoiceId
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserId(string userId, int invoiceId)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId && a.InvoiceId == invoiceId
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }

        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberByUserId(string userId)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.UserId == userId
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public IEnumerable<ViewInvoiceExtraMember> GetViewInvoiceExtraMemberById(int invoiceId)
        {
            var ViewInvoiceExtraMember = from a in Context.ViewInvoiceExtraMembers.AsNoTracking()
                                         where a.InvoiceId == invoiceId
                                         select a;

            return ViewInvoiceExtraMember.ToArray();
        }
        public int GetPayedTeamCount(string userId)
        {
            return Context.ViewInvoiceExtraMembers.AsNoTracking().Where(a => a.UserId == userId && a.PayStatus == true).Count();
        }

    }
}
