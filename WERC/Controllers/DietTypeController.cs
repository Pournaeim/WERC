using BLL;
using System.Web.Mvc;

namespace WERC.Controllers
{
    public class DietTypeController : BaseController
    {
       
        [ActionName("gdtddl")]
        public ActionResult GettDietTypeDropDownList()
        {
            var bsDietType = new BLDietType();

            var DietTypeList = bsDietType.GetDietTypeSelectListItem(0, int.MaxValue);

            return Json(DietTypeList, JsonRequestBehavior.AllowGet);
        }
    }
}