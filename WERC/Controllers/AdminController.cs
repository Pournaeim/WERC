using BLL;
using BLL.SystemTools;

using ClosedXML.Excel;

using CyberneticCode.Web.Mvc.Helpers;

using DocumentFormat.OpenXml.Office.CustomXsn;

using ICSharpCode.SharpZipLib.Zip;

using Model;
using Model.ViewModels.Admin;
using Model.ViewModels.EmailLog;
using Model.ViewModels.Grade;
using Model.ViewModels.Grade.Report;
using Model.ViewModels.Invoice;
using Model.ViewModels.Judge;
using Model.ViewModels.Person;
using Model.ViewModels.Task;
using Model.ViewModels.Team;
using Model.ViewModels.Test;
using Model.ViewModels.User;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;
using WERC.Filters.CacheFilters;
using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    [RoleBaseAuthorize(SystemRoles.Admin, SystemRoles.SafetyAdmin)]
    public class AdminController : BaseController
    {

        BLPerson blPerson = new BLPerson();
        VmPerson person = null;

        // GET: Admin
        public ActionResult test()
        {

            return View();
        }
        public ActionResult Index()
        {
            var UserRoles = TempData["UserRoles"] as IEnumerable<string>;

            return View(new VmAdmin() { CurrentUserRoles = UserRoles });
        }

        [HttpGet]
        [ActionName("lupf")]
        public ActionResult LoadUpdateProfileForm()
        {
            var blPerson = new BLPerson();
            var vmPerson = blPerson.GetPersonProfileByUserId(CurrentUserId);

            vmPerson.OnActionSuccess = "loadAdminPanel";
            #region Ethincity
            var bsEthnicity = new BLEthnicity();
            var allEthnicityList = bsEthnicity.GetAllEthnicity();

            if (vmPerson.ClientEthnicityIds != null && vmPerson.ClientEthnicityIds.Count() > 0)
            {
                var clientEthnicityIds = vmPerson.ClientEthnicityIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (clientEthnicityIds.Count() > 0)
                {

                    foreach (var item in clientEthnicityIds)
                    {
                        var goalsAfterGraduation = allEthnicityList.Where(t => t.Id == int.Parse(item)).First();
                        goalsAfterGraduation.Checked = "checked";
                    }

                    if (allEthnicityList.Count() > 0)
                    {
                        vmPerson.EthnicityList = allEthnicityList;
                    }
                }
            }
            else
            {
                vmPerson.EthnicityList = allEthnicityList;
            }
            #endregion Ethincity

            return View("UpdateProfile", vmPerson);
        }

        [ActionName("arm")]
        public ActionResult ApprovalRejectManagement()
        {
            return View("ApprovalRejectManagement", new VmApprovalRejectManagement());
        }

        [ActionName("prm")]
        public ActionResult PaymentRulesManagement()
        {
            var blParticipantRule = new BLParticipantRule();
            var ParticipantRuleList = blParticipantRule.GetParticipantRuleList();

            return View("PaymentRulesManagement", new VmPaymentRulesManagement()
            {
                ParticipantRule_BenchScale = ParticipantRuleList.Where(p => p.PaymentTypeId == 1).First(),
                ParticipantRule_Desktop = ParticipantRuleList.Where(p => p.PaymentTypeId == 2).First(),
            });
        }

        [ActionName("srm")]
        public ActionResult SubmissionRulesManagement()
        {
            return View("SubmissionRulesManagement", new VmSubmissionRulesManagement());
        }

        [ActionName("ptm")]
        public ActionResult PaymentTypeManagement()
        {
            return View("PaymentTypeManagement", new VmPaymentTypesManagement());
        }
        [ActionName("ocr")]
        public ActionResult SystemSettingManagement()
        {
            return View("SystemSettingManagement", new VmSystemSettingsManagement());
        }
        [ActionName("mtm")]
        public ActionResult MealTypeManagement()
        {
            return View("MealTypesManagement", new VmMealTypesManagement());
        }
        [ActionName("ssrm")]
        public ActionResult SundryRuleManagement()
        {
            var blSundryRule = new BLSundryRule();

            return View("SundryRuleManagement", blSundryRule.GetSundryRuleById(1));
        }

        [ActionName("unim")]
        public ActionResult UniversityManagement()
        {
            return View("UniversityManagement", new VmUniversityManagement()
            {
            });
        }

        [HttpGet]
        [ActionName("tfim")]
        public ActionResult TeamFullInfoManagement()
        {

            return View("TeamFullInfoManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("lelm")]
        public ActionResult LoadEmailLogManagement()
        {
            return View("EmailLogManagement", new VmTeamFullInfoManagement());
        }

        [HttpPost]
        [ActionName("gel")]
        public ActionResult GetEmailLogs(VmEmailLog filter, int index, int count)
        {
            var blEmailLog = new BLEmailLog();

            var emailLoglist = blEmailLog.GetEmailLogByFilter(filter, index * 100, count).ToList();
            var emailLoglistCount = blEmailLog.GetEmailLogCount();
            if (emailLoglist.Count == 0 && emailLoglistCount > 0)
            {
                emailLoglist = blEmailLog.GetEmailLogByFilter(filter, (index - 1) * 100, count).ToList();
            }
            //foreach (var item in emailLoglist)
            //{
            //    if (item.Body.Length > 32767)
            //    {
            //        item.Body = item.Body.Substring(0, 32767);
            //    }
            //}

            #region Generate Excel

            //var ds = new DataSet();

            //var excelData = from e in emailLoglist
            //                select new
            //                {
            //                    e.Id,
            //                    e.Date,
            //                    e.Subject,
            //                    e.Body,
            //                    e.AttachUrl,
            //                    e.SenderName,
            //                    e.SenderEmail,
            //                    e.SenderUserName,
            //                    e.SenderRoleName,
            //                    e.RecepientName,
            //                    e.RecepientEmail,
            //                    e.RecepientUserName,
            //                    e.RecepientRoleName,
            //                    e.ExcelFileUrl,
            //                };

            //var dt = ExcelHandler.ToDataTable(excelData.ToList());

            //var newTable = ExcelHandler.CreateExcelBaseDataTable(dt,

            //    "Date",
            //    "Subject",
            //    "Body",
            //    "AttachUrl",
            //    "SenderName",
            //    "SenderEmail",
            //    "SenderUserName",
            //    "SenderRoleName",
            //    "RecepientName",
            //    "RecepientEmail",
            //    "RecepientUserName",
            //    "RecepientRoleName"
            //    );

            //ds.Tables.Add(newTable.Copy());

            //var newFilePrefix = "";
            //var fileUrl = string.Empty;
            //var serverPath = string.Empty;
            //var path = "/Resources/Uploaded/Excel/";

            //var date = DateTime.Now;
            //newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            //var fileName = newFilePrefix + "ExcelReport.xlsx";

            //serverPath = Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fileName));

            //fileUrl = Path.Combine(path, fileName.Replace("\\", "/"));

            //var xlWorkbook = ExcelHandler.ExportDataSetToExcel(ds, serverPath);

            //xlWorkbook.Worksheet(1).Columns("e").Width = 30;
            //xlWorkbook.Worksheet(1).RowHeight = 27;

            //foreach (IXLRow row in xlWorkbook.Worksheet(1).Rows())
            //{
            //    foreach (IXLCell cell in row.Cells())
            //    {
            //        row.Style.Alignment.SetShrinkToFit(true);
            //        row.Style.Alignment.ShrinkToFit = true;
            //    }
            //}

            //xlWorkbook.SaveAs(serverPath);

            //foreach (var item in emailLoglist)
            //{
            //    item.ExcelFileUrl = fileUrl;
            //}


            #endregion Generate Excel
            var jsonResult = new
            {
                data = emailLoglist,
                itemsCount = emailLoglistCount
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("tam")]
        public ActionResult TeamActivationManagement()
        {
            CheckSuppressScoring();

            return View("TeamActivationManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("ups_tfim")]
        public ActionResult UpdatePayStatusTeamFullInfoManagement()
        {
            var blInvoice = new BLInvoice();

            var invoiceList = blInvoice.GetInvoiceIds(false);

            if (invoiceList != null)
            {
                foreach (var invoiceId in invoiceList.Ids)
                {

                    int? lastOrderId = 0;

                    var blShopCart = new BLShopCart();
                    var lastOrderInfo = blShopCart.GetCheckoutStatus(invoiceId, out lastOrderId);

                    if (lastOrderInfo != null)
                    {
                        //Update all data in one transaction
                        blInvoice.UpdateInvoiceOrderStatus(lastOrderInfo, invoiceId, true, lastOrderId.Value, true, true);
                    }
                }
            }

            return View("TeamFullInfoManagement", new VmTeamFullInfoManagement());
        }

        [HttpGet]
        [ActionName("tem")]
        public ActionResult TeamEmailManagement()
        {
            var blTeamMember = new BLTeamMember();

            var memberUserIds = blTeamMember.GetAllTeamMembersUserIds();


            return View("TeamEmailManagement", new VmTeamFullInfoManagement()
            {
                MemberUserIds = memberUserIds
            });
        }

        [HttpGet]
        [ActionName("ruem")]
        public ActionResult RoleBaseUserEmailManagement()
        {
            return View("RoleBaseUserEmailManagement", new VmRoleBaseUserEmailManagement());
        }

        [HttpGet]
        [ActionName("jam")]
        public ActionResult JudgeAddressManagement()
        {
            return View("JudgeAddressManagement", new VmRoleBaseUserEmailManagement());
        }

        [HttpGet]
        [ActionName("jtfim")]
        public ActionResult JudgeTaskFullInfoManagement()
        {
            return View("JudgeTaskFullInfoManagement", new VmJudgeTaskFullInfoManagement());
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VmApprovalReject model)
        {
            var result = true;
            var user = UserManager.Users.FirstOrDefault(u => u.Id == model.UserId);
            string returnUrlLink = string.Empty;

            person = blPerson.GetPersonByUserId(model.UserId);


            //returnUrlLink = "/person/up/" + model.UserId;// Update Profile
            returnUrlLink = "";// Update Profile

            var callbackUrl = Url.Action("Login", "Account", new { returnUrl = returnUrlLink }, protocol: Request.Url.Scheme);

            var emailTitle = "33rd WERC Environmental Design Contest 2023";

            var body = "<h2>" + emailTitle + "</h2>" +
            "<br/>" +
            "Dear " + person.FirstName + " " + person.LastName + ", " +
            "<br/>" +
            "<br/>" +
            "<h4>" +
            "Your 33rd WERC Environmental Design Contest 2023 account approved by the WERC administrator. Please sign in to system by clicking " +
            "<a href=\"" + callbackUrl + "\">here </a><span>or copy link below and paste in the browser: </span>" +
            callbackUrl +
            "</h4>" +
            "<hr/>" +
            "<span>User Name: </span>" + user.UserName +
            "<hr/>" +
            "If you have questions about the WERC Environmental Design Contest online platform, please call 575 - 646 - 8171 or email wercteams.nmsu.edu.";


            var subject = "2023 WERC Design Contest Account Approval";

            if (model.Approval == (int)Approval.Reject)
            {
                model.EmailConfirmed = false;
                model.LockoutEnabled = true;

                body = "<h1>" + emailTitle + "</h1>" +
                "<br/>" +
                "Dear " + person.FirstName + " " + person.LastName + ", " +
                "<br/>" +
                "<br/>" +
                "<h2>Your account has been rejected by administrator." +
                "<br/><br/><span>User Name: </span>" + user.UserName;
                subject = "Account Has Been Rejected";
            }
            else
                if (model.Approval == (int)Approval.Approve)
            {
                model.EmailConfirmed = true;
                model.LockoutEnabled = false;
            }
            else
                if (model.Approval == (int)Approval.Pending)
            {
                model.EmailConfirmed = false;
                model.LockoutEnabled = false;


                body = "<h1>" + emailTitle + "</h1>" +
                "<br/>" +
                "Dear " + person.FirstName + " " + person.LastName + ", " +
                "<br/>" +
                "<br/>" +
                "<h2>Your Account has been Set to pending to Approval by Administrator." +
                "<br/><br/><span>User Name: </span>" + user.UserName;

                subject = "Pending for Approval Account";
            }

            user.EmailConfirmed = model.EmailConfirmed;
            user.LockoutEnabled = model.LockoutEnabled;

            await UserManager.UpdateAsync(user);

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

            emailHelper = new EmailHelper()
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

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }


        #region Assining tasks to Judge

        [HttpGet]
        [ActionName("attjm")]
        public ActionResult AssignTaskToJudgeManagement()
        {
            var bsTask = new BLTask();
            var bsPerson = new BLPerson();

            var tasks = bsTask.GetAllTask();
            var judges = bsPerson.GetUsersByRoleNames(new string[] { SystemRoles.Judge.ToString() }).Select(r => new SelectListItem
            {
                Value = r.UserId.ToString(),
                Text = r.Name
            }).ToList();

            return View("AssigningTaskToJudge", new VmAssignTaskToJudgeManagement()
            {
                Tasks = tasks,
                Judges = judges
            });
        }
        [HttpGet]
        [ActionName("attlm")]
        public ActionResult AssignTaskToLabManagement()
        {
            var bsTask = new BLTask();
            var bsPerson = new BLPerson();

            var tasks = bsTask.GetAllTask();
            var labs = bsPerson.GetUsersByRoleNames(new string[] { SystemRoles.Lab.ToString() }).Select(r => new SelectListItem
            {
                Value = r.UserId.ToString(),
                Text = r.Name
            }).ToList();

            return View("AssigningTaskToLab", new VmAssignTaskToLabManagement()
            {
                Tasks = tasks,
                Labs = labs
            });
        }

        [HttpPost]
        [ActionName("gjt")]
        public PartialViewResult GetUserTasksByUser(string userId)
        {
            var blUserTask = new BLUserTask();

            var tasks = blUserTask.GetUserTasksByUser(userId);

            return PartialView("_DropedTaskList", new VmTaskCollection
            {
                TaskList = tasks
            });
        }

        [HttpPost]
        [ActionName("gtjt")]
        public PartialViewResult GetTempUserTasksByUser(string userId)
        {
            var blTempUserTask = new BLTempUserTask();

            var tasks = blTempUserTask.GetTempUserTasksByUser(userId);

            return PartialView("_TempDropedTaskList", new VmTaskCollection
            {
                TaskList = tasks
            });
        }
        [HttpPost]
        [ActionName("glt")]
        public PartialViewResult GetLabUserTasksByUser(string userId)
        {
            var blUserTask = new BLUserTask();

            var tasks = blUserTask.GetUserTasksByUser(userId);

            return PartialView("_DropedTaskList", new VmTaskCollection
            {
                TaskList = tasks
            });
        }

        [HttpPost]
        [ActionName("attj")]
        public ActionResult AssignTasksToJudge(string userId, int[] taskIds)
        {
            var blUserTask = new BLUserTask();

            var result = blUserTask.AssignTasksToUser(userId, taskIds);

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("attl")]
        public ActionResult AssignTasksToLab(string userId, int[] taskIds)
        {
            var blUserTask = new BLUserTask();

            var result = blUserTask.AssignTasksToUser(userId, taskIds);

            var jsonResult = new
            {
                success = result,
                message = "",
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        #endregion Assing tasks to Judge

        #region Lab Result

        [ActionName("lrr")]
        [HttpGet]
        public ActionResult LabResultReport()
        {
            var blTest = new BLTest();

            return View("TeamTestManagementForm", new VmTeamTestManagement
            {
                CurrentUserId = CurrentUserId,
                LabUsers = blTest.GetTeamTaskTestCollections(),
            });
        }

        #endregion Lab Result


        [HttpGet]
        [ActionName("gl")]
        public ActionResult GradeList(int activeItemId = -1)
        {
            var bsGrade = new BLGrade();

            return View("GradeList", new VmGradeCollection
            {
                HtmlControlId = "Admin_GradeList",
                DataAction = "gs",
                DataController = "admin",
                AllowEdit = true,
                AllowDelete = true,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = true,
                ParentHtmlControlId = "GradeList_ParentHtmlControlId",
                OnItemSelected = "",
                GradeList = bsGrade.GetAllGrade()
            });
        }

        [HttpPost]
        [ActionName("gs")]
        public PartialViewResult SearchGrade(
            bool draggable,
            string OnItemSelected,
            bool allowEdit,
            bool allowDelete,
            string htmlControlId,
            string dataAction,
            string dataController,
            string ParentHtmlControlId, string onItemDragged,
            string gradeName = "")
        {
            var bsGrade = new BLGrade();
            var gradeList = bsGrade.GetGradeList(gradeName);

            return PartialView("_GradeList",
                new VmGradeCollection
                {
                    DataAction = dataAction,
                    DataController = dataController,
                    AllowEdit = allowEdit,
                    AllowDelete = allowDelete,
                    Draggable = draggable,
                    ShowSearchBox = true,
                    SearchText = gradeName,
                    GradeList = gradeList,
                    HtmlControlId = htmlControlId,
                    ParentHtmlControlId = ParentHtmlControlId,
                    OnItemSelected = OnItemSelected,
                    OnItemDragged = onItemDragged
                });
        }

        [HttpGet]
        [ActionName("tl")]
        public ActionResult TaskList(int activeItemId = -1)
        {
            var bsTask = new BLTask();

            return View("TaskList", new VmTaskCollection
            {
                HtmlControlId = "Admin_TaskList",
                DataAction = "ts",
                DataController = "admin",
                AllowEdit = true,
                AllowDelete = true,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = false,
                ParentHtmlControlId = "TaskList_ParentHtmlControlId",
                OnItemSelected = "",
                TaskList = bsTask.GetAllTask()
            });
        }
        [HttpGet]
        [ActionName("testl")]
        public ActionResult TestList(int activeItemId = -1)
        {
            var bsTest = new BLTest();

            return View("TestList", new VmTestCollection
            {
                HtmlControlId = "Admin_TestList",
                DataAction = "ts",
                DataController = "admin",
                AllowEdit = true,
                AllowDelete = true,
                ActiveItemId = activeItemId,
                Draggable = false,
                ShowSearchBox = false,
                ParentHtmlControlId = "TestList_ParentHtmlControlId",
                OnItemSelected = "",
                TestList = bsTest.GetAllTest()
            });
        }

        [HttpGet]
        [ActionName("ltestf")]
        public ActionResult LoadTestEditForm(int id)
        {
            var blTest = new BLTest();
            var test = blTest.GetTestById(id);
            test.OnActionSuccess = "loadTestList";

            return View("EditTest", test);
        }
        [HttpGet]
        [ActionName("ltef")]
        public ActionResult LoadTaskEditForm(int id)
        {
            var blTask = new BLTask();
            var task = blTask.GetTaskById(id);
            task.OnActionSuccess = "loadTaskList";

            return View("EditTask", task);
        }

        [HttpGet]
        [ActionName("lgef")]
        public ActionResult LoadGradeEditForm(int id)
        {
            var blGrade = new BLGrade();
            var grade = blGrade.GetGradeWithDetailsById(id);
            grade.OnActionSuccess = "loadGradeList";

            return View("EditGrade", grade);
        }

        [HttpPost]
        [ActionName("ts")]
        public PartialViewResult SearchTask(
            bool draggable,
            string OnItemSelected,
            bool allowEdit,
            bool showSearchBox,
            bool allowDelete,
            string htmlControlId,
            string dataAction,
            string dataController,
            string ParentHtmlControlId, string onItemDragged,
            string taskName = "")
        {
            var bsTask = new BLTask();
            var taskList = bsTask.GetTaskList(taskName);

            return PartialView("_TaskList",
                new VmTaskCollection
                {
                    DataAction = dataAction,
                    DataController = dataController,
                    AllowEdit = allowEdit,
                    AllowDelete = allowDelete,
                    Draggable = draggable,
                    ShowSearchBox = showSearchBox,
                    SearchText = taskName,
                    TaskList = taskList,
                    HtmlControlId = htmlControlId,
                    ParentHtmlControlId = ParentHtmlControlId,
                    OnItemSelected = OnItemSelected,
                    OnItemDragged = onItemDragged
                });
        }

        [HttpPost]
        [ActionName("se")]
        [RoleBaseAuthorize(SystemRoles.Admin, SystemRoles.SafetyAdmin)]
        public ActionResult SendEmail(VmEmail email)
        {
            var result = true;
            var message = "Operation succeeded";
            string adminEmail = "";
            if (email.AdditionalEmails != null)
            {
                if (email.AdditionalEmails.Length == 1 && email.AdditionalEmails[0] == "")
                {
                    email.AdditionalEmails = null;
                }
            }

            List<string> allEmails = new List<string>();

            if (email.UserIds != null && email.UserIds.Length > 0)
            {
                BLPerson blPerson = new BLPerson();
                var emails = blPerson.GetEmailsByUserIds(email.UserIds);

                adminEmail = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().Email;


                allEmails.Add(adminEmail);
                allEmails.AddRange(emails);
            }

            if (email.AdditionalEmails != null)
            {
                allEmails.AddRange(email.AdditionalEmails);
            }

            if (allEmails.Count > 0)
            {
                emailHelper = new EmailHelper
                {
                    AdminEmail = adminEmail,
                    SpecialEmail = specialEmail,
                    SendInBcc = true,
                    Subject = email.EmailSubject,
                    Body = email.EmailBody,
                    IsBodyHtml = true,
                    EmailList = allEmails.ToArray()
                };

                for (var i = 0; i < emailHelper.EmailList.Length; i++)
                {
                    emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                }

                emailHelper.CurrentUserId = CurrentUserId;
                result = emailHelper.Send();

                if (result == false)
                {
                    message = "Operation has been failed" + "\n" +
                       emailHelper.ErrorMessage;
                }
            }
            else
            {
                result = false;
                message = "Users not selected";
            }
            var jsonResult = new
            {
                result,
                success = result,
                message,
            };

            if (result == true)
            {
                emailHelper = new EmailHelper
                {
                    AdminEmail = adminEmail,
                    SpecialEmail = specialEmail,
                    SendInBcc = true,
                    Subject = "List of recipient " + email.EmailSubject,
                    Body = string.Join(", ", allEmails),
                    IsBodyHtml = true,
                    EmailList = new string[] { adminEmail }
                };
                emailHelper.CurrentUserId = CurrentUserId;
                result = emailHelper.Send();

            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ActionName("settm")]
        [RoleBaseAuthorize(SystemRoles.Judge)]
        public ActionResult SendEmailToTeamMembers(int teamId, VmEmail email)
        {
            var result = true;
            var message = "Operation succeeded";
            string adminEmail = "";

            List<string> allEmails = new List<string>();

            var emails = new BLTeamMember().GetTeamMembers(teamId).Select(t => t.Email);

            if (emails != null && emails.Count() > 0)
            {
                adminEmail = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().Email;

                allEmails.Add(adminEmail);
                allEmails.AddRange(emails);
            }

            if (allEmails.Count > 0)
            {
                emailHelper = new EmailHelper
                {
                    AdminEmail = adminEmail,
                    SpecialEmail = specialEmail,
                    SendInBcc = true,
                    Subject = email.EmailSubject,
                    Body = email.EmailBody,
                    IsBodyHtml = true,
                    EmailList = allEmails.ToArray()
                };

                for (var i = 0; i < emailHelper.EmailList.Length; i++)
                {
                    emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
                }

                emailHelper.CurrentUserId = CurrentUserId;
                result = emailHelper.Send();

                if (result == false)
                {
                    message = "Operation has been failed" + "\n" +
                       emailHelper.ErrorMessage;
                }
            }
            else
            {
                result = false;
                message = "Users not selected";
            }
            var jsonResult = new
            {
                result,
                success = result,
                message,
            };

            if (result == true)
            {
                emailHelper = new EmailHelper
                {
                    AdminEmail = adminEmail,
                    SpecialEmail = specialEmail,
                    SendInBcc = true,
                    Subject = "List of recipient " + email.EmailSubject,
                    Body = string.Join(", ", allEmails),
                    IsBodyHtml = true,
                    EmailList = new string[] { adminEmail }
                };
                emailHelper.CurrentUserId = CurrentUserId;
                result = emailHelper.Send();

            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("snsie")]
        [RoleBaseAuthorize(SystemRoles.Admin, SystemRoles.SafetyAdmin)]
        public ActionResult SendNoneScoredItemEmail(string judgeUserId)
        {
            var result = true;
            var message = "Operation succeeded";
            string adminEmail = "";

            List<string> allEmails = new List<string>();

            BLPerson blPerson = new BLPerson();
            person = blPerson.GetPersonByUserId(judgeUserId);

            var emails = blPerson.GetEmailsByUserIds(new string[] { judgeUserId });

            adminEmail = new BLUser().GetUsersByRoleName(SystemRoles.Admin.ToString()).FirstOrDefault().Email;

            allEmails.Add(adminEmail);
            allEmails.AddRange(emails);

            var callbackUrl = Url.Action("lnag", "judge", new
            {
                userId = judgeUserId,
            }, protocol: Request.Url.Scheme);

            var subject = "Non-scored Items Status";
            var body = "<h1>33rd WERC Environmental Design Contest 2023</h1>" +
              "<br/>" +
              "Dear " + person.FirstName + " " + person.LastName + ", " +
              "<br/>" +
              "<br/>" +
              "To complete score items please click <a href=\"" + callbackUrl + "\">here</h2></a>" +
               "<span><br/> Or copy link below and paste in the browser: </span><br/>" + callbackUrl +

               "<hr/>" +
               "If you have questions about the WERC Environmental Design Contest online platform, please call 575 - 646 - 8171 or email wercteams.nmsu.edu.";

            emailHelper = new EmailHelper
            {
                AdminEmail = adminEmail,
                SpecialEmail = specialEmail,
                SendInBcc = true,
                Subject = "Non-scored Items Status",
                Body = body,
                IsBodyHtml = true,
                EmailList = allEmails.ToArray()
            };

            for (var i = 0; i < emailHelper.EmailList.Length; i++)
            {
                emailHelper.EmailList[i] = emailHelper.EmailList[i].Trim();
            }

            emailHelper.CurrentUserId = CurrentUserId;
            result = emailHelper.Send();

            if (result == false)
            {
                message = "Operation has been failed" + "\n" + emailHelper.ErrorMessage;
            }

            if (result == true)
            {
                emailHelper = new EmailHelper
                {
                    AdminEmail = adminEmail,
                    SpecialEmail = specialEmail,
                    SendInBcc = true,
                    Subject = "List of recipient " + subject,
                    Body = string.Join(", ", allEmails),
                    IsBodyHtml = true,
                    EmailList = new string[] { adminEmail }
                };

                emailHelper.CurrentUserId = CurrentUserId;
                result = emailHelper.Send();

            }

            var jsonResult = new
            {
                result,
                success = result,
                message,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("dadou")]
        [RoleBaseAuthorize(SystemRoles.Admin)]
        public ActionResult DeleteAllDataOfUsers(VmEmail email)
        {
            var result = true;
            var message = "Operation succeeded";
            var blUser = new BLUser();

            result = blUser.DeleteAllUsersExecptAdmin();

            if (result == false)
            {
                result = false;
                message = "Operation Failed";
            }

            var jsonResult = new
            {
                result,
                success = result,
                message,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        #region Grading Report

        [HttpGet]
        [ActionName("fgrm")]
        public ActionResult LoadFinalGradesReport()
        {
            var blGrade = new BLGrade();
            /// var gradegReportList = blGrade.GetGradeReportList("");

            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);

            var gradegReportList = blGrade.GetGradeReportListWithoutGradeId("", gradeId).ToList().ToList();

            ///var noneAnsweredGradegReports = blGrade.GetNoneAnsweredGradeReportListBasedOnJudge();
            ViewBag.BS_Title = "Final Score";

            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList
            });
        }
        [HttpGet]
        [ActionName("bsfgrm")]
        public ActionResult BenchScaleLoadFinalGradesReport()
        {
            var blGrade = new BLGrade();

            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);

            var gradegReportList = blGrade.BenchScaleGetGradeReportList("", gradeId).ToList().ToList();

            ViewBag.BS_Title = "Bench Scale Quality Final Score";
            return View("FinalGradeReportManagement", new VmFinalGradeReportManagement
            {
                CurrentUserId = CurrentUserId,
                GradeReportList = gradegReportList
            });
        }

        [HttpGet]
        [ActionName("lnag")]
        public ActionResult NoneAnsweredGrades()
        {
            var blGrade = new BLGrade();
            var noneAnsweredGradegReports = blGrade.GetNoneAnsweredGradeReportListBasedOnJudge();

            return View("NoneAnsweredGrades", new VmNoneAnsweredGradeManagement
            {
                JudgeBaseGrades = noneAnsweredGradegReports
            });
        }


        #endregion Grading Report

        #region Languages
        public ActionResult UploadLanguages()
        {
            UpdateLanguage();
            return View(new VmAdmin());
        }

        public string UpdateLanguage()
        {
            BLDBTools.ImportDataFromExcel(Server.MapPath(@"~/Documents/Dictionary/dictionary.xls"));

            return "Succeed";
        }
        #endregion


        [HttpPost]
        [ActionName("pcpi")]
        public ActionResult PayChequeProcessInvoice(string advisorUserId, int currentTeamId, List<VmTeamSelection> teamSelectionList)
        {
            var result = true;

            var blInvoice = new BLInvoice();
            var finishedInvoice = blInvoice.GetInvoiceByUserId(advisorUserId, true);

            if (finishedInvoice == null)
            {
                teamSelectionList[0].IsFirstTeam = true;
                teamSelectionList[0].Checked = true;
            }
            else
            {
                teamSelectionList[0].IsFirstTeam = false;
                teamSelectionList[0].Checked = true;

            }

            try
            {
                blInvoice.PayChequeProcessInvoice(advisorUserId, currentTeamId, teamSelectionList);

            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                lastStatus = result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("sps")]
        public ActionResult SetPrteliminaryScored(int id)
        {
            var blTeam = new BLTeam();
            var lastStatus = blTeam.UpdateTeamScored(id);

            var jsonData = new
            {
                lastStatus
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [ActionName("dlo")]
        public ActionResult DownloadLogos()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_Logos.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blPerson = new BLPerson();
                var persons = blPerson.GetUsersByRoleNames(new string[] { "Advisor" });

                foreach (var person in persons)
                {
                    if (!string.IsNullOrWhiteSpace(person.UniversityPictureUrl))
                    {
                        try
                        {
                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Universities/Downloads/" + person.Name + "_" + person.Abbreviation + ".jpg");

                            System.IO.File.Copy(Server.MapPath(person.UniversityPictureUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch { }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }

            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("dpho")]
        public ActionResult DownloadPhotos()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_Photos.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blPerson = new BLPerson();
                var persons = blPerson.GetAllPersons();

                foreach (var person in persons)
                {
                    if (!string.IsNullOrWhiteSpace(person.ProfilePictureUrl))
                    {
                        try
                        {
                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + person.Name + ".jpg");

                            System.IO.File.Copy(Server.MapPath(person.ProfilePictureUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("dsrf")]
        public ActionResult DownloadSubmisionRuleFiles(int id)
        {
            string fileName = "";
            string ruleName = "";
            var blSubmissionRule = new BLSubmissionRule();
            var teamsSubmissionRules = blSubmissionRule.GetDownloadTeamsSubmissionRuleById(id);

            if (teamsSubmissionRules.Count() > 0)
            {
                ruleName = teamsSubmissionRules.First().Name.Trim();
                fileName = string.Format("{0}_{1}.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"), ruleName);
            }
            else
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");
            }

            var hasFile = false;

            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();


                foreach (var team in teamsSubmissionRules)
                {

                    try
                    {
                        var temp = team.SubmissionRuleUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                        var fileExtension = temp[temp.Length - 1];

                        var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + ruleName + "_" + team.TeamName.Trim() + "." + fileExtension);

                        System.IO.File.Copy(Server.MapPath(team.SubmissionRuleUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                        ImageList.Add(downloadFileName);

                        hasFile = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);
        }
        [ActionName("dwr")]
        public ActionResult DownloadWrittenReports()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_WrittenReports.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.WrittenReportDate != null)
                    {
                        try
                        {
                            var temp = team.WrittenReportUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "WrittenReport_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.WrittenReportUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("dpr")]
        public ActionResult DownloadPreliminaryReports()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_PreliminaryReports.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.PreliminaryReportDate != null)
                    {
                        try
                        {
                            var temp = team.PreliminaryReportUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "PreliminaryReport_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.PreliminaryReportUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("dft")]
        public ActionResult DownloadFlashTalkReports()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_FlashTalkReports.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.FlashTalkReportDate != null)
                    {
                        try
                        {
                            var temp = team.FlashTalkReportUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "FlashTalkReport_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.FlashTalkReportUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("d_b")]
        public ActionResult DownloadBrochures()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_brochures.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.BrochureDate != null)
                    {
                        try
                        {
                            var temp = team.BrochureUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "brochure_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.BrochureUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }
        [ActionName("d_a")]
        public ActionResult DownloadAwardNominations()
        {
            var hasFile = false;

            var fileName = string.Format("{0}_AwardNominations.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.AwardNominationDate != null)
                    {
                        try
                        {
                            var temp = team.AwardNominationUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "AwardNomination_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.AwardNominationUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }


            return File(finalResult, "application/zip", fileName);

        }

        [ActionName("dottp")]
        public ActionResult DownloadOpenTaskTestPlans()
        {
            var hasFile = false;
            var fileName = string.Format("{0}_OpenTaskTestPlans.zip", DateTime.Today.Date.ToString("dd-MM-yyyy"));
            var tempOutPutPath = Server.MapPath(Url.Content("/Resources/Uploaded/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];

                var ImageList = new List<string>();

                var blTeam = new BLTeam();

                var teams = blTeam.GetTeamList();

                foreach (var team in teams)
                {
                    if (team.OpenTaskTestPlanDate != null)
                    {
                        try
                        {
                            var temp = team.OpenTaskTestPlanUrl.Split(new char[] { '?' })[0].Split(new char[] { '.' });

                            var fileExtension = temp[temp.Length - 1];

                            var downloadFileName = Server.MapPath(@"/Resources/Uploaded/Persons/Downloads/" + "OpenTaskTestPlan_" + team.Name + "." + fileExtension);

                            System.IO.File.Copy(Server.MapPath(team.OpenTaskTestPlanUrl.Split(new char[] { '?' })[0]), downloadFileName, true);

                            ImageList.Add(downloadFileName);
                            hasFile = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);

                    using (FileStream fs = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        }
                        while (sourceBytes > 0);
                    }
                }

                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (hasFile == false || finalResult == null || !finalResult.Any())
            {
                var layout = "~/Views/Shared/_LayoutAdmin.cshtml";

                var vmHandleErrorInfo = new VMHandleErrorInfo
                {
                    CurrentUserId = CurrentUserId,
                    ErrorMessage = "No files found.",
                    ViewLayout = layout,
                };

                TempData["vmHandleErrorInfo"] = vmHandleErrorInfo;

                return RedirectToAction("Error", "Error");

            }

            return File(finalResult, "application/zip", fileName);

        }

        [HttpGet]
        [ActionName("emrf")]
        [NoCache]
        public PartialViewResult ExtraMemberReportForm()
        {
            var blInvoice = new BLInvoice();

            var extraMemberInvoices = new List<VmInvoiceReport>();

            var advisors = new BLPerson().GetUsersByRoleNames(new string[] { GetSystemRolesString(SystemRoles.Advisor) });

            foreach (var item in advisors)
            {
                var invoice = blInvoice.GetInvoiceByUserId(item.UserId, false);

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


                var advisorInvoice = blInvoice.GetExtraMemberInvoiceFullInfoByUserId(item.UserId);

                if (advisorInvoice != null)
                {
                    extraMemberInvoices.Add(new VmInvoiceReport()
                    {
                        Id = advisorInvoice.Id,
                        UserId = advisorInvoice.UserId,
                        Title = advisorInvoice.Title,
                        DateOfIssue = advisorInvoice.DateOfIssue,
                        InvoiceNumber = advisorInvoice.InvoiceNumber,
                        InvoiceTotal = advisorInvoice.InvoiceTotal,
                        AccountOwner = advisorInvoice.AccountOwner,
                        Address = advisorInvoice.Address,
                        InvoiceDetails = advisorInvoice.InvoiceDetails,
                        Subtotal = advisorInvoice.Subtotal,
                        TotalConventionalFee = advisorInvoice.TotalConventionalFee,
                        Tax = advisorInvoice.Tax,
                        Total = advisorInvoice.Total,
                        AmountDue = advisorInvoice.AmountDue,
                        ConventionalFee = advisorInvoice.ConventionalFee,
                        Finished = advisorInvoice.Finished,
                        LastCheckedId = advisorInvoice.LastCheckedId,
                        AllowCheckFisrtTeam = advisorInvoice.AllowCheckFisrtTeam,
                        TransactionNo = advisorInvoice.TransactionNo,
                        Received = advisorInvoice.Received,
                        University = advisorInvoice.University,
                        teamDiscountLowerThan100 = advisorInvoice.teamDiscountLowerThan100,
                        ScholarshipDiscount = advisorInvoice.ScholarshipDiscount,
                        TeamAmountDue = advisorInvoice.TeamAmountDue,
                        Name = item.Name,
                    }
                        );
                }
            }

            if (extraMemberInvoices.Count == 0)
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
                    ErrorMessage = "There is no balance to pay for extra members.",
                    ViewLayout = layout
                });
            }

            return PartialView("ExtraMemberReportManagement",
                new VmInvoiceCollection
                {
                    Invoices = extraMemberInvoices
                });
        }


        [ActionName("sjat")]
        [HttpPost]
        public async Task<JsonResult> SubmitJudgesAssignedTasks(string[] userIdList)
        { //var domainName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host;
            var taskList = "";
            var blUserTask = new BLUserTask();
            var userTaskList = blUserTask.GetUserTasksByUsers(userIdList);

            var domainName = "33rd WERC Environmental Design Contest 2023";

            var subject = "Task Assignment for the 33rd WERC Design Contest";
            var body = "";

            foreach (var userId in userIdList)
            {
                var usertaskList = from u in userTaskList where u.UserId == userId select u;
                var fullName = usertaskList.First().Name;

                taskList = string.Join(",", usertaskList.Select(t => t.TaskName));

                body = "<h1>" + domainName + "</h1>" +
                "Dear " + fullName + ",<br/>" +
                "<br/>" +

                "I would like to thank you in advance for your time and effort that you will put into this design contest.<br/>" +

                "Tasks has been assigned to judges.We've worked so hard trying to accommodate as much as we could your preferences.<br/> " +

                "You will be assigned to judge teams in the following task(s): <strong>" + taskList + "</strong><br/>" +

                "Each team must submit a single PDF of the complete report(including the audits) no later than March 25th, 2023.Late reports will be penalized at a rate of 25 points per day.<br/>" +

                "The guidelines sent to teams about the criteria of their written report can be seen " +
                "<a href='https://iee.nmsu.edu/outreach/events/international-environmental-design-contest/guidelines/written-report/'>here</a>.<br/>" +

                "Another email will be sent shortly to discuss in detail the scoring process.<br/>" +

                "Once again, thank you.<br/>" +
                "<br/>" +
                "Kind Regards, <br/>" +
                "WERC team <br/>" +

                "<br/>" +
                "Environmental Design Contest <br/>" +

                "<br/>" +
                "Phone: 575-646-8171<br/>" +
                "Fax:     575-646-5440<br/>" +

                "<br/>" +
                "College of Engineering<br/>" +
                "1025 Stewart Street<br/>" +
                "MSC Eng NM<br/>" +
                "New Mexico State University<br/>" +
                "P.O. Box 30001<br/>" +
                "Las Cruces, NM 88003-8001<br/>" +

                "<br/>" +
                "<h5 style='color:#8c0b42'>BE BOLD. Shape the Future.</h5>" +
                "<strong style='color:#8c0b42'>New Mexico State University</strong><br/>";

                await UserManager.SendEmailAsync(userId, subject, body);

                BLEmailLog bLEmailLog = new BLEmailLog();
                bLEmailLog.CreateEmailLog(new VmEmailLog
                {
                    RecepientId = userId,
                    SenderId = CurrentUserId,
                    SendDate = DateTime.Now,
                    Subject = subject,
                    Body = body,
                    AttachUrl = "",
                });

                emailHelper = new EmailHelper()
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

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ActionName("gjbtc")]
        public ActionResult GetJudgeBaseTeamComments()
        {
            var blGrade = new BLGrade();
            var judgeBasedTeamGradeDetailList = blGrade.GetJudgeBaseTeamComments();

            ViewBag.ExcelUrl = GetJudgeBaseTeamCommentsExcelFile(judgeBasedTeamGradeDetailList.ToList());

            return View("JudgeBaseTeamComments", new VmNoneAnsweredGradeManagement
            {
                JudgeBaseGrades = judgeBasedTeamGradeDetailList
            });
        }


        public string GetJudgeBaseTeamCommentsExcelFile(List<VmJudgeBaseGrade> judgeBasedTeamGradeDetailList)
        {

            #region Generate Excel

            var table = "<html>" +
                "<head>" +
                "<style> " +
                "table, " +
                "td " +
                "{border:2px solid black}" +
                "table {border-collapse:collapse}" +
                "</style>" +
                "</head>" +
                "<body>" +
                "<table>" +
                "<thead>" +
                "<tr>" +
                "<th>Judge</th>" +
                "<th>Team</th>" +
                "<th>Score Sheet</th>" +
                "<th>Comment</th>" +
                "</thead>" +
                "<tbody>";
            var tr = "";

            foreach (var judge in judgeBasedTeamGradeDetailList)
            {
                tr += $"<tr>" +
                    $"<td>{judge.JudgeName}</td>" +
                    $"<td></td>" +
                    $"<td></td>" +
                    $"<td></td>" +
                    $"</tr>";

                foreach (var team in judge.TeamGradeList)
                {
                    tr += $"<tr>" +
                        $"<td></td>" +
                        $"<td>{team.TeamName}</td>" +
                        $"<td></td>" +
                        $"<td></td>" +
                        $"</tr>";

                    foreach (var scoreSheet in team.GradeReportList)
                    {
                        tr += $"<tr>" +
                        $"<td></td>" +
                        $"<td></td>" +
                        $"<td>{scoreSheet.GradeType}</td>" +
                        $"<td></td>" +
                        $"</tr>";
                        tr += $"<tr>" +
                        $"<td></td>" +
                        $"<td></td>" +
                        $"<td></td>" +
                        $"<td>{scoreSheet.Comment}</td>" +
                        $"</tr>";
                    }
                }
            }

            table += tr + "</tbody></table>";
            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;
            var path = "/Resources/Uploaded/Excel/";
            var date = DateTime.Now;

            newFilePrefix =
                date.Year.ToString("D4") + date.Month.ToString("D2") +
                date.Day.ToString("D2") + date.Hour.ToString("D2") +
                date.Minute.ToString("D2") + date.Second.ToString("D2");

            var fileName = newFilePrefix + "ExcelReport.xls";

            serverPath = Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fileName));

            fileUrl = Path.Combine(path, fileName.Replace("\\", "/"));

            System.IO.File.WriteAllText(serverPath, table );

            #endregion Generate Excel

            return path + "/" + fileName.Replace("\\", "/");
        }


    }
}
