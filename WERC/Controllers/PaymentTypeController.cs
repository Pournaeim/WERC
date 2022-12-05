using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.PaymentType;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class PaymentTypeController : BaseController
    {

        [HttpGet]
        [ActionName("gptddl")]
        public JsonResult GetPaymentTypeDropdownList()
        {
            var blPaymentType = new BLPaymentType();
            var paymentTypeList = blPaymentType.GetPaymentTypeSelectListItem(0, int.MaxValue);

            return Json(paymentTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmPaymentType filterItem = null)
        {
            var blPaymentType = new BLPaymentType();
            var paymentTypeList = blPaymentType.GetPaymentTypesByFilter(filterItem);

            return Json(paymentTypeList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreatePaymentType(VmPaymentType model)
        {
            var result = -1;
            var blPaymentType = new BLPaymentType();

            result = blPaymentType.CreatePaymentType(model);


            var message = "";
            if (result == -1)
            {
                message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                message = "Operation has been successful";
            }

            var jsonData = new
            {
                paymentTypeId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdatePaymentType(VmPaymentType model)
        {
            var message = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    var jsonEx = JsonConvert.SerializeObject(ModelState, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                               });

                    var jsonException = new
                    {
                        paymentTypeId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blPaymentType = new BLPaymentType();

                result = blPaymentType.UpdatePaymentType(model);

                if (result == false)
                {
                    message += "Operation has been failed...\n call system Admin\n";
                }
                else
                {
                    message += "Operation has been successful";
                }

                var jsonData = new
                {
                    paymentTypeId = model.Id,
                    success = result,
                    message

                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var jsonEx = JsonConvert.SerializeObject(ex, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });

                var jsonException = new
                {
                    paymentTypeId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeletePaymentType(VmPaymentType model)
        {
            var result = true;
            var blPaymentType = new BLPaymentType();

            result = blPaymentType.DeletePaymentType(model.Id);


            var message = "";
            if (result == false)
            {
                message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                message = "Operation has been successful";
            }

            var jsonData = new
            {
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}