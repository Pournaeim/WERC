using Model;

using Repository.EF.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewEmailLogRepository : EFBaseRepository<ViewEmailLog>
    {
        public IEnumerable<ViewEmailLog> Select(int index, int count)
        {
            var ViewEmailLogList = from ViewEmailLog in Context.ViewEmailLogs
                                   select ViewEmailLog;

            return ViewEmailLogList.OrderBy(A => A.SendDate).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewEmailLog> Select(ViewEmailLog filterItem, int index, int count)
        {
            var ViewEmailLogList = from ViewEmailLog in Context.ViewEmailLogs
                                   select ViewEmailLog;

            if (filterItem.Subject != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.Subject.ToLower().Contains(filterItem.Subject.ToLower()));
            }

            if (filterItem.Body != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.Body.ToLower().Contains(filterItem.Body.ToLower()));
            }

            if (filterItem.AttachUrl != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.AttachUrl.ToLower().Contains(filterItem.AttachUrl.ToLower()));
            }

            if (filterItem.SenderName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.SenderName.ToLower().Contains(filterItem.SenderName.ToLower()));
            }
            if (filterItem.SenderEmail != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.SenderEmail.ToLower().Contains(filterItem.SenderEmail.ToLower()));
            }

            if (filterItem.SenderUserName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.SenderUserName.ToLower().Contains(filterItem.SenderUserName.ToLower()));
            }
            if (filterItem.SenderName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.SenderName.ToLower().Contains(filterItem.SenderName.ToLower()));
            }
            if (filterItem.SenderRoleName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.SenderRoleName.Contains(filterItem.SenderRoleName));
            }
            if (filterItem.RecepientName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.RecepientName.ToLower().Contains(filterItem.RecepientName.ToLower()));
            }
            if (filterItem.RecepientEmail != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.RecepientEmail.ToLower().Contains(filterItem.RecepientEmail.ToLower()));
            }
            if (filterItem.RecepientUserName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.RecepientUserName.ToLower().Contains(filterItem.RecepientUserName.ToLower()));
            }
            if (filterItem.RecepientRoleName != null)
            {
                ViewEmailLogList = ViewEmailLogList.Where(t => t.RecepientRoleName.ToLower().Contains(filterItem.RecepientRoleName.ToLower()));
            }

            return ViewEmailLogList.OrderByDescending(e => e.SendDate).Skip(index).Take(count).ToArray();

        }

        public int GetEmailLogCount()
        {
            return Context.ViewEmailLogs.Count();
        }

        public IEnumerable<ViewEmailLog> GetViewEmailLogs(string emailSubject = "")
        {

            var emailLogList = from emailLog in Context.ViewEmailLogs
                               select emailLog;

            if (emailSubject != "")
            {
                emailLogList = emailLogList.Where(t => t.Subject.Contains(emailSubject));
            }

            return emailLogList.ToArray();
        }

        public object GetEmailLogsBySenderId(string senderId)
        {
            return Context.ViewEmailLogs.Where(t => t.SenderId == senderId);
        }
        public object GetEmailLogsByRecepientId(string recepientId)
        {
            return Context.ViewEmailLogs.Where(t => t.RecepientId == recepientId);
        }

        public ViewEmailLog GetViewEmailLogById(int id)
        {

            return Context.ViewEmailLogs.First(t => t.Id == id);
        }
        public IEnumerable<ViewEmailLog> GetViewEmailLogByIds(int[] ids)
        {
            return Context.ViewEmailLogs.Where(t => ids.Contains(t.Id));
        }
    }
}
