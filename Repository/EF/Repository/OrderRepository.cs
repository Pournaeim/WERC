using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class OrderRepository : EFBaseRepository<Order>
    {

        public void CreateOrder(Order newOrder)
        {
            Add(newOrder);
        }
        public void UpdateOrder(Order updateableOrder)
        {
            var oldOrder = (from s in Context.Orders where s.Id == updateableOrder.Id select s).FirstOrDefault();

            oldOrder.ShopOrderId = updateableOrder.ShopOrderId;
            oldOrder.UserId = updateableOrder.UserId;
            oldOrder.OrderDate = updateableOrder.OrderDate;
            oldOrder.InvoiceId = updateableOrder.InvoiceId;
            oldOrder.Complete = updateableOrder.Complete;

            Update(oldOrder);
        }
        public void UpdateOrderStatus(string received, string transactionNo, int id, bool complete)
        {
            var oldOrder = (from s in Context.Orders where s.Id == id select s).FirstOrDefault();

            oldOrder.Complete = complete;
            oldOrder.TransactionNo = transactionNo;
            try
            {
                oldOrder.Received = DateTime.Parse(received);
            }
            catch { }


            Update(oldOrder);
        }
        public void DeleteOrder(int OrderId)
        {
            var oldOrder = (from s in Context.Orders where s.Id == OrderId select s).FirstOrDefault();
            Delete(oldOrder);
        }
        public IEnumerable<Order> EntityList { get; set; }
        public int Count(Func<Order, bool> predicate)
        {
            return EntityList.Count();
        }
        public IEnumerable<Order> Select(int index = 0, int count = int.MaxValue)
        {
            var OrderList = from Order in Context.Orders
                            select Order;

            return OrderList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Order> Select(Func<Order, bool> predicate, int index, int count)
        {
            var OrderList = (from Order in Context.Orders
                             select Order).Where(predicate);

            return OrderList.Skip(index).Take(count).ToArray();
        }
        public Order GetOrderById(int id)
        {
            var Order = Context.Orders.SingleOrDefault(a => a.Id == id);

            return Order;
        }

        public Order GetOrderByUserId(string userId, bool complete)
        {
            var Order = Context.Orders.AsNoTracking().SingleOrDefault(a => a.UserId == userId && a.Complete == complete);

            return Order;
        }
        public Order GetLastOrder(string userId, int invoiceId)
        {
            var order = (from o in Context.Orders.AsNoTracking()
                         where
                        o.UserId == userId && o.InvoiceId == invoiceId
                         orderby o.OrderDate descending
                         select o).FirstOrDefault();

            return order;
        }
        public Order GetLastOrder(int invoiceId)
        {
            var order = (from o in Context.Orders.AsNoTracking()
                         where o.InvoiceId == invoiceId
                         orderby o.OrderDate descending
                         select o).FirstOrDefault();

            return order;
        }
        public Order GetCompleteOrder(string userId, int invoiceId)
        {
            var order = (from o in Context.Orders.AsNoTracking()
                         where
                        o.UserId == userId && o.InvoiceId == invoiceId && o.Complete == true
                         select o).FirstOrDefault();

            return order;
        }

        public Order CheckCompleteOrder(string userId, int invoiceId)
        {
            var order = (from o in Context.Orders.AsNoTracking()
                         where
                        o.UserId == userId && o.InvoiceId == invoiceId
                         select o).FirstOrDefault();

            return order;
        }
    }
}
