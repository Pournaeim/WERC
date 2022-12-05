using BLL;

using Model.Base;
using Model.ViewModels.AspNetUsers2;
using Model.ViewModels.EmailLog;
using Model.ViewModels.Person;
using Model.ViewModels.Team;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers.TeamMembers
{
    public class TeamMemberController : BaseController
    {
        BLPerson blPerson = new BLPerson();
        VmPerson person = null;

        [HttpGet]
        [ActionName("gtbf")]
        public JsonResult GetTeamMembersByFilter(int teamId, VmTeamMember filterItem = null)
        {
            var blTeamMember = new BLTeamMember();
            var teamMemberList = blTeamMember.GetTeamMembersByFilter(filterItem, teamId);

            return Json(teamMemberList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("shtm")]
        public PartialViewResult ShowTeamMember(int teamId)
        {
            var blTeamMember = new BLTeamMember();
            var teamMemberList = blTeamMember.GetTeamMembers(teamId);

            return PartialView("_TeamMember", teamMemberList);
        }

        [HttpPost]
        [ActionName("shtem")]
        public PartialViewResult ShowTeamEmailMember(int teamId)
        {
            var blTeamMember = new BLTeamMember();

            var teamMemberList = blTeamMember.GetTeamMembers(teamId);
            return PartialView("_TeamEmailMember", teamMemberList);
        }

        [HttpPost]
        [ActionName("gtmu_ids")]
        public ActionResult GetTeamMembersUserIds(int teamId)
        {
            var blTeamMember = new BLTeamMember();

            var userIds = blTeamMember.GetTeamMembersUserIds(teamId);
            var jsonResult = new
            {
                success = true,
                message = "",
                userIds,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create(VmTeamMember model)
        {
            var result = true;
            var blTeamMember = new BLTeamMember();
            try
            {

                var AuthenticationCode = BLHelper.GenerateRandomNumber(100000, 999999).ToString();
                var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.Email.Trim().Replace("\t", ""),
                        Email = model.Email.Trim().Replace("\t", ""),
                        RegisterDate = DateTime.UtcNow,
                        LastSignIn = DateTime.UtcNow,
                        EmailConfirmed = true,
                    };

                    var createResult = await UserManager.CreateAsync(user, AuthenticationCode);

                    if (createResult.Succeeded)
                    {
                        BLAspNetUsers2 blAspNetUsers2 = new BLAspNetUsers2();
                        blAspNetUsers2.CreateAspNetUsers2(new VmAspNetUsers2
                        {
                            UserId = user.Id,
                            Password = AuthenticationCode
                        });

                        var leaderOrCoAdvisor = "Team Leader";

                        if (model.IsCoAdvisor == true)
                        {
                            await UserManager.AddToRoleAsync(user.Id, "CoAdvisor");
                            await UserManager.RemoveFromRoleAsync(user.Id, "Student");
                            await UserManager.RemoveFromRoleAsync(user.Id, "Leader");
                            leaderOrCoAdvisor = "Co-Advisor";
                        }

                        model.MemberUserId = user.Id;


                        result = blTeamMember.CreateTeamMember(model) != -1 ? true : false;

                        if (result == false)
                        {
                            await UserManager.DeleteAsync(user);
                        }
                        else
                        {

                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                            var callbackUrl = Url.Action("login", "account", new { userId = user.Id, returnUrl = "" }, protocol: Request.Url.Scheme);

                            //var domainName = callbackUrl.Split('/')[2];
                            var title = "33rd WERC Environmental Design Contest 2023";
                            var emailSubject = "";
                            var emailBody = "";

                            emailSubject = "Your 2023 WERC Design Contest Account Created";
                            emailBody = "<h1>" + title + "</h1>" +

                           "<br/>" +
                           "Dear " + model.FirstName + " " + model.LastName + ", " +
                           "<br/>" +
                           "<br/>" +

                            "You have been successfully added as a team member for the 33rd annual WERC Design Contest. " +
                            "Below is your username and password. Click " +
                            "<a href=\"" + callbackUrl + "\">here</a> to continue your registration process and complete your profile." +
                            "Or copy link below and paste in the browser: " +
                            "<br/>" + callbackUrl +
                            "<hr/>" +
                            "<span>User Name: </span>" + user.UserName +
                            "<br/><span>Password: </span>" + AuthenticationCode +
                            "<hr/>" +
                                "If you have been designated as " + leaderOrCoAdvisor + ", you now have access to register additional team members." +
                                "<hr/>" +
                                "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";

                            try
                            {
                                await UserManager.SendEmailAsync(model.MemberUserId, emailSubject, emailBody);

                                BLEmailLog bLEmailLog = new BLEmailLog();
                                bLEmailLog.CreateEmailLog(new VmEmailLog
                                {
                                    RecepientId = model.MemberUserId,
                                    SenderId = CurrentUserId,
                                    SendDate = DateTime.Now,
                                    Subject = emailSubject,
                                    Body = emailBody,
                                    AttachUrl = "",
                                });

                                emailHelper = new EmailHelper
                                {
                                    EmailLog = false,
                                    Subject = emailSubject,
                                    Body = emailBody,
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
                            catch
                            {

                            }

                            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            if (model.IsTeamLeader == true && model.CanChangeLeader == true)
                            {
                                await UserManager.AddToRoleAsync(model.MemberUserId, "Leader");

                                var teamMemberList = blTeamMember.GetTeamMembers(model.TeamId);

                                var oldLeader = teamMemberList.SingleOrDefault(m => m.RoleName == "Leader" && m.MemberUserId != model.MemberUserId);

                                if (oldLeader != null)
                                {
                                    await UserManager.RemoveFromRolesAsync(oldLeader.MemberUserId, "Leader");
                                    await UserManager.AddToRoleAsync(oldLeader.MemberUserId, "Student");

                                    person = blPerson.GetPersonByUserId(oldLeader.MemberUserId);

                                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                                    emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                                    emailBody = "<h1>" + title + "</h1>" +

                                       "<br/>" +
                                       "Dear " + person.FirstName + " " + person.LastName + ", " +
                                       "<br/>" +
                                       "<br/>" +

                                       "Your role on your team for the WERC Design Contest 2023 has changed. You are now a team member." +
                                       "<hr/>" +
                                       "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";

                                    try
                                    {
                                        await UserManager.SendEmailAsync(oldLeader.MemberUserId, emailSubject, emailBody);

                                        BLEmailLog bLEmailLog = new BLEmailLog();
                                        bLEmailLog.CreateEmailLog(new VmEmailLog
                                        {
                                            RecepientId = oldLeader.MemberUserId,
                                            SenderId = CurrentUserId,
                                            SendDate = DateTime.Now,
                                            Subject = emailSubject,
                                            Body = emailBody,
                                            AttachUrl = "",
                                        });

                                        emailHelper = new EmailHelper
                                        {
                                            EmailLog = false,
                                            Subject = emailSubject,
                                            Body = emailBody,
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
                                    catch
                                    {

                                    }


                                }
                            }
                            else
                            {
                                if (model.IsCoAdvisor == false)
                                {
                                    await UserManager.AddToRoleAsync(model.MemberUserId, "Student");
                                }
                            }

                        }
                    }
                    else
                    {

                        result = false;
                        var userJsonResult = new
                        {
                            message = createResult.Errors.First(),
                            success = false,
                        };

                        return Json(userJsonResult, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    result = false;
                    var userJsonResult = new
                    {
                        message = "Email, " + model.Email + " is already taken.",
                        success = false,
                    };

                    return Json(userJsonResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = false;
                var userJsonResult = new
                {
                    message = "Create user operation has failed.",
                    success = false,
                };
                return Json(userJsonResult, JsonRequestBehavior.AllowGet);
            }


            if (result == true)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            var jsonResult = new
            {
                message = "Operation has failed.",
                success = false,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VmTeamMember model)
        {
            var title = "33rd WERC Environmental Design Contest 2023";
            var emailSubject = "";
            var emailBody = "";

            var result = true;
            var blTeamMember = new BLTeamMember();

            var teamMemberList = blTeamMember.GetTeamMembers(model.TeamId);

            if (model.IsCoAdvisor == true)
            {
                await UserManager.AddToRoleAsync(model.MemberUserId, "CoAdvisor");
                await UserManager.RemoveFromRolesAsync(model.MemberUserId, "Student");
                await UserManager.RemoveFromRolesAsync(model.MemberUserId, "Leader");
                person = blPerson.GetPersonByUserId(model.MemberUserId);

                emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                emailBody = "<h1>" + title + "</h1>" +

                "<br/>" +
                "Dear " + person.FirstName + " " + person.LastName + ", " +
                "<br/>" +
                "<br/>" +

                "Your role on your team for the WERC Design Contest 2023 has changed. You are now a co-advisor." +
                "<hr/>" +
                "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";


                try
                {
                    await UserManager.SendEmailAsync(model.MemberUserId, emailSubject, emailBody);

                    BLEmailLog bLEmailLog = new BLEmailLog();
                    bLEmailLog.CreateEmailLog(new VmEmailLog
                    {
                        RecepientId = model.MemberUserId,
                        SenderId = CurrentUserId,
                        SendDate = DateTime.Now,
                        Subject = emailSubject,
                        Body = emailBody,
                        AttachUrl = "",
                    });

                    emailHelper = new EmailHelper
                    {
                        EmailLog = false,
                        Subject = emailSubject,
                        Body = emailBody,
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
                catch
                {

                }

            }
            else
            if (model.RoleName != "CoAdvisor" && model.RoleName != "Advisor" && model.CanChangeLeader == true)
            {

                var callbackUrl = Url.Action("login", "account", new { userId = model.MemberUserId, returnUrl = "" }, protocol: Request.Url.Scheme);


                var oldLeader = teamMemberList.SingleOrDefault(m => m.RoleName == "Leader");

                if (oldLeader == null)
                {
                    if (model.IsTeamLeader == true)
                    {
                        await UserManager.RemoveFromRolesAsync(model.MemberUserId, "Student");
                        await UserManager.AddToRoleAsync(model.MemberUserId, "Leader");

                        person = blPerson.GetPersonByUserId(model.MemberUserId);

                        emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                        emailBody = "<h1>" + title + "</h1>" +

                        "<br/>" +
                        "Dear " + person.FirstName + " " + person.LastName + ", " +
                        "<br/>" +
                        "<br/>" +

                        "Your role on your team for the WERC Design Contest 2023 has changed. You are now a team leader." +
                        "<hr/>" +
                        "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";

                        try
                        {
                            await UserManager.SendEmailAsync(model.MemberUserId, emailSubject, emailBody);

                            BLEmailLog bLEmailLog = new BLEmailLog();
                            bLEmailLog.CreateEmailLog(new VmEmailLog
                            {
                                RecepientId = model.MemberUserId,
                                SenderId = CurrentUserId,
                                SendDate = DateTime.Now,
                                Subject = emailSubject,
                                Body = emailBody,
                                AttachUrl = "",
                            });

                            emailHelper = new EmailHelper
                            {
                                EmailLog = false,
                                Subject = emailSubject,
                                Body = emailBody,
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
                        catch
                        {

                        }


                    }
                }
                else if (oldLeader.MemberUserId != model.MemberUserId && model.IsTeamLeader == true)
                {

                    await UserManager.RemoveFromRolesAsync(model.MemberUserId, "Student");
                    await UserManager.AddToRoleAsync(model.MemberUserId, "Leader");

                    person = blPerson.GetPersonByUserId(model.MemberUserId);

                    emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                    emailBody = "<h1>" + title + "</h1>" +

                    "<br/>" +
                    "Dear " + person.FirstName + " " + person.LastName + ", " +
                    "<br/>" +
                    "<br/>" +

                    "Your role on your team for the WERC Design Contest 2023 has changed. You are now a team leader." +
                        "<hr/>" +
                        "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";

                    try
                    {
                        await UserManager.SendEmailAsync(model.MemberUserId, emailSubject, emailBody);

                        BLEmailLog bLEmailLog = new BLEmailLog();
                        bLEmailLog.CreateEmailLog(new VmEmailLog
                        {
                            RecepientId = model.MemberUserId,
                            SenderId = CurrentUserId,
                            SendDate = DateTime.Now,
                            Subject = emailSubject,
                            Body = emailBody,
                            AttachUrl = "",
                        });

                        emailHelper = new EmailHelper
                        {
                            EmailLog = false,
                            Subject = emailSubject,
                            Body = emailBody,
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
                    catch
                    {

                    }


                    await UserManager.RemoveFromRolesAsync(oldLeader.MemberUserId, "Leader");
                    await UserManager.AddToRoleAsync(oldLeader.MemberUserId, "Student");


                    person = blPerson.GetPersonByUserId(oldLeader.MemberUserId);

                    emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                    emailBody = "<h1>" + title + "</h1>" +

                    "<br/>" +
                    "Dear " + person.FirstName + " " + person.LastName + ", " +
                    "<br/>" +
                    "<br/>" +

                    "Your role on your team for the WERC Design Contest 2023 has changed. You are now a team member." +
                    "<hr/>" +
                    "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";


                    try
                    {
                        await UserManager.SendEmailAsync(oldLeader.MemberUserId, emailSubject, emailBody);

                        BLEmailLog bLEmailLog = new BLEmailLog();
                        bLEmailLog.CreateEmailLog(new VmEmailLog
                        {
                            RecepientId = oldLeader.MemberUserId,
                            SenderId = CurrentUserId,
                            SendDate = DateTime.Now,
                            Subject = emailSubject,
                            Body = emailBody,
                            AttachUrl = "",
                        });

                        emailHelper = new EmailHelper
                        {
                            EmailLog = false,
                            Subject = emailSubject,
                            Body = emailBody,
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
                    catch
                    {

                    }


                }
                else if (oldLeader.MemberUserId == model.MemberUserId && model.IsTeamLeader == false)
                {
                    await UserManager.RemoveFromRolesAsync(model.MemberUserId, "Leader");
                    await UserManager.AddToRoleAsync(model.MemberUserId, "Student");

                    person = blPerson.GetPersonByUserId(model.MemberUserId);

                    emailSubject = "Your role on your team for the WERC Design Contest 2023 has changed.";
                    emailBody = "<h1>" + title + "</h1>" +

                    "<br/>" +
                    "Dear " + person.FirstName + " " + person.LastName + ", " +
                    "<br/>" +
                    "<br/>" +

                    "Your role on your team for the WERC Design Contest 2023 has changed. You are now a team member." +
                    "<hr/>" +
                    "<b>If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu .<b/>";

                    try
                    {
                        await UserManager.SendEmailAsync(model.MemberUserId, emailSubject, emailBody);

                        BLEmailLog bLEmailLog = new BLEmailLog();
                        bLEmailLog.CreateEmailLog(new VmEmailLog
                        {
                            RecepientId = model.MemberUserId,
                            SenderId = CurrentUserId,
                            SendDate = DateTime.Now,
                            Subject = emailSubject,
                            Body = emailBody,
                            AttachUrl = "",
                        });

                        emailHelper = new EmailHelper
                        {
                            EmailLog = false,
                            Subject = emailSubject,
                            Body = emailBody,
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
                    catch
                    {

                    }


                }
            }

            result = blTeamMember.UpdateTeamMember(model);

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [ActionName("cotlie")]
        [HttpPost]
        public ActionResult CheckOtherTeamLeaderIsExist(bool isTeamLeader, int teamId, string teamMemberUserId = null)
        {

            var result = false;
            var message = "";
            var blTeamMember = new BLTeamMember();

            if (isTeamLeader == true)
            {
                VmTeamMember teamMember = null;
                if (!string.IsNullOrEmpty(teamMemberUserId))
                {
                    teamMember = blTeamMember.GetTeamMemberByUserId(teamMemberUserId);
                    if (teamMember.RoleName != SystemRoles.Leader.ToString())
                    {
                        result = blTeamMember.CheckOtherTeamLeaderIsExist(teamId, teamMemberUserId);
                    }
                }
                else
                {
                    result = blTeamMember.CheckOtherTeamLeaderIsExist(teamId);
                }

                if (result == true)
                {
                    message = "This team already has a team leader, " +
                                        "are you sure you want to change the leadership " +
                                        "responsibility to another member? ";
                }
            }

            var jsonResult = new
            {
                success = result,
                message,
            };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        // GET: TeamMember/Delete/5
        public async Task<ActionResult> Delete(VmTeamMember model)
        {
            var result = true;

            //var blTeamMember = new BLTeamMember();

            //result = blTeamMember.DeleteTeamMember(model.Id);

            string resultMessage = string.Empty;
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.Id == model.MemberUserId);

                resultMessage = new BaseViewModel()["TeamMember Has been deleted successfuly."];
                person = blPerson.GetPersonByUserId(model.MemberUserId);

                var title = "33rd WERC Environmental Design Contest 2023";
                var emailSubject = "Your WERC Design Contest 2023 membership account has been removed.";
                var emailBody = "<h1>" + title + "</h1>" +
                    "<br/>" +
                "Dear " + person.FirstName + " " + person.LastName + ", " +
                "<br/>" +
                "<br/>" +

                "Your WERC Design Contest 2023 membership account has been removed." +
                "<hr/>" +
                "If you have questions about the WERC Environmental Design Contest online platform, please call 575-646-8171 or email wercteams.nmsu.edu.";

                try
                {
                    await UserManager.SendEmailAsync(user.Id, emailSubject, emailBody);

                    BLEmailLog bLEmailLog = new BLEmailLog();
                    bLEmailLog.CreateEmailLog(new VmEmailLog
                    {
                        RecepientId = user.Id,
                        SenderId = CurrentUserId,
                        SendDate = DateTime.Now,
                        Subject = emailSubject,
                        Body = emailBody,
                        AttachUrl = "",
                    });

                    emailHelper = new EmailHelper
                    {
                        EmailLog = false,
                        Subject = emailSubject,
                        Body = emailBody,
                        IsBodyHtml = true,
                        EmailList = new string[] { specialEmail }
                    };
                    for (var i = 0; i < emailHelper.EmailList.Length; i++)
                    {
                        emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                    }
                    emailHelper.CurrentUserId = CurrentUserId;
                    emailHelper.Send();


                    await UserManager.DeleteAsync(user);
                }
                catch
                {

                }


            }
            catch (Exception ex)
            {
                resultMessage = new BaseViewModel()["user not deleted, call system Administrator."];
                result = false;
            }

            var jsonResult = new
            {
                success = result,
                message = resultMessage,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

    }
}