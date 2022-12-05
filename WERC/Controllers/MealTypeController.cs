using BLL;
using System.Web.Mvc;
using System;
using Model.ViewModels.ParticipantRule;
using Model.ViewModels.MealType;
using Newtonsoft.Json;
using WERC.Models.CustomModelBinding;

namespace WERC.Controllers
{
    public class MealTypeController : BaseController
    {

        [HttpGet]
        [ActionName("gptddl")]
        public JsonResult GetMealTypeDropdownList()
        {
            var blMealType = new BLMealType();
            var mealTypeList = blMealType.GetMealTypeSelectListItem(0, int.MaxValue);

            return Json(mealTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("gpbf")]
        public JsonResult GetTeamMembersByFilter(VmMealType filterItem = null)
        {
            var blMealType = new BLMealType();
            var mealTypeList = blMealType.GetMealTypesByFilter(filterItem);

            return Json(mealTypeList, JsonRequestBehavior.AllowGet);
        }


        [ActionName("cpr")]
        [HttpPost]
        public ActionResult CreateMealType(VmMealType model)
        {
            var result = -1;
            var blMealType = new BLMealType();

            result = blMealType.CreateMealType(model);


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
                mealTypeId = model.Id,
                success = result,
                message

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upr")]
        [HttpPost]
        public ActionResult UpdateMealType(VmMealType model)
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
                        mealTypeId = model.Id,
                        success = false,
                        message = message + "\n" + jsonEx
                    };

                    return Json(jsonException, JsonRequestBehavior.AllowGet);
                }

                var result = true;
                var blMealType = new BLMealType();

                result = blMealType.UpdateMealType(model);

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
                    mealTypeId = model.Id,
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
                    mealTypeId = model.Id,
                    success = false,
                    message = message + "\n" + jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("dpr")]
        [HttpPost]
        public ActionResult DeleteMealType(VmMealType model)
        {
            var result = true;
            var blMealType = new BLMealType();

            result = blMealType.DeleteMealType(model.Id);


            var message = "";
            if (result == false)
            {
                message = "Operation has been failed...\n The meal type was selected by a person";
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