using BLL;
using System.Web.Mvc;
using WERC.Models;

namespace WERC.Controllers
{
    public class ReceiptController : BaseController
    {
        [HttpGet]
        [ActionName("receipt")]
        // Recept Log out
        public PartialViewResult LoadReceiptForm()
        {
            var blInvoice = new BLInvoice();

            //var invoice = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, true);
            //invoice.TransactionNo = "12355787";
            //invoice.Received = DateTime.Now.Date.ToShortDateString();

            var invoice = blInvoice.GetInvoiceByUserId(CurrentUserId, false);

            if (invoice != null)
            {
                int? lastOrderId = 0;

                var blShopCart = new BLShopCart();
                var lastOrderInfo = blShopCart.GetCheckoutStatus(CurrentUserId, invoice.Id, out lastOrderId);

                if (lastOrderInfo != null)
                {
                    blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoice.Id, true, lastOrderId.Value, true, true);

                    invoice = blInvoice.GetInvoiceFullInfoById(invoice.Id);

                    invoice.Received = lastOrderInfo.Order.Received;
                    invoice.TransactionNo = lastOrderInfo.Order.Tx;

                    return PartialView("LastReceiptForm", invoice);
                }
            }

            return PartialView("LastReceiptForm", invoice);

        }

        [HttpGet]
        [ActionName("receiptDetail")]
        //Invoice Menu
        public PartialViewResult LoadReceiptDetail(int id)
        {
            var blInvoice = new BLInvoice();

            var invoice = blInvoice.GetInvoiceFullInfoById(id);

            if (invoice == null)
            {
                return PartialView("_EmptyReviewOrder", new VMHandleErrorInfo
                {
                    ErrorMessage = "In order to see the detail of your receipt, " +
                    "please complete all team member(s) registration forms."
                });

            }

            var blOrder = new BLOrder();
            var completeOrderInfo = blOrder.GetCompleteOrder(CurrentUserId, id);

            invoice.Received = completeOrderInfo.Received.Value.ToShortDateString();
            invoice.TransactionNo = completeOrderInfo.TransactionNo;

            return PartialView("LastReceiptForm", invoice);
        }


        [HttpPost]
        [ActionName("receiptList")]
        [Route("{controller=receipt}/{action=receiptList}/succeed")] //Invoice List
        public ActionResult LoadReceiptSuccessList()
        {

            var blInvoice = new BLInvoice();

            var invoice = blInvoice.GetInvoiceByUserId(CurrentUserId, false);

            if (invoice != null)
            {
                int? lastOrderId = 0;

                var blShopCart = new BLShopCart();
                var lastOrderInfo = blShopCart.GetCheckoutStatus(CurrentUserId, invoice.Id, out lastOrderId);

                if (lastOrderInfo != null)
                {
                    //Update all data in one transaction
                    blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoice.Id, true, lastOrderId.Value, true, true);
                    invoice.Received = lastOrderInfo.Order.Received;
                    invoice.TransactionNo = lastOrderInfo.Order.Tx;
                }
            }

            var invoiceIdList = blInvoice.GetInvoiceFullInfoByUserAndStatus(CurrentUserId, true);
            if (invoiceIdList != null)
            {

                return View("LastReceiptList", invoiceIdList);

            }

            return PartialView("_EmptyReviewOrder", new VMHandleErrorInfo
            {
                ErrorMessage = "There is no receipt."
            });
        }

        [HttpGet]
        [ActionName("receiptListp")]
        public ActionResult LoadReceiptList()
        {
            var blInvoice = new BLInvoice();

            var invoiceIdList = blInvoice.GetInvoiceFullInfoByUserAndStatus(CurrentUserId, true);
            if (invoiceIdList != null)
            {

                return View("LastReceiptList", invoiceIdList);

            }

            return PartialView("_EmptyReviewOrder", new VMHandleErrorInfo
            {
                ErrorMessage = "There is no receipt."
            });

        }

    }
}