using BLL;
using System.Web.Mvc;

namespace WERC.Controllers
{
    public class YearClassificationController : BaseController
    {
        [ActionName("gycddl")]
        public ActionResult GetYearClassificationDropDownList()
        {
            var bsYearClassification = new BLYearClassification();

            var yearClassificationList = bsYearClassification.GetYearClassificationSelectListItem(0, int.MaxValue);

            return Json(yearClassificationList, JsonRequestBehavior.AllowGet);
        }
        
    }
}