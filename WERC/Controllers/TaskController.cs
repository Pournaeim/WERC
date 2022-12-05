using BLL;

using CyberneticCode.Web.Mvc.Helpers;

using Model.Base;
using Model.ViewModels.Task;

using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace WERC.Controllers
{
    public class TaskController : BaseController
    {
        [ActionName("gtddl")]
        public ActionResult GetTaskDropDownList()
        {
            var bsTask = new BLTask();

            var taskList = bsTask.GetTaskSelectListItem(0, int.MaxValue);

            return Json(taskList, JsonRequestBehavior.AllowGet);
        }
        [ActionName("g_lu_tddl")]
        public ActionResult GetLabUserTaskDropDownList()
        {
            var bsTask = new BLTask();

            var taskList = bsTask.GetLabUserTaskSelectListItem(CurrentUserId);

            return Json(taskList, JsonRequestBehavior.AllowGet);
        }
        [ActionName("gtddlwd")]
        public ActionResult GetTaskDropDownListWithDescription()
        {
            var bsTask = new BLTask();

            var taskList = bsTask.GetTaskSelectListItemWithDescription(0, int.MaxValue);

            return Json(taskList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("gtfibf")]
        [HttpGet]
        public JsonResult GetTaskFullInfoByFilter(VmTaskFullInfo filterItem = null)
        {
            var blTask = new BLTask();

            var judgeFullInfoList = blTask.GetTaskFullInfoByFilter(filterItem).ToList();

            return Json(judgeFullInfoList, JsonRequestBehavior.AllowGet);
        }

        [ActionName("cpn")]
        [HttpGet]
        public JsonResult checkProjectName(int id)
        {
            var blTask = new BLTask();

            return Json(blTask.GetTaskById(id).Preliminary, JsonRequestBehavior.AllowGet);
        }

        [ActionName("lctf")]
        [HttpGet]
        public ActionResult LoadCreateTaskForm()
        {
            return View("../Admin/CreateTask", new VmTask());
        }

        [ActionName("ct")]
        [HttpPost]
        public ActionResult Create(VmTask model)
        {
            var result = -1;
            var blTask = new BLTask();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedDocument != null)
                    {

                        Image image = Image.FromStream(model.UploadedDocument.InputStream);
                        Bitmap bitmap = new Bitmap(image);

                        ResizePicture(ref bitmap);

                        model.ImageUrl = UIHelper.UploadPictureFile(bitmap, model.UploadedDocument.FileName,
                            model.UploadedDocument.ContentLength, model.UploadedDocument.ContentType,
                            "/Resources/Uploaded/Tasks/" + CurrentUserId.Replace("-", ""));

                    }
                }

                result = blTask.CreateTask(model);
            }
            catch (Exception ex)
            {
                result = -1;
            }


            if (result != -1)
            {
                return RedirectToAction("tl", "Admin", new { activeItemId = result });
            }

            model.ActionMessageHandler.Message = "Operation has been failed...\n";

            return View("../Admin/CreateTask", model);
        }

        [ActionName("et")]
        [HttpPost]
        public ActionResult EditTask(VmTask model)
        {
            model.CurrentUserId = CurrentUserId;

            var oldUrl = model.ImageUrl;
            var result = true;
            var blTask = new BLTask();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedDocument != null)
                    {

                        Image image = Image.FromStream(model.UploadedDocument.InputStream);
                        Bitmap bitmap = new Bitmap(image);

                        ResizePicture(ref bitmap);

                        model.ImageUrl = UIHelper.UploadPictureFile(bitmap, model.UploadedDocument.FileName,
                            model.UploadedDocument.ContentLength, model.UploadedDocument.ContentType,
                            "/Resources/Uploaded/Tasks/" + CurrentUserId.Replace("-", ""));

                    }
                }

                result = blTask.UpdateTask(model);
            }
            catch (Exception ex)
            {
                result = false;
            }

            //if (result != false && !string.IsNullOrEmpty(model.ImageUrl))
            //{
            //    UIHelper.DeleteFile(oldUrl);
            //}

            if (result == false)
            {
                model.ActionMessageHandler.Message = "Operation has been failed...\n call system Admin";
            }

            var jsonData = new
            {
                TaskTitle = model.Name,
                TaskIconUrl = model.ImageUrl,
                TaskId = model.Id,
                success = result,
                message = model.ActionMessageHandler.Message = "Operation has been successful"

            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

            //return View("../Author/TaskEdit", model);
        }

        // GET: Task/Delete/5
        [ActionName("dt")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = true;

            var blTask = new BLTask();

            var resultMessage = blTask.CheckDeleteTask(id);

            if (resultMessage == "")
            {
                result = blTask.DeleteTask(id);
            }
            else
            {
                result = false;
            }

            if (result == true)
            {
                resultMessage = new BaseViewModel()["Task has been deleted successfuly."];
            }
            else
            {
                resultMessage = new BaseViewModel()[resultMessage];
            }

            var jsonResult = new
            {
                success = result,
                message = resultMessage,
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        // GET: Task/Delete/5
        [ActionName("chut")]
        [HttpPost]
        public ActionResult CheckUserTask(string id, int[] taskIds)
        {
            var result = true;
            var blUserTask = new BLUserTask();

            var count = blUserTask.CheckUsertask(id, taskIds);

            var message = "";
            if (count > 0)
            {
                result = false;
                message = new BaseViewModel()["This lab has input a result/s for a test in this task. You can't unassigned it..."];
            }

            var jsonResult = new
            {
                success = result,
                message
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
        // GET: Task/Delete/5
        [ActionName("chjut")]
        [HttpPost]
        public ActionResult CheckJudgeUserTask(string id, int[] taskIds)
        {
            var result = true;
            var blUserTask = new BLUserTask();

            var count = blUserTask.CheckJudgeUsertask(id, taskIds);

            var message = "";
            if (count > 0)
            {
                result = false;
                message = new BaseViewModel()["This judge has input a result/s for this task. You can't unassigned it..."];
            }

            var jsonResult = new
            {
                success = result,
                message
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}