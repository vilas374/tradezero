using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace TurnKeyBrokerSignUp.WebUI.Services
{
    public static class EmailHelper
    {
        public static int AddToEmailQueue(string storeURI, string fromName, string fromEmail, string toName, string toEmail, string cc, string bcc, string subject, string body)
        {
            try
            {
                var dataContext = new TurnKeyBrokerSignUp.WebUI.Models.TurnKeyBrokerSignUpDataContext();

                var _emailLog = new TurnKeyBrokerSignUp.WebUI.Models.EmailLog();

                _emailLog.StoreURI = storeURI;
                _emailLog.FromName = fromName;
                _emailLog.FromEmail = fromEmail;
                _emailLog.ToName = toName;
                _emailLog.ToEmail = toEmail;
                _emailLog.CCEmail = cc;
                _emailLog.BCCEmail = bcc;
                _emailLog.Subject = subject;
                _emailLog.Body = body;
                _emailLog.isSent = false;
                _emailLog.date_created = DateTime.Now;
                dataContext.EmailLogs.InsertOnSubmit(_emailLog);
                dataContext.SubmitChanges();

                dataContext.Dispose();

                return _emailLog.EmailLogID;
            }
            catch { }
            return 0;
        }

        public static void UpdateEmailQueueStatus(int EmailLogID, bool isSent, string message)
        {
            try
            {
                var dataContext = new TurnKeyBrokerSignUp.WebUI.Models.TurnKeyBrokerSignUpDataContext();

                var _emailLog = dataContext.EmailLogs.Where(em => em.EmailLogID == EmailLogID).FirstOrDefault();

                if (_emailLog != null)
                {
                    _emailLog.isSent = isSent;
                    _emailLog.message = message;
                    _emailLog.date_sent = DateTime.Now;
                    dataContext.SubmitChanges();
                }
                dataContext.Dispose();
            }
            catch { }
        }

        public static string SendEmail(string fromName, string fromEmail, string toName, string toEmail, string cc, string bcc, string subject, string body, bool isHtml,int ? tempID=0)
        {
            string networkUserName = string.Empty;
            
            MailMessage msg = new MailMessage();
             msg.From = new MailAddress(fromEmail, fromName);
            //msg.From = new MailAddress("bmtasset.sipl@gmail.com", "TradeZero");
            //for multiple recipients 
            foreach (var address in toEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                msg.To.Add(address);
            }
            MailAddress to = new MailAddress(toEmail, toName);
            var test = to.DisplayName;
            //msg.Bcc.Add(bcc);


            if (!string.IsNullOrEmpty(cc))
            {
                foreach (string s in cc.Split(','))
                    msg.CC.Add(new MailAddress(s));
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                foreach (string s in bcc.Split(','))
                    msg.Bcc.Add(new MailAddress(s));
            }

            if (isHtml)
                msg.IsBodyHtml = true;
            else
                msg.IsBodyHtml = false;


            msg.Subject = subject;
            msg.Body = body;

            string mailServer = "localhost";

            if (ConfigurationManager.AppSettings["MailServer"] != null)
                 mailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
              //  mailServer = "smtp.gmail.com"; 
            bool isGmail = false;

            if (ConfigurationManager.AppSettings["IsGmail"] != null)
                isGmail = bool.Parse(ConfigurationManager.AppSettings["IsGmail"].ToString());

            SmtpClient mail;

            if (isGmail)
            {
                mail = new SmtpClient("smtp.gmail.com", 587);
                mail.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["GmailUsername"].ToString(), ConfigurationManager.AppSettings["GmailPassword"].ToString());
                mail.EnableSsl = true;
            }
            else
            {
                
                mail = new SmtpClient(mailServer);
              // mail.Port = 587;
              //  mail.Port = 465;
                mail.UseDefaultCredentials = false;
                mail.EnableSsl = true;
                mail.Credentials = CredentialCache.DefaultNetworkCredentials;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                if (fromName=="TradeZero")
                {
                    
                    if (fromEmail.Trim() == "info@tradezero.co")
                    {
                        networkUserName = ConfigurationManager.AppSettings["InfoUserName"].ToString();
                    }
                    else
                    {
                        networkUserName = ConfigurationManager.AppSettings["NoreplyUserName"].ToString();

                    }
                    NetworkCred.UserName = networkUserName;
                     NetworkCred.Password = ConfigurationManager.AppSettings["NetworkPassword"].ToString();

                   // NetworkCred.UserName = "bmtasset.sipl@gmail.com";
                   // NetworkCred.Password = "sipl@1234";
                    mail.Credentials = NetworkCred;
                    mail.Timeout = 1000000;
                    
                }
                else
                {
                    if(fromName.Trim() == "GeneralTrade" || fromName.Trim() == "DRS Markets")
                    {
                        //For general trade
                        mailServer = ConfigurationManager.AppSettings["GeneralTradeMailServer"];
                        networkUserName = ConfigurationManager.AppSettings["GeneralTradeUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["GeneralTradeNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }
                    else if(fromName=="AscendTrading")
                    {
                        //For general trade
                        networkUserName = ConfigurationManager.AppSettings["AscendTradingUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["AscendTradingNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }
                    else if (fromName == "TKLTrading")
                    {
                        //For general trade
                        networkUserName = ConfigurationManager.AppSettings["TKLTradingUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["TKLTradingNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }
                    else if (fromName == "LiveStream")
                    {
                        //For general trade
                        networkUserName = ConfigurationManager.AppSettings["LiveStreamTradingUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["LiveStreamTradingNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }
                    else if (fromName == "StockSpy")
                    {
                        //For general trade
                        networkUserName = ConfigurationManager.AppSettings["StockSpyTradingUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["StockSpyTradingNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }
                    else if (fromName == "BlueInvesting")
                    {
                        //For general trade
                        networkUserName = ConfigurationManager.AppSettings["StockSpyTradingUserName"].ToString();
                        NetworkCred.UserName = networkUserName;
                        NetworkCred.Password = ConfigurationManager.AppSettings["StockSpyTradingNetworkPassword"].ToString();
                        mail.Credentials = NetworkCred;
                        mail.Timeout = 1000000;
                    }

                }
                
            }
            try
            {
                /*if (tempID == 2)
                {
                    System.Net.Mail.Attachment attachment;
                    String quickGuide = "C:/Users/User8/Desktop/vinay/Template_newaccountfunding/Quick_Guide_TradeZero(1).pdf";

                    string[] path = quickGuide.Split('/');
                    attachment = new System.Net.Mail.Attachment(quickGuide);
                    msg.Attachments.Add(attachment);
                    attachment.Name = path[6];
                }*/

                mail.Send(msg);
                
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string SendEmailTest(string fromName, string fromEmail, string toName, string toEmail, string cc, string bcc, string subject, string body, bool isHtml,string networkUserName,string networkPassword,string testMailServer)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(fromEmail, fromName);

            //for multiple recipients 
            foreach (var address in toEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                msg.To.Add(address);
            }
            //msg.Bcc.Add(bcc);


            if (!string.IsNullOrEmpty(cc))
            {
                foreach (string s in cc.Split(','))
                    msg.CC.Add(new MailAddress(s));
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                foreach (string s in bcc.Split(','))
                    msg.Bcc.Add(new MailAddress(s));
            }

            if (isHtml)
                msg.IsBodyHtml = true;
            else
                msg.IsBodyHtml = false;


            msg.Subject = subject;
            msg.Body = body;

            string mailServer = "localhost";

            if (ConfigurationManager.AppSettings["MailServer"] != null)
                mailServer = ConfigurationManager.AppSettings["MailServer"].ToString();
            bool isGmail = false;

            if (ConfigurationManager.AppSettings["IsGmail"] != null)
                isGmail = bool.Parse(ConfigurationManager.AppSettings["IsGmail"].ToString());

            SmtpClient mail;

            if (isGmail)
            {
                mail = new SmtpClient("smtp.gmail.com", 587);
                mail.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["GmailUsername"].ToString(), ConfigurationManager.AppSettings["GmailPassword"].ToString());
                mail.EnableSsl = true;
            }
            else
            {
                mail = new SmtpClient(testMailServer);
                mail.Credentials = CredentialCache.DefaultNetworkCredentials;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = networkUserName;
                NetworkCred.Password = networkPassword;
                mail.Credentials = NetworkCred;
                mail.Timeout = 1000000;
            }
            try
            {
                mail.Send(msg);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}