using BLL;

using Microsoft.AspNet.Identity.Owin;

using Model.ViewModels.AspNetUsers2;

using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;
using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [ActionName("ul")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult UserList()
        {
            var blUser = new BLUser();

            return View("UserList", blUser.GetAllUsers());
        }

        [HttpGet]
        [ActionName("su")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult SearchUser(
          string searchText = "")
        {
            var blUser = new BLUser();

            return View("UserList", blUser.GetUserByFiler(searchText));

        }

        [HttpPost]
        [ActionName("ci")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult ConfirmEmail(string userId)
        {
            var blUser = new BLUser();
            var result = blUser.ConfirmEmail(userId);
            var jsonData = new
            {
                result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [ActionName("gp")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult GetPassword(string userId)
        {
            string result = "";
            BLAspNetUsers2 blAspNetUsers2 = new BLAspNetUsers2();
            var userData = blAspNetUsers2.GetAspNetUsers2ByUserId(userId);
            if (userData != null)
            {
                result = userData.Password;
            }

            var jsonData = new
            {
                result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [ActionName("sd")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult SetPassword(string userId, string password, string email)
        {
            BLAspNetUsers2 blAspNetUsers2 = new BLAspNetUsers2();
            var result = blAspNetUsers2.CreateAspNetUsers2(
                new VmAspNetUsers2
                {
                    UserId = userId,
                    Password = password,
                    Email = email,
                    PasswordHash = UserManager.PasswordHasher.HashPassword(password)
                });

            var jsonData = new
            {
                result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [ActionName("du")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult DeleteUser(string userId)
        {
            var blUser = new BLUser();
            var result = blUser.DeleteUser(userId);
            var jsonData = new
            {
                result,
                message = "Operation has been failed...\n" +
                "This advisor has created a team or teams. in order to delete that the team(s) and team member(s) should be deleted at first."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [ActionName("asr")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public async Task<ActionResult> AssignStudentRole(string userId)
        {
            var blUser = new BLUser();
            var result = false;

            if (blUser.HasUserRoles(userId) == false)
            {
                await UserManager.AddToRoleAsync(userId, "Student");

                result = true;
            }
            var jsonData = new
            {
                result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }
    }
}