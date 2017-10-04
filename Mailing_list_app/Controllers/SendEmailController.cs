using Mailing_list_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Mailing_list_app.Controllers
{
    public class SendEmailController : Controller
    {
        dbContext dbcontext = new dbContext();

        //get all emails that user send
        public ActionResult ViewSendEmail()
        {
            List<SendDBEmail> sendDBEmail = dbcontext.SendDBEmails.Where(mail => mail.UserProfile.UserName == User.Identity.Name).Select(mail => mail).ToList();
            return View(sendDBEmail);
        }

        // create new email
        public ActionResult SendEmail()
        {
            SendEmail sendEmail;
            sendEmail = new SendEmail();
            return View(sendEmail);
        }

        //send email from user email to all recipients and save mail history in database
        [HttpPost]
        public ActionResult SendEmail(SendEmail sendEmail)
        {
            SendDBEmail sendDBEmail = new SendDBEmail();
            sendDBEmail.UserProfile = dbcontext.UserProfiles.Where(u=>u.UserName == User.Identity.Name).Select(_smtp => _smtp).FirstOrDefault();
            sendDBEmail.Recipients = (dbcontext.UserProfiles.Where(userProfiles => userProfiles.UserName==User.Identity.Name).Select(u=>u).FirstOrDefault()).Recipients.ToList();
            sendDBEmail.dateTime = DateTime.Now;
            foreach(var recipient in sendDBEmail.Recipients)
            {
                using (MailMessage mm = new MailMessage(sendDBEmail.UserProfile.SMTPs.FirstOrDefault().Email, recipient.Email))
                {
                    mm.Subject = sendEmail.Subject;
                    mm.Body = sendEmail.Body;
                    mm.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(sendDBEmail.UserProfile.SMTPs.FirstOrDefault().Email, Encryption.Decrypt(sendDBEmail.UserProfile.SMTPs.FirstOrDefault().Password, sendDBEmail.UserProfile.SMTPs.FirstOrDefault().Key));
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        ViewBag.Message = "Email sent.";
                    }
                }
            }
            sendDBEmail.Subject = sendEmail.Subject;
            dbcontext.SendDBEmails.Add(sendDBEmail);
            dbcontext.SaveChanges();
            return RedirectToAction("ViewSendEmail");
        }
    }
}