using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class EmailLogRepository : EFBaseRepository<EmailLog>
    {
        public int GetEmailLogCount()
        {
            return Context.EmailLogs.Count();
        }
        public IEnumerable<EmailLog> Select(int index, int count)
        {
            var EmailLogList = from EmailLog in Context.EmailLogs
                               select EmailLog;

            return EmailLogList.OrderBy(A => A.SendDate).Skip(index).Take(count).ToArray();
        }

        public IEnumerable<EmailLog> GetEmailLogs(string emailLogTile = "")
        {
            var emailLogList = from emailLog in Context.EmailLogs
                               select emailLog;

            if (emailLogTile != "")
            {
                emailLogList = emailLogList.Where(t => t.Subject.Contains(emailLogTile));
            }

            return emailLogList.ToArray();
        }

        public void CreateEmailLog(EmailLog newEmailLog)
        {
            Add(newEmailLog);
        }
        public void UpdateEmailLog(EmailLog updateableEmailLog)
        {
            var oldEmailLog = (from s in Context.EmailLogs where s.Id == updateableEmailLog.Id select s).FirstOrDefault();

            oldEmailLog.RecepientId = updateableEmailLog.RecepientId;

            if (!string.IsNullOrEmpty(updateableEmailLog.AttachUrl))
            {
                oldEmailLog.AttachUrl = updateableEmailLog.AttachUrl;
            }

            oldEmailLog.SenderId = updateableEmailLog.SenderId;
            oldEmailLog.SendDate = updateableEmailLog.SendDate;
            oldEmailLog.Subject = updateableEmailLog.Subject;
            oldEmailLog.Body = updateableEmailLog.Body;

            Update(oldEmailLog);
        }
        public bool DeleteEmailLog(int emailLogId)
        {
            var oldEmailLog = (from s in Context.EmailLogs where s.Id == emailLogId select s).FirstOrDefault();

            Delete(oldEmailLog);

            return true;
        }
        public bool DeleteAllEmailLog()
        {
            var allEmailLog = Context.EmailLogs;

            foreach (var item in allEmailLog)
            {
                Delete(item);
            }

            return true;
        }

        public EmailLog GetEmailLogById(int id)
        {
            return Context.EmailLogs.Find(id);
        }
    }
}
