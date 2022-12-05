using BLL.Base;

using Model;
using Model.ViewModels.EmailLog;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLEmailLog : BLBase
    {
        public int GetEmailLogCount()
        {
            var emailLogRepository = UnitOfWork.GetRepository<EmailLogRepository>();
            return emailLogRepository.GetEmailLogCount();
        }
         public int GetViewEmailLogCount()
        {
            var emailLogRepository = UnitOfWork.GetRepository<ViewEmailLogRepository>();
            return emailLogRepository.GetEmailLogCount();
        }

        public IEnumerable<VmEmailLog> GetAllEmailLogs(int index, int count)
        {
            var emailLogRepository = UnitOfWork.GetRepository<ViewEmailLogRepository>();

            var emailLogList = emailLogRepository.Select(index, count);
            var vmSelectListItem = (from emailLog in emailLogList
                                    select new VmEmailLog
                                    {
                                        Id = emailLog.Id,
                                        RecepientId = emailLog.RecepientId,
                                        SenderId = emailLog.SenderId,
                                        SendDate = emailLog.SendDate,
                                        Date = emailLog.SendDate.ToString(),
                                        Subject = emailLog.Subject,
                                        Body = emailLog.Body,
                                        AttachUrl = emailLog.AttachUrl,
                                        SenderName = emailLog.SenderName,
                                        SenderEmail = emailLog.SenderName,
                                        SenderUserName = emailLog.SenderUserName,
                                        SenderRoleName = emailLog.SenderUserName,
                                        RecepientName = emailLog.RecepientName,
                                        RecepientEmail = emailLog.RecepientEmail,
                                        RecepientUserName = emailLog.RecepientUserName,
                                        RecepientRoleName = emailLog.RecepientRoleName,

                                    });

            return vmSelectListItem;
        }
        public VmEmailLog GetEmailLogById(int id)
        {
            var viewEmailLogRepository = UnitOfWork.GetRepository<ViewEmailLogRepository>();

            var emailLog = viewEmailLogRepository.GetViewEmailLogById(id);

            var vmEmailLog =
                              new VmEmailLog
                              {
                                  Id = emailLog.Id,
                                  RecepientId = emailLog.RecepientId,
                                  SenderId = emailLog.SenderId,
                                  SendDate = emailLog.SendDate,
                                  Subject = emailLog.Subject,
                                  Body = emailLog.Body,
                                  AttachUrl = emailLog.AttachUrl,
                                  SenderName = emailLog.SenderName,
                                  SenderEmail = emailLog.SenderName,
                                  SenderUserName = emailLog.SenderUserName,
                                  SenderRoleName = emailLog.SenderUserName,
                                  RecepientName = emailLog.RecepientName,
                                  RecepientEmail = emailLog.RecepientEmail,
                                  RecepientUserName = emailLog.RecepientUserName,
                                  RecepientRoleName = emailLog.RecepientRoleName,

                              };

            return vmEmailLog;
        }
        public int CreateEmailLog(VmEmailLog vmEmailLog)
        {
            var result = -1;
            try
            {
                var emailLogRepository = UnitOfWork.GetRepository<EmailLogRepository>();

                var newEmailLog = new EmailLog
                {
                    RecepientId = vmEmailLog.RecepientId,
                    SenderId = vmEmailLog.SenderId,
                    SendDate = vmEmailLog.SendDate,
                    Subject = vmEmailLog.Subject,
                    Body = vmEmailLog.Body,
                    AttachUrl = vmEmailLog.AttachUrl,
                };

                emailLogRepository.CreateEmailLog(newEmailLog);

                UnitOfWork.Commit();

                result = newEmailLog.Id;
            }
            catch (Exception ex)
            {
                result = -1;
            }

            return result;
        }
        public bool UpdateEmailLog(VmEmailLog vmEmailLog)
        {
            try
            {
                var emailLogRepository = UnitOfWork.GetRepository<EmailLogRepository>();

                var updateableEmailLog = new EmailLog
                {
                    Id = vmEmailLog.Id,
                    RecepientId = vmEmailLog.RecepientId,
                    SenderId = vmEmailLog.SenderId,
                    SendDate = vmEmailLog.SendDate,
                    Subject = vmEmailLog.Subject,
                    Body = vmEmailLog.Body,
                    AttachUrl = vmEmailLog.AttachUrl,
                };

                emailLogRepository.UpdateEmailLog(updateableEmailLog);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteEmailLog(int emailLogId)
        {
            try
            {
                var EmailLogRepository = UnitOfWork.GetRepository<EmailLogRepository>();


                if (EmailLogRepository.DeleteEmailLog(emailLogId) == true)
                {
                    return UnitOfWork.Commit();
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<VmEmailLog> GetEmailLogByFilter(VmEmailLog filterItem, int index, int count)
        {
            var viewEmailLogRepository = UnitOfWork.GetRepository<ViewEmailLogRepository>();

            var viewFilterItem = new ViewEmailLog
            {
                Id = filterItem.Id,
                Subject = filterItem.Subject,
                Body = filterItem.Body,
                AttachUrl = filterItem.AttachUrl,
                SenderName = filterItem.SenderName,
                SenderEmail = filterItem.SenderEmail,
                SenderUserName = filterItem.SenderUserName,
                SenderRoleName = filterItem.SenderRoleName,
                RecepientName = filterItem.RecepientName,
                RecepientEmail = filterItem.RecepientEmail,
                RecepientUserName = filterItem.RecepientUserName,
                RecepientRoleName = filterItem.RecepientRoleName,
            };

            if (filterItem.Date != null)
            {
                filterItem.SendDate = DateTime.Parse(filterItem.Date);

            }
            var viewemailLogList = viewEmailLogRepository.Select(viewFilterItem, index, count);

            var vmEmailLogList = from emailLog in viewemailLogList
                                 select new VmEmailLog
                                 {
                                     Id = emailLog.Id,
                                     RecepientId = emailLog.RecepientId,
                                     SenderId = emailLog.SenderId,
                                     SendDate = emailLog.SendDate,
                                     Date = emailLog.SendDate.ToString(),
                                     Subject = emailLog.Subject,
                                     Body = emailLog.Body,
                                     AttachUrl = emailLog.AttachUrl,
                                     SenderName = emailLog.SenderName,
                                     SenderEmail = emailLog.SenderEmail,
                                     SenderUserName = emailLog.SenderUserName,
                                     SenderRoleName = emailLog.SenderRoleName,
                                     RecepientName = emailLog.RecepientName,
                                     RecepientEmail = emailLog.RecepientEmail,
                                     RecepientUserName = emailLog.RecepientUserName,
                                     RecepientRoleName = emailLog.RecepientRoleName,
                                 };
            return vmEmailLogList.ToList();
        }

    }
}
