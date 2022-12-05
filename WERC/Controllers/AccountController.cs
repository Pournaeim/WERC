using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Model.ApplicationDomainModels;
using Model.Base;
using Model.ViewModels;
using Model.ViewModels.AspNetUsers2;
using Model.ViewModels.EmailLog;
using Model.ViewModels.Person;
using Model.ViewModels.User;

using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;
using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {

        ApplicationDbContext context = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
                var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

                user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole()
                {
                    RoleId = "f3b628a1-ab7d-48dc-811d-d509e645d7f0",
                    UserId = userId,
                });

                UserManager.Update(user);

                result = true;
            }

            var jsonData = new
            {
                result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                var blSystemSetting = new BLSystemSetting();
                ViewBag.ReturnUrl = returnUrl;
                return View(new LoginViewModel()
                {
                    ActiveRegisteration = blSystemSetting.GetSystemSettingById(1).Active
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }

        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var blSiteInfo = new BLPerson();

            //CaptchaResponse response = ValidateCaptcha(model.GoogleRecaptchaResponse);

            //if (response.Success == false &&
            //    Request.Url.Authority.Contains("cyberneticcode.com") == false &&
            //    Request.Url.Authority.Contains("localhost:") == false)
            //{
            //    blSiteInfo.CreateSiteInfo("Error From Google ReCaptcha");

            //    return Content("Login: Error From Google ReCaptcha : " + response.ErrorMessage[0].ToString());
            //}

            string URL = HttpContext.Request.Url.Host.ToString();

            blSiteInfo.CreateSiteInfo("Reg: " + URL);
            try
            {
                ViewBag.ReturnUrl = returnUrl;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                IEnumerable<string> userRoles = null;
                var UserName = model.UserName;

                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                if (user != null)
                {
                    UserName = user.UserName;

                    var blUser = new BLUser();
                    SmUserRolesList.UserRoles = blUser.GetAllUserRoles();

                    userRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == UserName select roles.RoleName).AsEnumerable<string>();

                    if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        if (!userRoles.Contains(SystemRoles.Advisor.ToString()) && !userRoles.Contains(SystemRoles.Judge.ToString()))
                        {
                            ModelState.AddModelError("", "You need to confirm your email.");
                            return View(model);
                        }

                    }
                    //if (await UserManager.IsLockedOutAsync(user.Id))
                    //{
                    //    return View("Lockout");
                    //}


                    TempData["UserRoles"] = userRoles;
                }

                var result = await SignInManager.PasswordSignInAsync(UserName, model.Password, model.RememberMe, shouldLockout: true);

                switch (result)
                {
                    case SignInStatus.Success:

                        CurrentUserId = user.Id;
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            if (userRoles.Contains(SystemRoles.Admin.ToString()))
                            {
                                return RedirectToAction("index", "admin");
                            }
                            if (userRoles.Contains(SystemRoles.SafetyAdmin.ToString()))
                            {
                                return RedirectToAction("index", "SafetyAdmin");
                            }

                            if (userRoles.Contains("Advisor"))
                            {
                                return RedirectToAction("tl", "Advisor");
                            }

                            if (userRoles.Contains(SystemRoles.Judge.ToString()))
                            {
                                return RedirectToAction("index", "judge");
                            }

                            if (userRoles.Contains(SystemRoles.Student.ToString()))
                            {
                                return RedirectToAction("index", "home");
                            }

                            if (userRoles.Contains(SystemRoles.Leader.ToString()))
                            {
                                return RedirectToAction("index", "home");
                            }
                            if (userRoles.Contains(SystemRoles.CoAdvisor.ToString()))
                            {
                                return RedirectToAction("index", "home");
                            }
                            ViewBag.UserRole = "";
                        }

                        return RedirectToLocal(returnUrl);


                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe, SetWelcomMessage = true });

                    case SignInStatus.LockedOut:
                        return View("Lockout");

                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, model.RememberMe });

                    case SignInStatus.Failure:

                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error", new VMHandleErrorInfo());
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl = "")
        {


            if (AllowRegistration == false)
            {
                var blTask = new BLTask();
                var taskCount = blTask.GetTaskCount();

                if (taskCount > 0)
                {
                    AllowRegistration = true;
                }
                else
                {
                    return View("Error", new VMHandleErrorInfo("Registration is not allowed before initializing tasks"));
                }
            }

            var UserRoles = TempData["UserRoles"] as IEnumerable<string>;
            var blUniversity = new BLUniversity();
            var universityList = blUniversity.GetUniversitySelectListItem(0, int.MaxValue);

            return View(
                new RegisterViewModel()
                {
                    UniversityList = from u in universityList
                                     select new SelectListItem
                                     {
                                         Text = u.Text,
                                         Value = u.Value
                                     },
                    CurrentUserRoles = UserRoles,
                    ReturnUrl = returnUrl
                });
        }

        [ActionName("acu")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult AdminCreateUser(string role = "")
        {
            var roleList = context.Roles.Where(r =>
                r.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752" &&
                r.Id != "291d6069-44a3-4960-86d3-b91bda430e71" &&
                r.Id != "4d7951d8-8eda-4452-8ff1-dfc9076d8bbe" &&
                r.Id != "58c326dd-38ea-4d3c-92f9-3935e3763e68" &&
                r.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752" &&
                r.Id != "C518E507-1C8C-48E4-81528305BA2453D" &&
                r.Id != "f3b628a1-ab7d-48dc-811d-d509e645d7f0"
            ).OrderBy(r => r.Name).ToList()
                .Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name }).ToList();

            var roleName = "";
            //if (role == "eic")
            //{
            //    roleName = "Advisor";
            //    roleList.Where(r => r.Value == roleName).First().Selected = true;
            //}

            ViewBag.Roles = roleList;

            return View("AdminCreateUser", new RegisterViewModel()
            {
                RoleName = roleName,
                ReturnUrl = HttpContext.Request.RawUrl
            });
        }

        [ActionName("eiccu")]
        [RoleBaseAuthorize(SystemRoles.Advisor)]
        public ActionResult AdvisorCreateUser(string role = "")
        {

            var roleName = "";
            roleName = "Editor";

            if (role == "r")
            {
                roleName = "Editor";
            }

            return View("AdvisorCreateUser", new RegisterViewModel()
            {
                RoleName = roleName,
                ReturnUrl = HttpContext.Request.RawUrl
            });

        }


        /// <summary>  
        /// Validate Captcha  
        /// </summary>  
        /// <param name="response"></param>  
        /// <returns></returns>  
        public CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = WebConfigurationManager.AppSettings["recaptchaPrivateKey"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string byAdmin = "")
        {
            bool userExists = false;
            var blSiteInfo = new BLPerson();

            //CaptchaResponse response = ValidateCaptcha(model.GoogleRecaptchaResponse);

            //if (response.Success == false && byAdmin == "" &&
            //    Request.Url.Authority.Contains("localhost:") == false)
            //{
            //    blSiteInfo.CreateSiteInfo("Error From Google ReCaptcha");

            //    return Content("Reg: Error From Google ReCaptcha : " + response.ErrorMessage[0].ToString());
            //}

            string URL = HttpContext.Request.Url.Host.ToString();

            blSiteInfo.CreateSiteInfo("Reg: " + URL);

            if (ModelState.IsValid)
            {
                var body = "";
                var user = new ApplicationUser
                {
                    UserName = model.Email.Trim().Replace("\t", ""),
                    Email = model.Email.Trim().Replace("\t", ""),
                    RegisterDate = DateTime.UtcNow,
                    PhoneNumber = model.PhoneNumber,
                    LastSignIn = DateTime.UtcNow,
                    WorkPhoneNumber = model.WorkPhoneNumber,
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    BLAspNetUsers2 blAspNetUsers2 = new BLAspNetUsers2();
                    blAspNetUsers2.CreateAspNetUsers2(new VmAspNetUsers2
                    {
                        UserId = user.Id,
                        Password = model.Password
                    });

                    UserManager.AddToRole(user.Id, model.RoleName);

                    var blPerson = new BLPerson();

                    VmPerson vmPerson = null;

                    //if (model.RoleName == SystemRoles.Advisor.ToString() || model.RoleName == SystemRoles.Judge.ToString())
                    //{

                    //    BLPersonArchive blPersonArchive = new BLPersonArchive();
                    //    vmPerson = blPersonArchive.GetPersonByEmail(model.Email);

                    //}

                    if (vmPerson != null)
                    {
                        vmPerson.UserId = user.Id;
                        vmPerson.Agreement = false;

                        blPerson.CreatePersonWithFullInfo(vmPerson);
                    }
                    else
                    {
                        vmPerson = new VmPerson();

                        vmPerson.UserId = user.Id;
                        vmPerson.Sex = model.Sex;
                        vmPerson.FirstName = model.FirstName;
                        vmPerson.LastName = model.LastName;
                        //UniversityId = model.UniversityId;
                        vmPerson.UniversityId = model.RoleName.ToLower().Contains("judge") ? null : (int?)model.UniversityId;
                        vmPerson.WelcomeDinner = false;
                        vmPerson.LunchOnMonday = false;
                        vmPerson.LunchOnTuesday = false;
                        vmPerson.ReceptionNetworkOnTuesday = false;
                        vmPerson.AwardBanquet = false;
                        vmPerson.NoneOfTheAbove = false;
                        blPerson.CreatePerson(vmPerson);
                    }


                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    //if (model.RoleName != "Student")
                    //{
                    //    model.ReturnUrl = "";
                    //}

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code, returnUrl = model.ReturnUrl }, protocol: Request.Url.Scheme);

                    var subject = "Confirm your WERC Environmental Design Contest 2023 account.";
                    //var domainName = callbackUrl.Split('/')[2];
                    body = "<h1> 33rd WERC Environmental Design Contest 2023" + "</h1>" +  //Body ...
                      "<br/>" +
                      "Dear " + model.FirstName + " " + model.LastName + ", " +
                      "<br/>" +
                      "<br/>" +
                      "Thank you for your interest in the 33rd WERC Environmental Design Contest. We have received your request for access to the online platform." + // Each request requires approval from our system administrator.
                      "<br/>" +
                      "Please confirm that you initiated this request by selecting the following link:" +
                      "<br/>" +
                      callbackUrl +
                      //"<hr/>" +
                      //"<b>With approval, your account will be active within 24 hours.</b>" +
                      "<hr/>" +
                      "If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu ." +
                      "<br/>" +
                      "<br/>" +
                      "<span>User Name: </span>" + user.UserName +
                      "<br/>" +
                      "<span>Password: </span>" + model.Password;
                    try
                    {
                        await UserManager.SendEmailAsync(user.Id,
                                               subject, // Subject
                                               body);
                    }
                    catch (Exception ex4)
                    {
                        var ErrorMessage = ex4.Message + ((ex4.InnerException != null) ? ex4.InnerException.Message : "") + "\n";
                        blSiteInfo.CreateSiteInfo("Send Email Error ex4:" + ErrorMessage);
                    }


                    BLEmailLog bLEmailLog = new BLEmailLog();
                    bLEmailLog.CreateEmailLog(new VmEmailLog
                    {
                        RecepientId = user.Id,
                        SenderId = CurrentUserId,
                        SendDate = DateTime.Now,
                        Subject = subject,
                        Body = body,
                        AttachUrl = "",
                    });

                    emailHelper = new EmailHelper
                    {
                        EmailLog = false,
                        SpecialEmail = specialEmail,
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                        EmailList = new string[] { specialEmail }
                    };
                    for (var i = 0; i < emailHelper.EmailList.Length; i++)
                    {
                        emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                    }
                    emailHelper.CurrentUserId = CurrentUserId;
                    emailHelper.Send();

                    if (model.RoleName == SystemRoles.Advisor.ToString() || model.RoleName == SystemRoles.Judge.ToString())
                    {
                        var adminUserId = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().UserId;

                        callbackUrl = Url.Action("arm", "Admin", new { userId = user.Id }, protocol: Request.Url.Scheme);

                        var adminPerson = new BLPerson().GetPersonByUserId("c87419bb-de56-48ae-abba-c56a2692d4cb");

                        body = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                            "<br/>" +
                            "Dear " + adminPerson.FirstName + " " + adminPerson.LastName + ", " +
                            "<br/>" +
                            "<br/>" +
                            "New user has registered on 33rd WERC Environmental Design Contest 2023. " +
                            "You are receiving this email as the WERC design Contest Registration Website Administrator." +
                            "<br/>" +
                            "<b>" + model.FirstName + " " + model.LastName + "</b>" +
                            " has registered as a" +
                            //// " has requested to sign up as a" +
                            "<b>" + (model.RoleName.Contains("Advisor") == true ? " Faculty Advisor " : " Judge. ") + "</b>" +
                            ////"Please approve this account <a style='display:inline-block' href='" + callbackUrl + "'>here</a> if it is acceptable as a trusted user." +
                            ////"<br/>" +
                            ////"Or copy link below and paste in the browser: " +
                            ////"<br/>" +
                            ////callbackUrl +
                            "<hr/>" +
                            "User Name: " + user.UserName +
                            "<br/>" +
                            "Role: " + (model.RoleName.Contains("Advisor") == true ? " Faculty Advisor" : " Judge");

                        try
                        {
                            await UserManager.SendEmailAsync(adminUserId, subject, body);

                        }
                        catch (Exception ex4)
                        {
                            var ErrorMessage = ex4.Message + ((ex4.InnerException != null) ? ex4.InnerException.Message : "") + "\n";
                            blSiteInfo.CreateSiteInfo("Send Email Error ex4:" + ErrorMessage);
                        }

                        bLEmailLog.CreateEmailLog(new VmEmailLog
                        {
                            RecepientId = adminUserId,
                            SenderId = CurrentUserId,
                            SendDate = DateTime.Now,
                            Subject = subject,
                            Body = body,
                            AttachUrl = "",
                        });

                        emailHelper = new EmailHelper
                        {
                            EmailLog = false,
                            SpecialEmail = specialEmail,
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true,
                            EmailList = new string[] { specialEmail }
                        };
                        for (var i = 0; i < emailHelper.EmailList.Length; i++)
                        {
                            emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                        }
                        emailHelper.CurrentUserId = CurrentUserId;
                        emailHelper.Send();
                    }

                    //if (model.RoleName == SystemRoles.Judge.ToString())
                    //{
                    //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //    return RedirectToAction("index", "home");
                    //}

                    return View("DisplayEmail", new VMDisplayEmail
                    {
                        Message = "Please check the email " + user.Email + " and confirm that you initiated this request.",

                        RoleName = model.RoleName
                    });
                }

                AddErrors(result);
            }
            else
            {
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
            }
            //string userName = HttpContext.User.Identity.Name;

            //if (HttpContext.User.IsInRole(SystemRoles.Admin.ToString()))
            //{
            //    var roleList = context.Roles.Where(r => r.Id != "652a69dc-d46c-4cbf-ba28-8e7759b37752").OrderBy(r => r.Name).ToList().Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name }).ToList();
            //    ViewBag.Roles = roleList;
            //    return View("AdminCreateUser", model);

            //}

            // If we got this far, something failed, redisplay form

            if (!string.IsNullOrEmpty(model.ReturnUrl) && model.RoleName != "Student")
            {
                return RedirectToLocal(model.ReturnUrl);
            }

            TempData["LastModelStateErrors"] = null;

            return View(model);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            RegisterDate = DateTime.UtcNow
        //        };

        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            var callbackUrl = Url.Action(
        //               "ConfirmEmail", "Account",
        //               new { userId = user.Id, code = code },
        //               protocol: Request.Url.Scheme);

        //            await UserManager.SendEmailAsync(user.Id,
        //               "Confirm your account",
        //               "Please confirm your account by clicking this link: <a href=\""
        //                                               + callbackUrl + "\">link</a>");
        //            // ViewBag.Link = callbackUrl;   // Used only for initial demo.
        //            return View("DisplayEmail");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code, string returnUrl = "")
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();

            if (userId == null || code == null)
            {
                return View("Error", new VMHandleErrorInfo("Email Confirmation not valid"));
            }


            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var blUser = new BLUser();

            IEnumerable<string> userRoles = null;

            if (user != null)
            {
                SmUserRolesList.UserRoles = blUser.GetAllUserRoles();
                userRoles = (from roles in SmUserRolesList.UserRoles where roles.UserName == user.UserName select roles.RoleName).AsEnumerable<string>();

                TempData["UserRoles"] = userRoles;

                if (user.EmailConfirmed == true)
                {
                    return RedirectToAction("login", "account");
                }
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {

                if (userRoles.Contains(SystemRoles.Advisor.ToString()) || userRoles.Contains(SystemRoles.Judge.ToString()))
                {
                    user.EmailConfirmed = true;
                    UserManager.Update(user);

                    return View("ConfirmEmail", new VMConfirmEmail
                    {
                        Message = "Thank you for confirming your WERC Design Contest 2023 account. \n"// +
                        //"Your account will be approved and active by the WERC administrator within 24 hours."
                    });
                }

                await SignInManager.SignInAsync(user, false, true);

                if (returnUrl != "")
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    CurrentUserId = user.Id;
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                //return View("ConfirmEmail", new VMConfirmEmail());

            }

            if (result.Errors.First().ToLower().Contains("invalid token"))
            {

                code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId, code }, protocol: Request.Url.Scheme);

                var subject = "Confirm your WERC Environmental Design Contest 2023 account.";

                var blPerson = new BLPerson();
                var person = blPerson.GetPersonByUserId(userId);

                var body = "<h1> 33rd WERC Environmental Design Contest 2023" + "</h1>" +  //Body ...
                   "<br/>" +
                   "Dear " + person.FirstName + " " + person.LastName + ", " +
                   "<br/>" +
                   "<br/>" +
                   "Thank you for your interest in the 33rd WERC Environmental Design Contest. We have received your request for access to the online platform." + // Each request requires approval from our system administrator.
                   "<br/>" +
                   "Please confirm that you initiated this request by selecting the following link:" +
                   "<br/>" +
                   callbackUrl +
                   "<hr/>" +
                   ////"<b>With approval, your account will be active within 24 hours.</b>" +
                   ////"<hr/>" +
                   "If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu ." +
                   "<br/>" +
                   "<br/>" +
                   "<span>User Name: </span>" + user.UserName;

                await UserManager.SendEmailAsync(user.Id,
                    subject, // Subject
                    body);

                BLEmailLog bLEmailLog = new BLEmailLog();
                bLEmailLog.CreateEmailLog(new VmEmailLog
                {
                    RecepientId = user.Id,
                    SenderId = CurrentUserId,
                    SendDate = DateTime.Now,
                    Subject = subject,
                    Body = body,
                    AttachUrl = "",
                });

                var emailHelper = new EmailHelper
                {
                    EmailLog = false,
                    SpecialEmail = specialEmail,
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    EmailList = new string[] { specialEmail }
                };
                for (var i = 0; i < emailHelper.EmailList.Length; i++)
                {
                    emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                }
                emailHelper.CurrentUserId = CurrentUserId;
                emailHelper.Send();

                return View("Error", new
                    VMHandleErrorInfo("Confirmation email link has been expired for security reasons. \n New Confirmation email has sent to your email." +
                    "\n" + "If you do not receive the confirmation message within a few minutes of signing up, please check your Spam or Bulk or Junk E - Mail folder just in case the confirmation email got delivered there instead of your inbox. If so, select the confirmation message and mark it Not Spam, which should allow future messages to get through."));
            }

            return View("Error", new VMHandleErrorInfo(result.Errors.First()));

        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();


                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPassword", new ForgotPasswordViewModel("There was a problem We're sorry. We weren't able to identify you given the information provided."));
                }

                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPassword", new ForgotPasswordViewModel("the email " + user.Email + " not confirmed in WERC..."));
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new
                {
                    userId = user.Id,
                    code
                }, protocol: Request.Url.Scheme);

                var blPerson = new BLPerson();
                var person = blPerson.GetPersonByUserId(user.Id);

                var subject = "WERC 2023 Account Password Reset";
                var body = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
                   "<br/>" +
                   "Dear " + person.FirstName + " " + person.LastName + ", " +
                   "<br/>" +
                   "<br/>" +
                   "To reset your password please click <a href=\"" + callbackUrl + "\">here</h2></a>" +
                    "<span><br/> Or copy link below and paste in the browser: </span><br/>" + callbackUrl +

                    "<hr/>" +
                    "If you have questions about the WERC Environmental Design Contest online platform, please call 575 - 646 - 8171 or email wercteams.nmsu.edu.";

                await UserManager.SendEmailAsync(user.Id, subject, body);

                BLEmailLog bLEmailLog = new BLEmailLog();
                bLEmailLog.CreateEmailLog(new VmEmailLog
                {
                    RecepientId = user.Id,
                    SenderId = CurrentUserId,
                    SendDate = DateTime.Now,
                    Subject = subject,
                    Body = body,
                    AttachUrl = "",
                });

                emailHelper = new EmailHelper
                {
                    EmailLog = false,
                    SpecialEmail = specialEmail,
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    EmailList = new string[] { specialEmail }
                };
                for (var i = 0; i < emailHelper.EmailList.Length; i++)
                {
                    emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                }
                emailHelper.CurrentUserId = CurrentUserId;
                emailHelper.Send();

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View(new ForgotPasswordConfirmationViewModel());
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel());
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //ApplicationUser user = context.Users.Where(u => u.UserName.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {

                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);

                    BLPerson blPerson = new BLPerson();
                    VmPerson person = null;

                    person = blPerson.GetPersonByUserId(user.Id);

                    var emailHelper = new EmailHelper
                    {
                        SpecialEmail = specialEmail,
                        Subject = "Reset Password",
                        Body =
                        "Full Name: " + person.FirstName + " " + person.LastName +
                        "<br/>" +
                        "Username: " + user.UserName +
                        "<br/>" +
                        "Password: " + model.Password,
                        IsBodyHtml = true,
                        EmailList = new string[] { specialEmail }
                    };
                    for (var i = 0; i < emailHelper.EmailList.Length; i++)
                    {
                        emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                    }
                    emailHelper.CurrentUserId = CurrentUserId;
                    emailHelper.Send();

                    return RedirectToAction("Index", "Home");
                    //  return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                AddErrors(new IdentityResult(new string[] { "User not found...!" }));
            }


            return View(new ResetPasswordViewModel());

        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View(new BaseViewModel());
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider

            try
            {
                Session["Workaround"] = 0;
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message + " \n " + (ex.InnerException) ?? "";
                //return View("Error", new VMHandleErrorInfo());
                return RedirectToAction("login");
            }
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                //return View("Error", new VMHandleErrorInfo());
                return RedirectToAction("login");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error", new VMHandleErrorInfo());

            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return View("Error", new VMHandleErrorInfo("Problem in Social Signin"));
                //return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home", new { SetWelcomMessage = true });

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, MostSetWelcomeMessage = true });
            }
        }

        ////
        //// GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{

        //    var result = await AuthenticationManager.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie);
        //    if (result == null || result.Identity == null)//here will check if user login done
        //    {
        //        return View("Error", new VMHandleErrorInfo("if (result == null || result.Identity == null)"));
        //        //return RedirectToAction("Login");
        //    }

        //    var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
        //    if (idClaim == null)
        //    {
        //        return View("Error", new VMHandleErrorInfo("idClaim == null"));
        //        //return RedirectToAction("Login");
        //    }

        //    var login = new UserLoginInfo(idClaim.Issuer, idClaim.Value);//here getting login info
        //    var name = result.Identity.Name == null ? "" : result.Identity.Name.Replace(" ", "");//here getting user name

        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return View("Error", new VMHandleErrorInfo("loginInfo == null"));
        //        //return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result1 = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result1)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure", new VMHandleErrorInfo());
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email.Trim().Replace("\t", ""),
                    Email = model.Email.Trim().Replace("\t", ""),
                    RegisterDate = DateTime.UtcNow.Date,
                    EmailConfirmed = true
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {

                    UserManager.AddToRole(user.Id, "Student");
                    var blPerson = new BLPerson();

                    blPerson.CreatePerson(new VmPerson
                    {
                        UserId = user.Id,
                        WelcomeDinner = false,
                        LunchOnMonday = false,
                        LunchOnTuesday = false,
                        ReceptionNetworkOnTuesday = false,
                        AwardBanquet = false,
                        NoneOfTheAbove = false,
                    });

                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["WelcomeMessage"] = null;
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }
        //
        // Get: /Account/LogOff
        [HttpGet]
        public ActionResult LogOut()
        {
            try
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session["WelcomeMessage"] = null;
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login");
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View(new VMHandleErrorInfo());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            TempData["LastModelStateErrors"] = result.Errors;

            foreach (var error in result.Errors)
            {
                string errorMessage = error;
                if (error.EndsWith("is already taken."))
                    errorMessage = errorMessage.Replace("Name ", " User Name ");
                ModelState.AddModelError("", errorMessage);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {

                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);

            }
        }
        #endregion
        [HttpGet]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        [ActionName("sibu")]
        public ActionResult AnonymousLogin(string id)
        {

            ApplicationUser user = context.Users.Where(u => u.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var ident = new ClaimsIdentity(
                new[]
                {
                // adding following 2 claim just for supporting default antiforgery provider
                    new Claim(
                        ClaimTypes.NameIdentifier,user.UserName),
                        new Claim(
                            "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "ASP.NET Identity",
                            "http://www.w3.org/2001/XMLSchema#string"),

                        new Claim(ClaimTypes.Name,user.UserName),
                }, DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
               new AuthenticationProperties { IsPersistent = false }, ident);

            CurrentUserId = user.Id;
            #region user info

            IEnumerable<string> userRoles = null;



            var blUser = new BLUser();
            SmUserRolesList.UserRoles = blUser.GetAllUserRoles();

            userRoles = (from roles in SmUserRolesList.UserRoles where roles.UserId == id select roles.RoleName).AsEnumerable<string>();

            TempData["UserRoles"] = userRoles;

            CurrentUserId = user.Id;
            if (userRoles.Contains(SystemRoles.Admin.ToString()))
            {
                return RedirectToAction("index", "admin");
            }

            if (userRoles.Contains(SystemRoles.SafetyAdmin.ToString()))
            {
                return RedirectToAction("index", "SafetyAdmin");
            }

            if (userRoles.Contains("Advisor"))
            {
                return RedirectToAction("tl", "Advisor");
            }

            if (userRoles.Contains(SystemRoles.Judge.ToString()))
            {
                return RedirectToAction("index", "judge");
            }

            if (userRoles.Contains(SystemRoles.Student.ToString()))
            {
                return RedirectToAction("index", "home");
            }

            if (userRoles.Contains(SystemRoles.Leader.ToString()))
            {
                return RedirectToAction("index", "home");
            }
            if (userRoles.Contains(SystemRoles.CoAdvisor.ToString()))
            {
                return RedirectToAction("index", "home");
            }

            ViewBag.UserRole = "";

            return RedirectToAction("index", "home");

            #endregion
        }
        #region Custom Actoins
        [ChildActionOnly]
        public PartialViewResult Get_ExternalLoginsListPartial(string returnUrl)
        {
            return PartialView("~/Views/Account/_ExternalLoginsListPartial", new ExternalLoginListViewModel() { ReturnUrl = returnUrl });
        }

        #endregion Custom Actoins
    }
}