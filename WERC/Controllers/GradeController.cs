using BLL;

using ClosedXML.Excel;

using Model.Base;
using Model.ViewModels.Grade;
using Model.ViewModels.Grade.Grading;

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

using WERC.Filters.ActionFilterAttributes;
using WERC.Models;

using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC.Controllers
{
    [RoleBaseAuthorize(SystemRoles.Admin, SystemRoles.Judge)]
    public class GradeController : BaseController
    {
        [ActionName("ggddl")]
        public ActionResult GetGradeDropDownList()
        {
            var bsGrade = new BLGrade();

            var GradeList = bsGrade.GetGradeSelectListItem(0, int.MaxValue);

            return Json(GradeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("pal")]
        public ActionResult ProcessABETList(int gradeId)
        {
            var bsGrade = new BLGrade();

            var grade = bsGrade.GetGradeById(gradeId);

            return Json(grade.ABET, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gABETddl")]
        public ActionResult GetABETDropDownList()
        {
            var bsGrade = new BLGrade();

            var GradeList = bsGrade.GetABETSelectListItem(0, int.MaxValue);

            return Json(GradeList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("ggddlbtf")]
        public ActionResult GetGradeDropDownListByTask(int taskId)
        {
            var bsGrade = new BLGrade();
            int teamId = int.Parse(Request.QueryString[1]);
            var GradeList = bsGrade.GetGradeSelectListItemByTask(taskId, 0, int.MaxValue).ToList();
            int gradeId = int.Parse(WebConfigurationManager.AppSettings["GradeId"]);
            //var GradeList = bsGrade.GetGradeSelectListItemByTaskWithoutGradeId(taskId, 0, int.MaxValue, gradeId).ToList();

            int[] teamIds = { 45, 46, 47, 48, 52 };
            if (teamIds.Contains(teamId))
            {
                for (int i = 0; i < GradeList.Count(); i++)
                {
                    if (int.Parse(GradeList[i].Value) == gradeId)
                    {
                        GradeList.RemoveAt(i);
                    }
                }
            }

            return Json(GradeList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("ggbf")]
        [HttpGet]
        public JsonResult GetGradeByFilter(string name = null)
        {
            var blGrade = new BLGrade();

            var teamList = blGrade.GetGradeList(name);
            return Json(teamList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("lgtf")]
        [HttpGet]
        public ActionResult LoadGradingTeamForm()
        {
            var blTeamGradeDetail = new BLTeamGradeDetail();
            var gradingTypeList = blTeamGradeDetail.GetTeamGradeMetaData(CurrentUserId);

            return View("../Judge/OLD_GradingTeamForm", new VmGradingManagement
            {
                CurrentUserId = CurrentUserId,
                GradingTypeList = gradingTypeList
            });
        }

        [ActionName("lmsgtf")]
        [HttpGet]
        public ActionResult LoadManagementSingleTeamGradingForm(int id)
        {
            var bsTeam = new BLTeam();
            var teamInfo = bsTeam.GetTeamById(id);
            var teamName = teamInfo.Name;
            var bsGrade = new BLGrade();

            var GradeList = bsGrade.GetGradeSelectListItem(0, int.MaxValue);
            var blTeamGradeDetail = new BLTeamGradeDetail();
            var teamGradeDetail = blTeamGradeDetail.GetSingleTeamGradeDetailByJudgeAndTeam(CurrentUserId, id).FirstOrDefault();

            return View("../Judge/GradingTeamForm", new VmSingleTeamGradingManagement
            {
                CurrentUserId = CurrentUserId,
                GradeList = GradeList,
                TeamId = id,
                TeamName = teamName,
                Task = teamInfo.Task,
                TaskId = teamInfo.TaskId,
                University = teamInfo.University,
                ABETId = teamGradeDetail?.ABETId,
            });
            ;
        }

        [ActionName("l_s_gtf")]
        [HttpPost]
        public PartialViewResult LoadSingleTeamGradingForm(int teamId, int gradeId)
        {
            var blGrade = new BLGrade();
            var blTeamGradeDetail = new BLTeamGradeDetail();
            var gradingTypeList = blTeamGradeDetail.GetSingleTeamGradeMetaData(CurrentUserId, gradeId, teamId);

            double? gradeTotalScore = null;

            if (gradingTypeList.TeamGrading.GradingDetailList.Count() > 0)
            {
                gradeTotalScore = gradingTypeList.TeamGrading.GradingDetailList.Sum(p => p.Point * p.Coefficient) ?? 0;
            }

            return PartialView("../Judge/_GradingTable", new VmSingleGradingType
            {
                CurrentUserId = CurrentUserId,
                GradeId = gradeId,
                TeamId = teamId,
                TeamName = gradingTypeList.TeamGrading.TeamName,
                TeamGrading = gradingTypeList.TeamGrading,
                TotalScore = blTeamGradeDetail.GetTotalScore(CurrentUserId, teamId),
                GradeTotalScore = gradeTotalScore ?? 0,
                GradeType = blGrade.GetGradeById(gradeId).Name,
            });
        }

        [ActionName("gtswg")]
        [HttpPost]
        public JsonResult GetTotalScoreWithoutGrade(int teamId, int gradeId)
        {
            var blTeamGradeDetail = new BLTeamGradeDetail();

            var totalScoreData = blTeamGradeDetail.GetSingleTeamTotalScoreWithGradeWithoutCurrentJudge(CurrentUserId, gradeId, teamId);
            return Json(totalScoreData, JsonRequestBehavior.AllowGet);

        }
        //[ActionName("lctgf")]
        //[HttpGet]
        //public ActionResult LoadCreateTeamGradeForm(int id)
        //{
        //    var blTeamGradeDetail = new BLTeamGradeDetail();
        //    var teamGradeDetail = blTeamGradeDetail.GetTeamGradeMetaData(id)

        //    return View("../Judge/CreateTeamGrade", teamGradeDetail)
        //}


        [ActionName("old_sg")]
        [HttpPost]
        public ActionResult old_SaveGrading(VmClientGrading[] clientGrading)
        {
            var result = true;
            var blTeamGradeDetail = new BLTeamGradeDetail();

            try
            {
                if (ModelState.IsValid)
                {
                    result = blTeamGradeDetail.UpdateTeamGradeDetail(CurrentUserId, clientGrading);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                success = result,
                message = "",
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("sg")]
        [HttpPost]
        public ActionResult SaveGrading(VmClientGrading[] clientGrading)
        {
            var result = true;
            var blTeamGradeDetail = new BLTeamGradeDetail();

            try
            {
                var comment = (from c in clientGrading where c.Description != "" select c.Description).First();

                foreach (var item in clientGrading)
                {
                    item.Description = comment;
                }

                //var ABETId = (from c in clientGrading where c.ABETId != null select c.ABETId).First();

                //foreach (var item in clientGrading)
                //{
                //    item.ABETId = ABETId;
                //}

                result = blTeamGradeDetail.UpdateSingleTeamGradeDetail(CurrentUserId, clientGrading[0].GradeId, clientGrading[0].TeamId, clientGrading);

            }
            catch (Exception ex)
            {
                result = false;
            }

            var jsonData = new
            {
                success = result,
                message = "",
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("ctg")]
        [HttpPost]
        public ActionResult CreateTeamGrade(VmTeamGradeDetail model)
        {
            var result = true;
            var blTeamGradeDetail = new BLTeamGradeDetail();

            model.CurrentUserId = CurrentUserId;

            try
            {
                if (ModelState.IsValid)
                {
                    result = blTeamGradeDetail.CreateTeamGradeDetail(model);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            if (result == true)
            {
                return RedirectToAction("tl", "judge", new { activeItemId = result });
            }

            model.ActionMessageHandler.Message = "Operation has been failed...\n";

            return View("../Jadge/CreateTeamGrade", model);
        }

        [ActionName("lcgf")]
        [HttpGet]
        public ActionResult LoadCreateGradeForm()
        {
            var blPerson = new BLPerson();
            var person = blPerson.GetPersonByUserId(CurrentUserId);

            return View("../Admin/CreateGrade", new VmGrade());
        }

        [ActionName("cg")]
        [HttpPost]
        public ActionResult Create(VmGrade model)
        {
            var result = -1;
            var blGrade = new BLGrade();

            model.CurrentUserId = CurrentUserId;

            try
            {
                if (ModelState.IsValid)
                {
                    result = blGrade.CreateGrade(model);
                }
            }
            catch (Exception ex)
            {
                result = -1;
            }

            if (result != -1)
            {
                return RedirectToAction("gl", "Admin", new { activeItemId = result });
            }

            model.ActionMessageHandler.Message = "Operation has been failed...\n";

            return View("../Admin/CreateGrade", model);
        }

        [ActionName("eg")]
        [HttpPost]
        public ActionResult EditGrade(VmGrade model)
        {
            model.CurrentUserId = CurrentUserId;

            var result = true;
            var blGrade = new BLGrade();

            try
            {
                if (ModelState.IsValid)
                {
                    result = blGrade.UpdateGrade(model);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }
            else
            {
                model.ActionMessageHandler.Message = "Operation has been successful";
            }

            var jsonData = new
            {
                success = result,
                message = model.ActionMessageHandler.Message
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [ActionName("dg")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = true;

            var blGrade = new BLGrade();

            result = blGrade.DeleteGrade(id);

            string resultMessage = string.Empty;

            if (result == true)
            {
                resultMessage = new BaseViewModel()["Score Sheet Has been deleted successfuly."];
            }
            else
            {
                resultMessage = new BaseViewModel()["This grade has assigned to task. You can't delete it..."];

            }

            var jsonResult = new
            {
                success = result,
                message = resultMessage,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }



        [ActionName("egtex")]
        public ActionResult ExportGradeToExcel()
        {
            var blTeamGradeDetail = new BLTeamGradeDetail();

            var teamGradeDetailList = blTeamGradeDetail.GetAllTeamGradeDetail().ToList();

            #region Generate Excel

            var ds = new DataSet();

            var excelData = from e in teamGradeDetailList
                            select new
                            {
                                e.Id,
                                e.TeamId,
                                e.GradeDetailId,
                                e.TaskId,
                                e.TeamName,
                                e.Date,
                                e.State,
                                e.ImageUrl,
                                e.LabResultUrl,
                                e.WrittenReportUrl,
                                e.GradeId,
                                e.EvaluationItem,
                                e.MaxPoint,
                                e.Grade,
                                e.Point,
                                e.TeamNumber,
                                e.Description,
                                e.JudgeUserId,
                                e.Coefficient,
                                e.Signature,
                                e.FirstName,
                                e.LastName,
                            };

            var dt = ExcelHandler.ToDataTable(excelData.ToList());

            var newTable = ExcelHandler.CreateExcelBaseDataTable(dt,

                "Id",
                "TeamId",
                "GradeDetailId",
                "TaskId",
                "TeamName",
                "Date",
                "State",
                "ImageUrl",
                "LabResultUrl",
                "WrittenReportUrl",
                "GradeId",
                "EvaluationItem",
                "MaxPoint",
                "Grade",
                "Point",
                "TeamNumber",
                "Description",
                "JudgeUserId",
                "Coefficient",
                "Signature",
                "FirstName",
                "LastName"
                );

            ds.Tables.Add(newTable.Copy());

            var newFilePrefix = "";
            var fileUrl = string.Empty;
            var serverPath = string.Empty;
            var path = "/Resources/Uploaded/Excel/";

            var date = DateTime.Now;
            newFilePrefix = date.Year.ToString("D4") + date.Month.ToString("D2") + date.Day.ToString("D2") + date.Hour.ToString("D2") + date.Minute.ToString("D2") + date.Second.ToString("D2");
            var fileName = newFilePrefix + "TeamsScroreDetail.xlsx";

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
                }
            }

            xlWorkbook.SaveAs(serverPath);

            #endregion Generate Excel

            var jsonResult = new
            {
                excelFileUrl = fileUrl,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

    }
}