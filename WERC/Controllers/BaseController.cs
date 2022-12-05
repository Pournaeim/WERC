using BLL;
using CyberneticCode.Web.Mvc.Helpers;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using WERC.AppDomainHelper;
using WERC.Filters.ActionFilterAttributes;
using WERC.Filters.CacheFilters;
using WERC.Filters.FilterAttributes;

namespace WERC.Controllers
{
    [RequireHttps]
    [Localization]
    [WERCHandleError]
    //[NoCache]
    public class BaseController : Controller

    {
        public EmailHelper emailHelper = null;
        public string specialEmail = "bictchairman@gmail.com";
        protected ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            protected set
            {
                _userManager = value;
            }
        }

        public string CurrentUserId { get; set; }
        public IEnumerable<string> CurrentUserRoles { get; set; }

        public ReflectionCall ControlerReflectionCall { get; set; }

        public BaseController()
        {


        }
        public void LoadLastModelStateErrors()
        {
            if (TempData["LastModelStateErrors"] != null)
            {
                var LastModelStateErrors = (IEnumerable<string>)TempData["LastModelStateErrors"];

                foreach (var error in LastModelStateErrors)
                {
                    ModelState.AddModelError("", error);
                }
            }
        }

        public void ResizePicture(ref Bitmap bitmap)
        {

            if (bitmap.Width > 192)
            {
                //IPTools.ResizeImageWidthBySaveScale(ref bitmap, 192);
                IPTools.Resize_Picture(ref bitmap, "", 144, 0, 96);
            }
            if (bitmap.Height > 192)
            {
                IPTools.Resize_Picture(ref bitmap, "", 0, 144, 96);
                //IPTools.ResizeImageHeightBySaveScale(ref bitmap, 192);
            }

        }

        public void CheckSuppressScoring()
        {
            var blTeam = new BLTeam();

            var blSundryRule = new BLSundryRule();

            var sundryRule = blSundryRule.GetSundryRuleById(1);

            var deadLineDate = DateTime.Parse(sundryRule.DueDate);

            var currentDate = DateTime.Now;

            if (deadLineDate.Date < currentDate.Date

                || 
               (deadLineDate.Date == currentDate.Date && deadLineDate.TimeOfDay <= currentDate.TimeOfDay))
            {

                blTeam.SetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring();

            }
            else
            {
                blTeam.UnSetAllowSuppressScoringAllTeamsExceptAdminSuppressScoring();
            }

        }


    }
}