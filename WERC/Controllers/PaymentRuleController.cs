using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.PaymentRule;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class PaymentRuleController : BaseController
    {

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmPaymentRule filterItem = null)
        {
            var blPaymwntRule = new BLPaymentRule();
            var paymentRuleList = blPaymwntRule.GetPaymentRulesByFilter(filterItem);

            return Json(paymentRuleList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("upar")]
        [HttpPost]
        public ActionResult UpdateParticipantRule([Bind(Exclude = "ExtraParticipantFee")] VmParticipantRule model)
        //public ActionResult UpdateParticipantRule( VmParticipantRule model)
        {
            var message = "";
            var result = true;
            try
            {
                model.ExtraParticipantFee = decimal.Parse(model.UIExtraParticipantFee,System.Globalization.NumberStyles.Currency);

                if (!ModelState.IsValid)
                {

                    var jsonEx = JsonConvert.SerializeObject(ModelState, Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                               });

                    var jsonException = new
                    {
                        participantRuleId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx

                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var blParticipantRule = new BLParticipantRule();

                result = blParticipantRule.UpdateParticipantRule(model);


                if (result == false)
                {
                    message = model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
                }
                else
                {
                    message = model.ActionMessageHandler.Message = "Operation has been successful";
                }

                var jsonData = new
                {
                    participantRuleId = model.Id,
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
                    paymentRuleId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreatePaymentRule(VmPaymentRule model)
        {
            var result = -1;
            var blPaymentRule = new BLPaymentRule();

            result = blPaymentRule.CreatePaymentRule(model);

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
                paymentRuleId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdatePaymentRule(VmPaymentRule model)
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
                        paymentRuleId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blPaymentRule = new BLPaymentRule();

                result = blPaymentRule.UpdatePaymentRule(model);

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
                    paymentRuleId = model.Id,
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
                    paymentRuleId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeletePaymentRule(VmPaymentRule model)
        {
            var result = true;
            var blPaymentRule = new BLPaymentRule();

            result = blPaymentRule.DeletePaymentRule(model.Id);


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