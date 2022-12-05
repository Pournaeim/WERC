using BLL;
using Model.ViewModels.Invoice;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using WERC.Filters.CacheFilters;
using WERC.Models;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    public class InvoiceController : BaseController
    {

        [HttpGet]
        [ActionName("lif")]
        [Route("{controller=invoice}/{action=lif}/error")]
        public ActionResult LoadInvoiceError()
        {

            var errorMessage = "Error: Unsuccessful Payment Process";
            return RedirectToAction("lifp", new { message = errorMessage });
        }

        [HttpGet]
        [ActionName("lifp")]
        [Route("invoice/lifp/{message:string}")]
        public PartialViewResult LoadInvoiceForm(string message = null)
        {

            var blInvoice = new BLInvoice();

            var invoice = blInvoice.GetInvoiceByUserId(CurrentUserId, false);

            if (invoice != null)
            {
                int? lastOrderId = 0;

                var blShopCart = new BLShopCart();

                var lastOrderInfo = blShopCart.GetCheckoutStatus(CurrentUserId, invoice.Id, out lastOrderId);

                if (lastOrderInfo != null && string.IsNullOrEmpty(message))
                {
                    var blOrder = new BLOrder();

                    if (blOrder.CheckCompleteOrder(CurrentUserId, invoice.Id).Complete == true)
                    {
                        blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoice.Id, true, lastOrderId.Value, true, true);
                    }
                    invoice = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);
                }
            }

            var invoiceRsult = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);

            if (invoiceRsult == null)
            {
                var layout = "_LayoutAdmin.cshtml";

                if (CurrentUserRoles.Contains("Advisor"))
                {
                    layout = "~/Views/Shared/_LayoutAdvisor.cshtml";
                }

                if (CurrentUserRoles.Contains("Judge"))
                {
                    layout = "~/Views/Shared/_LayoutJudge.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Student.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutStudent.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.Leader.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutLeader.cshtml";
                }

                if (CurrentUserRoles.Contains(SystemRoles.CoAdvisor.ToString()))
                {
                    layout = "~/Views/Shared/_LayoutCoAdvisor.cshtml";
                }

                return PartialView("_EmptyReviewOrder", new VMHandleErrorInfo
                {
                    ErrorMessage = "Please complete all team member's profiles before proceeding to payment.",
                    ViewLayout = layout,

                });
            }

            List<VmTeamSelection> teamSelectionList = new List<VmTeamSelection>();

            foreach (var item in invoiceRsult.InvoiceDetails)
            {
                teamSelectionList.Add(new VmTeamSelection
                {
                    Checked = item.IsChecked,
                    IsFirstTeam = item.IsFirstTeam,
                    TeamId = item.TeamId
                });
            }

            /// blInvoice.ProcessInvoice(CurrentUserId, invoiceList.LastCheckedId, teamSelectionList);
            var ScholarshipDiscount = blInvoice.ProcessInvoice(CurrentUserId, invoiceRsult.LastCheckedId, teamSelectionList);

            invoiceRsult = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);

            invoiceRsult.ScholarshipDiscount = ScholarshipDiscount;

            invoiceRsult.ErrorMessage = message;
            return PartialView("ReviewOrderManagement", invoiceRsult);
        }

        [HttpPost]
        [ActionName("pi")]
        public ActionResult ProcessInvoice(int currentTeamId, List<VmTeamSelection> teamSelectionList)
        {

            var blInvoice = new BLInvoice();
            var finishedInvoice = blInvoice.GetInvoiceByUserId(CurrentUserId, true);

            //// if (finishedInvoice == null && teamSelectionList.Count(t => t.Checked == true) > 0 && teamSelectionList.Count(t => t.IsFirstTeam == true) != 1)
            if (blInvoice.GetPayedTeamCount(CurrentUserId) == 0 && teamSelectionList.Count(t => t.Checked == true) > 0 && teamSelectionList.Count(t => t.IsFirstTeam == true) != 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new
                {
                    hasError = true,
                    message = "One team must be as a first team",

                }, JsonRequestBehavior.DenyGet);
            }

            var ScholarshipDiscount = blInvoice.ProcessInvoice(CurrentUserId, currentTeamId, teamSelectionList);

            var invoice = blInvoice.GetInvoiceFullInfoByUserId(CurrentUserId, false);

            invoice.ScholarshipDiscount = ScholarshipDiscount;
            invoice.LastCheckedId = currentTeamId;

            return PartialView("~/Views/Invoice/_ReviewOrder.cshtml", invoice);
        }


        [HttpGet]
        [ActionName("lifem")]
        [NoCache]
        public PartialViewResult LoadExtraMemberInvoiceForm()
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
                    blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoice.Id, true, lastOrderId.Value, true, true);

                }
            }

            var invoiceList = blInvoice.GetExtraMemberInvoiceFullInfoByUserId(CurrentUserId);

            if (invoiceList == null)
            {
                return PartialView("_EmptyReviewOrder", new VMHandleErrorInfo
                {
                    ErrorMessage = "There is no balance to pay for extra members."
                });
            }

            return PartialView("ExtraMemberReviewOrderManagement", invoiceList);
        }


    }
}