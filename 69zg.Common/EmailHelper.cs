using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace _69zg.Common
{
   public class EmailHelper
    {
        /// <summary>
        /// 海外反馈发送邮件，发单封
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">正文</param>
        /// <param name="files">文件s[]</param>
        /// <returns></returns>
        public static bool SendEmailFeed(string subject, string body, HttpFileCollection files)
        {
            string mailTo = ConfigParam.GetAppSetting("eMailTo", "");
            string mailFrom = ConfigParam.GetAppSetting("eMailFrom", "");
            string mailFromPassWord = ConfigParam.GetAppSetting("eMailFromPSW", "");
            string mailHost = ConfigParam.GetAppSetting("eMailHost", "");
            return SendEmail(mailTo, mailFrom, mailFromPassWord, mailHost, subject, body, files);
        }

        /// <summary>
        /// 发送多封邮件
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static void SendEmails(string subject, string body, HttpFileCollection files)
        {
            string mailTo = ConfigParam.GetAppSetting("eMailTo", "");
            string mailFrom = ConfigParam.GetAppSetting("eMailFrom", "");
            string mailFromPassWord = ConfigParam.GetAppSetting("eMailFromPSW", "");
            string mailHost = ConfigParam.GetAppSetting("eMailHost", "");
            string[] mailTos = null;
            if (mailTo.Contains("|"))
            {
                mailTos = mailTo.Trim('|').Split('|');
                for (int i = 0; i < mailTos.Length; i++)
                {
                    SendEmail(mailTo, mailFrom, mailFromPassWord, mailHost, subject, body, files);
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="emailTo">发送至</param>
        /// <param name="emailFrom">发送者</param>
        /// <param name="password"></param>
        /// <param name="emailHost"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool SendEmail(string emailTo, string emailFrom, string password, string emailHost, string subject, string body, HttpFileCollection files)
        {
            try
            {
                string displayName = ConfigParam.GetAppSetting("eMailName", "产品中心");
                MailAddress from = new MailAddress(emailFrom, displayName);
                MailAddress to = new MailAddress(emailTo);
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;//设置邮件主题 
                message.IsBodyHtml = true;//设置邮件正文为html格式
                message.Body = body;//设置邮件内容;     
                //string attfile = "D:\\摄影包.txt";
                //string attfile = Page.Request.Form["filepath"].ToString().Trim();
                //Attachment attafile = new Attachment(attfile);
                //message.Attachments.Add(attafile);

                if (files != null)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        if (file.ContentLength > 0)
                        {
                            System.IO.Stream filestream = file.InputStream;
                            Attachment attachfile = new Attachment(filestream, file.ContentType);
                            message.Attachments.Add(attachfile);
                        }
                    }
                }

                //SmtpClient client = new SmtpClient("127.0.0.1");
                SmtpClient client = new SmtpClient(emailHost);
                //设置发送邮件身份验证方式 
                //注意如果发件人地址是abc@def.com，则用户名是abc而不是abc@def.com 
                client.Credentials = new NetworkCredential(emailFrom, password);

                client.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailACollection">收件人</param>
        /// <param name="htmlstr">发送内容</param>
        /// <returns>Tuple<成功邮箱，失败邮箱></returns>
        public static Tuple<string, string> SendMail(MailAddressCollection mailACollection, string subject, string htmlstr)
        {
            MailMessage mailMessage = new MailMessage();
            StringBuilder successmails = new StringBuilder();
            StringBuilder faildmails = new StringBuilder();
            string accountmail = ConfigParam.GetAppSetting("MailAccount");
            string accountpwd = ConfigParam.GetAppSetting("EmailPassword"); ;
            List<string> sendemailusers = new List<string>();
            mailMessage.IsBodyHtml = true;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mailMessage.From = new MailAddress(accountmail, ConfigParam.GetAppSetting("MailDisplayName"), Encoding.UTF8);
            mailMessage.Priority = MailPriority.High;
            mailMessage.Subject = subject;
            SmtpClient smtp = new SmtpClient();
            Array myArr = Array.CreateInstance(typeof(string), mailACollection.Count);
            smtp.Host = ConfigParam.GetAppSetting("MailHost");
            smtp.Port = int.Parse(ConfigParam.GetAppSetting("MailPort", "25"));
            NetworkCredential network = new NetworkCredential();
            network.UserName = accountmail;
            network.Password = accountpwd;
            smtp.Credentials = network;
            smtp.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                    delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
            foreach (MailAddress mailaddr in mailACollection)
            {
                if (sendemailusers.Contains(mailaddr.Address))
                    continue;
                sendemailusers.Add(mailaddr.Address);
                mailMessage.Body = htmlstr.Replace("{mail}", EncryptUtil.EnDeString(mailaddr.Address, true));
                mailMessage.To.Add(mailaddr);
                try
                {
                    smtp.Send(mailMessage);
                    successmails.Append(mailaddr.Address + ";");
                }
                catch
                {
                    faildmails.Append(mailaddr.Address + ";");
                }
                finally
                {
                    mailMessage.To.Remove(mailaddr);
                }
            }
            mailMessage.Dispose();
            return Tuple.Create(successmails.ToString(), faildmails.ToString());
        }
    }
}
