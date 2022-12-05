using BLL.Base;
using Model;
using Model.ViewModels.Order;
using Repository.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLOrder : BLBase
    {
        public VmOrder GetOrderById(int id)
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var order = orderRepository.GetOrderById(id);

            var vmOrder = new VmOrder
            {
                Id = order.Id,
                ShopOrderId = order.ShopOrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                InvoiceId = order.InvoiceId,
                Complete = order.Complete,
            };

            return vmOrder;
        }
        public int? GetLastOrder(string userId, int invoiceId)
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var order = orderRepository.GetLastOrder(userId, invoiceId);

            if (order == null)
            {
                return null;
            }

            return order.Id;
        }
        public int? GetLastOrder(int invoiceId)
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var order = orderRepository.GetLastOrder(invoiceId);

            if (order == null)
            {
                return null;
            }

            return order.ShopOrderId;
        }
        public VmOrder GetCompleteOrder(string userId, int invoiceId)
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var order = orderRepository.GetCompleteOrder(userId, invoiceId);
            var vmOrder = new VmOrder
            {
                Id = order.Id,
                InvoiceId = order.InvoiceId,
                Received = order.Received,
                TransactionNo = order.TransactionNo,
            };

            return vmOrder;
        }

        public IEnumerable<VmOrder> GetAllOrder()
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var orderList = orderRepository.Select(0, int.MaxValue);
            var vmOrderList = from order in orderList
                              select new VmOrder
                              {
                                  Id = order.Id,
                                  ShopOrderId = order.ShopOrderId,
                                  UserId = order.UserId,
                                  OrderDate = order.OrderDate,
                                  InvoiceId = order.InvoiceId,
                                  Complete = order.Complete,

                              };

            return vmOrderList;
        }

        public int CreateOrder(VmOrder vmOrder)
        {            
            var result = -1;
            try
            {
                var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

                var newOrder = new Order
                {
                    ShopOrderId = vmOrder.ShopOrderId,
                    UserId = vmOrder.UserId,
                    OrderDate = vmOrder.OrderDate,
                    InvoiceId = vmOrder.InvoiceId,
                    Complete = vmOrder.Complete,
                };

                orderRepository.CreateOrder(newOrder);

                UnitOfWork.Commit();

                result = newOrder.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateOrder(VmOrder vmOrder)
        {
            try
            {
                var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

                var updateableOrder = new Order
                {
                    Id = vmOrder.Id,
                    ShopOrderId = vmOrder.ShopOrderId,
                    UserId = vmOrder.UserId,
                    OrderDate = vmOrder.OrderDate,
                    InvoiceId = vmOrder.InvoiceId,
                    Complete = vmOrder.Complete,
                };

                orderRepository.UpdateOrder(updateableOrder);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteOrder(int orderId)
        {
            try
            {
                var orderRepository = UnitOfWork.GetRepository<OrderRepository>();


                orderRepository.DeleteOrder(orderId);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }

        public VmOrder CheckCompleteOrder(string userId, int invoiceId)
        {
            var orderRepository = UnitOfWork.GetRepository<OrderRepository>();

            var order = orderRepository.CheckCompleteOrder(userId, invoiceId);
            var vmOrder = new VmOrder
            {
                Id = order.Id,
                InvoiceId = order.InvoiceId,
                Received = order.Received,
                TransactionNo = order.TransactionNo,
            };

            return vmOrder;
        }

    }
}
