using BLL.Base;

using Model.ViewModels.EmailLog;

using System;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace BLL
{
    public class EmailHelper : BLBase
    {
        MailMessage MailToUserMessage = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        BLEmailLog bLEmailLog = new BLEmailLog();
        BLPerson bLPerson = new BLPerson();

        public bool EmailLog = true;
        public string[] EmailList { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public bool SendInBcc { get; set; }
        public string SpecialEmail { get; set; }
        public string AdminEmail { get; set; }
        public string ErrorMessage { get; set; }
        public string CurrentUserId { get; set; }

        public bool Send()
        {
            SpecialEmail = "bictchairman@gmail.com";
            bool result = true;
            try
            {
                var persons = bLPerson.GetUsersByEmails(EmailList);

                MailToUserMessage.BodyEncoding = Encoding.UTF8;
                MailToUserMessage.SubjectEncoding = Encoding.UTF8;
                MailToUserMessage.Subject = Subject;
                MailToUserMessage.IsBodyHtml = IsBodyHtml;
                MailToUserMessage.Body = Body;

                var i = 0;

                foreach (string email in EmailList)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {

                        if (SendInBcc)
                        {
                            MailToUserMessage.Bcc.Add(email);
                        }
                        else
                        {
                            MailToUserMessage.To.Add(email);
                        }
                    }
                    var receipient = persons.Where(p => p.Email == email).FirstOrDefault();
                    //var sender = persons.Where(p => p.Email == MailToUserMessage.From.Address).FirstOrDefault();

                    if (EmailLog == true)
                    {
                        bLEmailLog.CreateEmailLog(new VmEmailLog
                        {
                            RecepientId = receipient?.UserId,
                            SenderId = CurrentUserId,
                            SendDate = DateTime.Now,
                            Subject = Subject,
                            Body = Body,
                            AttachUrl = "",
                        });
                    }
                    i++;

                    if (i % 10 == 0)
                    {

                        result = SendFinalResult(result);

                        MailToUserMessage.Bcc.Clear();
                        MailToUserMessage.To.Clear();
                    }
                }
                if (MailToUserMessage.Bcc.Count > 0 || MailToUserMessage.To.Count > 0)
                {
                    result = SendFinalResult(result);
                }
            }
            catch (Exception ex1)
            {
                result = false;
                ErrorMessage += ex1.Message + ((ex1.InnerException != null) ? ex1.InnerException.Message : "") + "\n";
                var blSiteInfo = new BLPerson();
                blSiteInfo.CreateSiteInfo("Send Email Error ex1:" + ErrorMessage);
            }

            return result;
        }

        private bool SendFinalResult(bool result)
        {
            var blSiteInfo = new BLPerson();
            try
            {
                if (SendInBcc)
                {
                    MailToUserMessage.Bcc.Add(SpecialEmail);
                    // MailToUserMessage.Bcc.Add(AdminEmail);
                }
                else
                {
                    MailToUserMessage.To.Add(SpecialEmail);
                    //MailToUserMessage.To.Add(AdminEmail);
                }

                smtp.EnableSsl = false;
                smtp.Send(MailToUserMessage);
            }
            catch (Exception ex2)
            {
                ErrorMessage = ex2.Message + ((ex2.InnerException != null) ? ex2.InnerException.Message : "") + "\n";

                blSiteInfo.CreateSiteInfo("Send Email Error ex2:" + ErrorMessage);
                try
                {
                    smtp.EnableSsl = true;
                    smtp.Send(MailToUserMessage);
                }
                catch (Exception ex3)
                {
                    result = false;
                    ErrorMessage += ex3.Message + ((ex3.InnerException != null) ? ex3.InnerException.Message : "") + "\n";

                    blSiteInfo.CreateSiteInfo("Send Email Error ex3:" + ErrorMessage);
                }
            }

            return result;
        }


    }
}