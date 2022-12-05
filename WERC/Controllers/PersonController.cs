using BLL;

using ClosedXML.Excel;

using CyberneticCode.Web.Mvc.Helpers;

using Model;
using Model.ViewModels.Admin;
using Model.ViewModels.Person;

using Newtonsoft.Json;

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using static Model.ApplicationDomainModels.ConstantObjects;

using Image = System.Drawing.Image;

namespace WERC.Controllers
{
    public class PersonController : BaseController
    {

        [HttpPost]
        [ActionName("gpi")]
        public PartialViewResult GetProfileInfo(string userId)
        {
            var blPerson = new BLPerson();
            var profile = blPerson.GetPersonByUserId(userId);

            //if (profile.RoleName == SystemRoles.Judge.ToString())
            //{
            //    profile.HideEmergency = true;
            //}

            return PartialView("_ProfileInfo", profile);
        }

        [ActionName("gaddl")]
        public ActionResult GetAdvisorList()
        {
            var blPerson = new BLPerson();

            var advisorList = blPerson.GetUsersByRoleNameListItem(
                new string[] {
                    SystemRoles.Advisor.ToString(),
                });

            return Json(advisorList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [ActionName("gapbf")]
        public JsonResult GetAdvisorPersonMembersByFilter(VmApprovalReject filterItem = null)
        {
            var blPerson = new BLPerson();
            var vmApprovalRejectList = blPerson.GetUsersByFilterAndRoleNames(
                new string[] {
                    SystemRoles.Advisor.ToString(),
                },
                filterItem);

            return Json(vmApprovalRejectList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [ActionName("gjpbf")]
        public JsonResult GetJudgePersonMembersByFilter(VmApprovalReject filterItem = null)
        {
            var blPerson = new BLPerson();
            var vmApprovalRejectList = blPerson.GetUsersByFilterAndRoleNames(
                new string[] {
                    SystemRoles.Judge.ToString(),
                },
                filterItem);

            return Json(vmApprovalRejectList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("grbuebf")]
        public JsonResult GetRoleBaseUserEmailByFilter(VmPerson filter = null)
        {

            var blPerson = new BLPerson();

            var userFullInfoList = blPerson.GetUsersFulInfoByFilter(filter);
            foreach (var item in userFullInfoList)
            {
                item.StreetLine1 =
                            (!string.IsNullOrEmpty(item.StreetLine1) ? item.StreetLine1 + "<br/>" : "") +
                            (!string.IsNullOrEmpty(item.StreetLine2) ? item.StreetLine2 + "<br/>" : "") +
                            (!string.IsNullOrEmpty(item.City) ? item.City + "<br/>" : "") +
                            (!string.IsNullOrEmpty(item.State) ? item.State + "<br/>" : "") +
                            (!string.IsNullOrEmpty(item.ZipCode) ? item.ZipCode + "<br/>" : "");
            }
            #region Generate Excel

            var ds = new DataSet();

            foreach (var person in userFullInfoList)
            {
                if (person.EthnicityList != null)
                {
                    person.Ethnicities = string.Join("\n", person.EthnicityList.Select(e => e.Name));
                }

                if (person.GoalsAfterGraduationList != null)
                {
                    person.GoalsAfterGraduations = string.Join("\n", person.GoalsAfterGraduationList.Select(g => g.Name));
                }

            }
            var excelData = from e in userFullInfoList
                            select new
                            {
                                e.FirstName,
                                e.LastName,
                                e.Email,
                                Gender = (e.Sex == null || e.Sex == true) ? "Female" : "Male",
                                e.ShortBio,
                                e.University,
                                e.JacketSize,
                                e.T_Shirt_Size,
                                e.PhoneNumber,
                                Address = e.StreetLine1.Replace("<br/>", Environment.NewLine),
                                e.RoleName,
                                e.DietType,
                                Allergies = (string.IsNullOrWhiteSpace(e.Allergies)) ? "" : e.Allergies.Replace("<br/>", Environment.NewLine),
                                TeamName = (string.IsNullOrWhiteSpace(e.TeamName)) ? "" : e.TeamName.Replace("<br/>", Environment.NewLine),
                                Tasks = (string.IsNullOrWhiteSpace(e.Tasks)) ? "" : e.Tasks.Replace("<br/>", Environment.NewLine),
                                e.ExcelFileUrl,
                                IEEEMembership = e.IEEEMembership,
                                e.Ethnicities  ,
                                e.GoalsAfterGraduations  ,
                                e.GoalsAfterGraduationSite
                            };

           
            var dt = ExcelHandler.ToDataTable(excelData.ToList());

            var newTable = ExcelHandler.CreateExcelBaseDataTable(dt,

                "FirstName",
                "LastName",
                "Email",
                "Gender",
                "ShortBio",
                "University",
                "JacketSize",
                "T_Shirt_Size",
                "PhoneNumber",
                "Address",
                "RoleName",
                "DietType",
                "Allergies",
                "TeamName",
                "Tasks",
                "IEEEMembership",
                "Ethnicities",
                "GoalsAfterGraduations",
                "GoalsAfterGraduationSite");
            
            newTable.Columns["Ethnicities"].ColumnName = "Race/Ethnicity";
            newTable.Columns["GoalsAfterGraduations"].ColumnName = "How heard about the contest";
            newTable.Columns["GoalsAfterGraduationSite"].ColumnName = "Having read about it online(the site)";
            
            ds.Tables.Add(newTable.Copy());

            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;
            var path = "/Resources/Uploaded/Excel/";

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + "ExcelReport.xlsx";

            serverPath = Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fileName));

            fileUrl = Path.Combine(path, fileName.Replace("\\", "/"));


            #region calculate statistics


            var T_ShirtSizeStatisticDataTable = new DataTable();

            T_ShirtSizeStatisticDataTable.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn{  ColumnName = "T-SHIRT"},
                    new DataColumn{  ColumnName="S" },
                    new DataColumn{  ColumnName="M" },
                    new DataColumn{  ColumnName="L" },
                    new DataColumn{  ColumnName="XL" },
                    new DataColumn{  ColumnName="XXL" },
                    new DataColumn{  ColumnName="3XL" },
                    new DataColumn{  ColumnName="4XL" },
                    new DataColumn{  ColumnName="Total Role" },
                });

            var roles = new[] {
                "Student",
                "Leader",
                "Advisor",
                "CoAdvisor",
                "Judge",
            };

            var SCount = 0;
            var MCount = 0;
            var LCount = 0;
            var XLCount = 0;
            var XXLCount = 0;
            var _3XLCount = 0;
            var _4XLCount = 0;
            var RoleCount = 0;

            foreach (var role in roles)
            {

                var S = excelData.Count(s => s.T_Shirt_Size.ToLower() == "s" && s.RoleName.ToLower() == role.ToLower());
                var M = excelData.Count(s => s.T_Shirt_Size.ToLower() == "m" && s.RoleName.ToLower() == role.ToLower());
                var L = excelData.Count(s => s.T_Shirt_Size.ToLower() == "l" && s.RoleName.ToLower() == role.ToLower());
                var XL = excelData.Count(s => s.T_Shirt_Size.ToLower() == "xl" && s.RoleName.ToLower() == role.ToLower());
                var XXL = excelData.Count(s => s.T_Shirt_Size.ToLower() == "xxl" && s.RoleName.ToLower() == role.ToLower());
                var _3XL = excelData.Count(s => s.T_Shirt_Size.ToLower() == "3xl" && s.RoleName.ToLower() == role.ToLower());
                var _4XL = excelData.Count(s => s.T_Shirt_Size.ToLower() == "4xl" && s.RoleName.ToLower() == role.ToLower());


                var roleCount = excelData.Count(s => s.RoleName.ToLower() == role.ToLower());
                RoleCount += roleCount;

                T_ShirtSizeStatisticDataTable.Rows.Add(role, S, M, L, XL, XXL, _3XL, _4XL, roleCount);

                SCount += S;
                MCount += M;
                LCount += L;
                XLCount += XL;
                XXLCount += XXL;
                _3XLCount += _3XL;
                _4XLCount += _4XL;
            }

            T_ShirtSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");
            T_ShirtSizeStatisticDataTable.Rows.Add("Sum", SCount, MCount, LCount, XLCount, XXLCount, _3XLCount, _4XLCount, RoleCount);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var JacketSizeStatisticDataTable = new DataTable();

            JacketSizeStatisticDataTable.Columns.AddRange(
                new DataColumn[] {
                    new DataColumn{  ColumnName = "Jacket"},
                    new DataColumn{  ColumnName="S" },
                    new DataColumn{  ColumnName="M" },
                    new DataColumn{  ColumnName="L" },
                    new DataColumn{  ColumnName="XL" },
                    new DataColumn{  ColumnName="XXL" },
                    new DataColumn{  ColumnName="3XL" },
                    new DataColumn{  ColumnName="4XL" },
                    new DataColumn{  ColumnName="Total Role" },
                });
            var jacketRoles = new[] {
                "Advisor",
                "CoAdvisor",
                "Judge",
            };

            SCount = 0;
            MCount = 0;
            LCount = 0;
            XLCount = 0;
            XXLCount = 0;
            _3XLCount = 0;
            _4XLCount = 0;
            RoleCount = 0;

            var jacketSCount = 0;
            var jacketMCount = 0;
            var jacketLCount = 0;
            var jacketXLCount = 0;
            var jacketXXLCount = 0;
            var jacket_3XLCount = 0;
            var jacket_4XLCount = 0;
            var maleJacketRoleCount = 0;
            var femaleJacketRoleCount = 0;

            foreach (var role in jacketRoles)
            {

                var S = excelData.Count(s => s.JacketSize.ToLower() == "s" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var M = excelData.Count(s => s.JacketSize.ToLower() == "m" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var L = excelData.Count(s => s.JacketSize.ToLower() == "l" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var XL = excelData.Count(s => s.JacketSize.ToLower() == "xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var XXL = excelData.Count(s => s.JacketSize.ToLower() == "xxl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var _3XL = excelData.Count(s => s.JacketSize.ToLower() == "3xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                var _4XL = excelData.Count(s => s.JacketSize.ToLower() == "4xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");

                var roleCount = excelData.Count(s => s.RoleName.ToLower() == role.ToLower() && s.Gender == "Female");
                femaleJacketRoleCount += roleCount;

                JacketSizeStatisticDataTable.Rows.Add(role + "(Female)", S, M, L, XL, XXL, _3XL, _4XL, roleCount);

                SCount += S;
                MCount += M;
                LCount += L;
                XLCount += XL;
                XXLCount += XXL;
                _3XLCount += _3XL;
                _4XLCount += _4XL;

                jacketSCount += S;
                jacketMCount += M;
                jacketLCount += L;
                jacketXLCount += XL;
                jacketXXLCount += XXL;
                jacket_3XLCount += _3XL;
                jacket_4XLCount += _4XL;


            }

            JacketSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");
            JacketSizeStatisticDataTable.Rows.Add("Sum(Female)", jacketSCount, jacketMCount, jacketLCount, jacketXLCount, jacketXXLCount, jacket_3XLCount, jacket_4XLCount, femaleJacketRoleCount);
            JacketSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");

            jacketSCount = 0;
            jacketMCount = 0;
            jacketLCount = 0;
            jacketXLCount = 0;
            jacketXXLCount = 0;
            jacket_3XLCount = 0;
            jacket_4XLCount = 0;


            foreach (var role in jacketRoles)
            {

                var S = excelData.Count(s => s.JacketSize.ToLower() == "s" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var M = excelData.Count(s => s.JacketSize.ToLower() == "m" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var L = excelData.Count(s => s.JacketSize.ToLower() == "l" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var XL = excelData.Count(s => s.JacketSize.ToLower() == "xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var XXL = excelData.Count(s => s.JacketSize.ToLower() == "xxl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var _3XL = excelData.Count(s => s.JacketSize.ToLower() == "3xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                var _4XL = excelData.Count(s => s.JacketSize.ToLower() == "4xl" && s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");

                var roleCount = excelData.Count(s => s.RoleName.ToLower() == role.ToLower() && s.Gender == "Male");
                maleJacketRoleCount += roleCount;

                JacketSizeStatisticDataTable.Rows.Add(role + "(Male)", S, M, L, XL, XXL, _3XL, _4XL, roleCount);

                SCount += S;
                MCount += M;
                LCount += L;
                XLCount += XL;
                XXLCount += XXL;
                _3XLCount += _3XL;
                _4XLCount += _4XL;

                jacketSCount += S;
                jacketMCount += M;
                jacketLCount += L;
                jacketXLCount += XL;
                jacketXXLCount += XXL;
                jacket_3XLCount += _3XL;
                jacket_4XLCount += _4XL;
            }

            JacketSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");
            JacketSizeStatisticDataTable.Rows.Add("Sum(Male)", jacketSCount, jacketMCount, jacketLCount, jacketXLCount, jacketXXLCount, jacket_3XLCount, jacket_4XLCount, maleJacketRoleCount);
            JacketSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");
            JacketSizeStatisticDataTable.Rows.Add("", "", "", "", "", "", "", "", "");
            JacketSizeStatisticDataTable.Rows.Add("Sum(Female/Male)", SCount, MCount, LCount, XLCount, XXLCount, _3XLCount, _4XLCount, maleJacketRoleCount + femaleJacketRoleCount);


            #endregion calculate statistics

            var xlWorkbook = ExcelHandler.ExportDataSetToExcel(ds, serverPath, T_ShirtSizeStatisticDataTable, JacketSizeStatisticDataTable);


            //var ws = xlWorkbook.Worksheet(1);
            //IXLCell cell = ws.Cell(1, 1);
            //IXLRange range = ws.Range("B"+ ds.Tables[0].Rows.Count + T_ShirtSizeStatisticDataTable.Rows.Count + 
            //                          ":J"+ ds.Tables[0].Rows.Count + T_ShirtSizeStatisticDataTable.Rows.Count + JacketSizeStatisticDataTable.Rows.Count );

            //var column = range.FirstColumnUsed(c => c.FirstCell().GetString() == "Sum");
            //column.Style.Font.Bold = true;

            xlWorkbook.Worksheet(1).Columns("e").Width = 30;
            xlWorkbook.Worksheet(1).RowHeight = 27;

            foreach (IXLRow row in xlWorkbook.Worksheet(1).Rows())
            {

                foreach (IXLCell cell in row.Cells())
                {
                    row.Style.Alignment.SetShrinkToFit(true);
                    row.Style.Alignment.ShrinkToFit = true;

                    if (cell.Value.ToString() == "Sum" || cell.Value.ToString() == "Sum(Female)" ||
                        cell.Value.ToString() == "Sum(Male)" || cell.Value.ToString() == "Sum(Female/Male)")
                    {
                        row.Style.Font.Bold = true;
                        row.Style.Font.FontSize = 13;

                    }
                }

            }

            xlWorkbook.SaveAs(serverPath);

            foreach (var item in userFullInfoList)
            {
                item.ExcelFileUrl = fileUrl;
            }


            #endregion Generate Excel

            return Json(userFullInfoList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("gjal")]
        public JsonResult GetJudgeAddressList()
        {
            var blPerson = new BLPerson();

            var teamFullInfoList = blPerson.GetUsersFulInfoByFilter(new VmPerson { RoleName = "Judge" }).ToList();
            foreach (var item in teamFullInfoList)
            {
                item.StreetLine1 =
                            "Street Line 1 :" + item.StreetLine1 + "<br/>" +
                            "Street Line 2 :" + item.StreetLine2 + "<br/>" +
                            "City :" + item.City + "<br/>" +
                            "State :" + item.State + "<br/>" +
                            "Zip Code :" + item.ZipCode;

            }
            #region Generate Excel

            var ds = new DataSet();

            var excelData = from e in teamFullInfoList
                            select new
                            {
                                e.FirstName,
                                e.LastName,
                                e.Email,
                                Gender = (e.Sex == null || e.Sex == true) ? "Female" : "Male",
                                e.RoleName,
                                e.WorkPhoneNumber,

                                Address = e.StreetLine1.Replace("<br/>", Environment.NewLine),

                                TeamName = (string.IsNullOrWhiteSpace(e.TeamName)) ? "" : e.TeamName.Replace("<br/>", Environment.NewLine),
                                Tasks = (string.IsNullOrWhiteSpace(e.Tasks)) ? "" : e.Tasks.Replace("<br/>", Environment.NewLine),
                                e.ExcelFileUrl,
                            };

            var dt = ExcelHandler.ToDataTable(excelData.ToList());

            var newTable = ExcelHandler.CreateExcelBaseDataTable(dt,

                "FirstName",
                "LastName",
                "Email",
                "Gender",
                "RoleName",
                "WorkPhoneNumber",
                "Address",
                "TeamName",
                "Tasks");

            ds.Tables.Add(newTable.Copy());

            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;
            var path = "/Resources/Uploaded/Excel/";

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + "ExcelReport.xlsx";

            serverPath = Path.Combine(HttpContext.Server.MapPath(path), Path.GetFileName(fileName));

            fileUrl = Path.Combine(path, fileName.Replace("\\", "/"));

            var xlWorkbook = ExcelHandler.ExportDataSetToExcel(ds, serverPath);

            xlWorkbook.Worksheet(1).Columns("e").Width = 30;
            xlWorkbook.Worksheet(1).RowHeight = 27;

            foreach (IXLRow row in xlWorkbook.Worksheet(1).Rows())
            {

                foreach (IXLCell cell in row.Cells())
                {
                    row.Style.Alignment.SetShrinkToFit(true);
                    row.Style.Alignment.ShrinkToFit = true;

                    if (cell.Value.ToString() == "Sum" || cell.Value.ToString() == "Sum(Female)" ||
                        cell.Value.ToString() == "Sum(Male)" || cell.Value.ToString() == "Sum(Female/Male)")
                    {
                        row.Style.Font.Bold = true;
                        row.Style.Font.FontSize = 13;
                    }
                }
            }

            xlWorkbook.SaveAs(serverPath);

            foreach (var item in teamFullInfoList)
            {
                item.ExcelFileUrl = fileUrl;
            }


            #endregion Generate Excel

            return Json(teamFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("up")]
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(VmPerson model)
        {
            var result = true;
            var user = UserManager.Users.FirstOrDefault(u => u.Id == model.UserId);

            if (model.SaveAndFinishLater == true)
            {
                model.Agreement = false;
            }
            if (model.SaveAndFinishLater == false && (
                string.IsNullOrWhiteSpace(model.FirstName) ||
                string.IsNullOrWhiteSpace(model.LastName) ||
                model.Agreement == null ||
                model.Agreement == false)
                )
            {
                result = false;
            }
            else
            {
                model.CurrentUserId = CurrentUserId;

                var blPerson = new BLPerson();

                if (CurrentUserRoles.Contains("Judge") && !string.IsNullOrWhiteSpace(model.ClientTaskIds))
                {
                    var taskIdList = model.ClientTaskIds.Split(new char[] { ',' }, StringSplitOptions.None);
                    model.TaskIds = new int[taskIdList.Length];

                    for (var i = 0; i < taskIdList.Length; i++)
                    {
                        model.TaskIds[i] = int.Parse(taskIdList[i]);
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.ClientGoalsAfterGraduationIds))
                {
                    var goalsAfterGraduationIdList = model.ClientGoalsAfterGraduationIds.Split(new char[] { ',' }, StringSplitOptions.None);
                    model.GoalsAfterGraduationIds = new int[goalsAfterGraduationIdList.Length];

                    for (var i = 0; i < goalsAfterGraduationIdList.Length; i++)
                    {
                        model.GoalsAfterGraduationIds[i] = int.Parse(goalsAfterGraduationIdList[i]);
                    }
                }
                if (!string.IsNullOrWhiteSpace(model.ClientEthnicityIds))
                {
                    var ethnicityIdList = model.ClientEthnicityIds.Split(new char[] { ',' }, StringSplitOptions.None);
                    model.EthnicityIds = new int[ethnicityIdList.Length];

                    for (var i = 0; i < ethnicityIdList.Length; i++)
                    {
                        model.EthnicityIds[i] = int.Parse(ethnicityIdList[i]);
                    }
                }
                if (model.ClientMealTypeIds != null)
                {
                    var mealTypeIdList = model.ClientMealTypeIds.Split(new char[] { ',' }, StringSplitOptions.None);
                    model.MealTypeIds = new int[mealTypeIdList.Length];

                    for (var i = 0; i < mealTypeIdList.Length; i++)
                    {
                        model.MealTypeIds[i] = int.Parse(mealTypeIdList[i]);
                    }
                }

                result = blPerson.UpdatePerson(model);

                if (result != false)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.WorkPhoneNumber = model.WorkPhoneNumber;

                    if (model.Email != null)
                    {
                        user.Email = model.Email;
                    }

                    var blUser = new BLUser();
                    blUser.UpdatePhoneUserNumber(user.Id, model.PhoneNumber, model.WorkPhoneNumber);

                }
            }

            var message = "";

            if (result == false)
            {
                message = model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                message = model.ActionMessageHandler.Message = "Operation has been successful";
            }

            var returnUrl = "";

            if (CurrentUserRoles.Contains(SystemRoles.Admin.ToString()))
            {
                returnUrl = "/admin/index";
            }

            if (CurrentUserRoles.Contains("Advisor"))
            {
                if (user.EmailConfirmed == true)
                {
                    returnUrl = "/advisor/index";
                }
                else
                {
                    returnUrl = "/home/index";

                }
            }

            if (CurrentUserRoles.Contains(SystemRoles.Judge.ToString()))
            {
                if (user.EmailConfirmed == true)
                {
                    returnUrl = "/judge/index";
                }
                else
                {
                    returnUrl = "/home/index";

                }
            }

            if (CurrentUserRoles.Contains(SystemRoles.Student.ToString()))
            {
                returnUrl = "/student/index";
            }

            if (CurrentUserRoles.Contains(SystemRoles.Leader.ToString()))
            {
                returnUrl = "/leader/index";
            }
            if (CurrentUserRoles.Contains(SystemRoles.CoAdvisor.ToString()))
            {
                returnUrl = "/coadvisor/index";
            }

            if (CurrentUserRoles.Contains(SystemRoles.Lab.ToString()))
            {
                if (user.EmailConfirmed == true)
                {
                    returnUrl = "/lab/index";
                }
                else
                {
                    returnUrl = "/home/index";

                }
            }

            var jsonData = new
            {
                personId = model.Id,
                success = result,
                message,
                returnUrl,

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

        [ActionName("ra")]
        [HttpPost]
        public ActionResult ReplaceAdvisor(string userId, string firstName, string lastName, string email)
        {
            var blPerson = new BLPerson();
            var result = true;

            result = blPerson.ReplaceAdvisor(userId, firstName, lastName, email);

            var jsonData = new
            {
                success = result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }
        [ActionName("gui")]
        [HttpPost]
        public ActionResult GetUserInfo(string userId)
        {
            var blPerson = new BLPerson();

            var userInfo = blPerson.GetPersonByUserId(userId);

            var jsonData = new
            {
                userInfo.UserId,
                userInfo.FirstName,
                userInfo.LastName,
                userInfo.Email,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("upp")]
        [HttpPost]
        public ActionResult UploadProfileImage(string oldProfilePictureUrl, HttpPostedFileBase uploadedProfilePicture)
        {
            var result = true;
            var blPerson = new BLPerson();
            string profilePictureUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {

                    Image image = Image.FromStream(uploadedProfilePicture.InputStream);
                    Bitmap bitmap = new Bitmap(image);


                    //ResizePicture(ref bitmap);

                    profilePictureUrl = UIHelper.UploadPictureFile(bitmap, uploadedProfilePicture.FileName,
                        uploadedProfilePicture.ContentLength, uploadedProfilePicture.ContentType,
                        "/Resources/Uploaded/Persons/Profile/" + CurrentUserId.Replace("-", ""));


                    result = blPerson.UploadProfileImage(CurrentUserId, profilePictureUrl);

                }
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
                    success = false,
                    message = jsonEx

                };

                return Json(jsonException, JsonRequestBehavior.AllowGet);
            }

            //if (result != false && !string.IsNullOrEmpty(profilePictureUrl))
            //{
            //    try
            //    {
            //        //UIHelper.DeleteFile(oldProfilePictureUrl);
            //    }
            //    catch (Exception ex)
            //    {
            //        var jsonEx = JsonConvert.SerializeObject(ex, Formatting.Indented,
            //                       new JsonSerializerSettings
            //                       {
            //                           ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //                       });

            //        var jsonException = new
            //        {
            //            success = false,
            //            message = jsonEx

            //        };

            //        return Json(jsonException, JsonRequestBehavior.AllowGet);
            //    }
            //}

            var jsonData = new
            {
                profilePictureUrl,
                success = result,
                message = "Your profile picture updated."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

        private static ImageCodecInfo GetEncoderInfo(string v)
        {
            throw new NotImplementedException();
        }

        [ActionName("uup")]
        [HttpPost]
        public ActionResult UploadUniversityImage(int universityId, string oldUniversityPictureUrl, HttpPostedFileBase uploadedUniversityPicture)
        {
            var result = true;
            var blUniversity = new BLUniversity();
            string universityPictureUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {

                    Image image = Image.FromStream(uploadedUniversityPicture.InputStream);
                    Bitmap bitmap = new Bitmap(image);

                    ResizePicture(ref bitmap);

                    universityPictureUrl = UIHelper.UploadPictureFile(bitmap, uploadedUniversityPicture.FileName,
                        uploadedUniversityPicture.ContentLength, uploadedUniversityPicture.ContentType,
                        "/Resources/Uploaded/Universities/" + CurrentUserId.Replace("-", ""));

                    result = blUniversity.UploadUniversityImage(universityId, universityPictureUrl);

                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(universityPictureUrl))
            //{
            //    UIHelper.DeleteFile(oldUniversityPictureUrl);
            //}

            var jsonData = new
            {
                universityPictureUrl,
                success = result,
                message = "Your University picture updated."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

        [ActionName("luup")]
        [HttpPost]
        public ActionResult LoadUniversityPictureUrl(int universityId)
        {
            var result = true;
            var blUniversity = new BLUniversity();

            var jsonData = new
            {
                universityPictureUrl = blUniversity.GetUniversityPictureUrl(universityId) ?? "",
                success = result,
                message = "Your University picture updated."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

        [ActionName("uf")]
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase attachFile)
        {
            var result = true;
            var blPerson = new BLPerson();
            string attachedFileUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    attachedFileUrl = UIHelper.UploadFile(attachFile, "/Resources/Uploaded/" + CurrentUserId.Replace("-", ""));
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                attachedFileUrl,
                success = result,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("ur")]
        [HttpPost]
        public ActionResult UploadResume(string oldResumeUrl, HttpPostedFileBase UploadedResume)
        {
            var result = true;
            var blPerson = new BLPerson();
            string resumeUrl = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    resumeUrl = UIHelper.UploadFile(UploadedResume, "/Resources/Uploaded/Persons/Resume/" + CurrentUserId.Replace("-", ""));
                    if (string.IsNullOrWhiteSpace(resumeUrl) == false)
                    {
                        result = blPerson.UploadResume(CurrentUserId, resumeUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(resumeUrl))
            //{
            //    UIHelper.DeleteFile(oldResumeUrl);
            //}

            var jsonData = new
            {
                resumeUrl,
                success = result,
                message = "Your resume uploaded."
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/PersonEdit", model);
        }

    }
}