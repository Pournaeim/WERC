using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class InvoiceRepository : EFBaseRepository<Invoice>
    {

        public void CreateInvoice(Invoice newInvoice)
        {
            Add(newInvoice);
        }
        public void UpdateInvoice(Invoice updateableInvoice)
        {
            var oldInvoice = (from s in Context.Invoices where s.Id == updateableInvoice.Id select s).FirstOrDefault();

            oldInvoice.Title = updateableInvoice.Title;
            oldInvoice.DateOfIssue = updateableInvoice.DateOfIssue;
            oldInvoice.InvoiceNumber = updateableInvoice.InvoiceNumber;
            oldInvoice.InvoiceTotal = updateableInvoice.InvoiceTotal;
            oldInvoice.Subtotal = updateableInvoice.Subtotal;
            oldInvoice.Tax = updateableInvoice.Tax;
            oldInvoice.Total = updateableInvoice.Total;
            oldInvoice.AmountDue = updateableInvoice.AmountDue;
            oldInvoice.Finished = updateableInvoice.Finished;

            Update(oldInvoice);
        }
        public void UpdateInvoiceTitle(int id, string title)
        {
            var oldInvoice = (from s in Context.Invoices where s.Id == id select s).FirstOrDefault();

            oldInvoice.Title = title;

            Update(oldInvoice);
        }
        public void UpdateInvoiceStatus(int invoiceId, bool finished)
        {
            var oldInvoice = Context.Invoices.Find(invoiceId);

            oldInvoice.Finished = finished;

            Update(oldInvoice);
        }
        public void DeleteInvoice(int InvoiceId)
        {
            var oldInvoice = (from s in Context.Invoices where s.Id == InvoiceId select s).FirstOrDefault();
            Delete(oldInvoice);
        }
        public IEnumerable<Invoice> EntityList { get; set; }
        public int Count(Func<Invoice, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<Invoice> Select(int index = 0, int count = int.MaxValue)
        {
            var InvoiceList = from Invoice in Context.Invoices
                              select Invoice;

            return InvoiceList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Invoice> Select(Func<Invoice, bool> predicate, int index, int count)
        {
            var InvoiceList = (from Invoice in Context.Invoices
                               select Invoice).Where(predicate);

            return InvoiceList.Skip(index).Take(count).ToArray();
        }
        public Invoice GetInvoiceById(int id)
        {
            var Invoice = Context.Invoices.SingleOrDefault(a => a.Id == id);

            return Invoice;
        }

        public Invoice GetInvoiceByUserId(string userId, bool finished)
        {
            var Invoice = Context.Invoices.AsNoTracking().SingleOrDefault(a => a.UserId == userId && a.Finished == finished);

            return Invoice;
        }
        public IEnumerable<Invoice> GetInvoiceList(bool finished)
        {
            var invoiceList = Context.Invoices.Where(a => a.Finished == finished);

            return invoiceList;
        }
        public IEnumerable<Invoice> GetInvoiceIdsByUserId(string userId, bool finished)
        {
            var Invoice = from i in Context.Invoices
                          where i.UserId == userId && i.Finished == finished
                          select i;

            return Invoice.ToArray();
        }
        public IEnumerable<Invoice> GetInvoiceIds(bool finished)
        {
            var Invoice = from i in Context.Invoices
                          where i.Finished == finished
                          select i;

            return Invoice.ToArray();
        }
    }
}
