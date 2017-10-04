using Mailing_list_app.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mailing_list_app.Controllers
{
    public class SMTPController : Controller
    {
        dbContext dbcontext = new dbContext();
        SMTP smtp = new SMTP();

        // get user email and password
        public ActionResult SMTPview()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                smtp = dbcontext.SMTPs.Where(x => x.UserProfiles.UserName == User.Identity.Name).Select(s => s).FirstOrDefault();
                if (smtp == null)
                {
                    ViewBag.Email = null;
                    return View();
                }
                else
                {
                    ViewBag.Email = smtp.Email;
                    return View(smtp);
                }

            }
            else
            {
                ViewBag.Email = null;
                return View();
            }
        }

        //Create email and password for current user
        [HttpGet]
        public ActionResult Create()
        {
            return View(smtp);
        }

        //write email for current user in database
        [HttpPost, ActionName("Create")]
        public ActionResult Create(SMTP s)
        {
            if (ModelState.IsValid)
            {
                //secure password storage
                s.Key = Encryption.GenerateEncryptionKey(8);
                s.Password = Encryption.Encrypt(s.Password, s.Key);
                s.UserProfiles = dbcontext.UserProfiles.Where(n => n.UserName == User.Identity.Name).Select(n => n).FirstOrDefault();
                dbcontext.SMTPs.Add(s);
                dbcontext.SaveChanges();
                return RedirectToAction("SMTPview");
            }
            return View("Create");

        }

        //find email by id  
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SMTP s = dbcontext.SMTPs.Where(el => el.Id == id).FirstOrDefault();
            s.Password = Encryption.Decrypt(s.Password, s.Key);
            if (s == null)
                return HttpNotFound();

            return View(s);
        }

        //edit email or password and save in database
        [HttpPost, ActionName("Edit")]
        public ActionResult EditConfirmed(SMTP s)
        {
            if (ModelState.IsValid)
            {
                s.Key = Encryption.GenerateEncryptionKey(8);
                s.Password = Encryption.Encrypt(s.Password, s.Key);
                dbcontext.Entry(s).State = EntityState.Modified;
                dbcontext.SaveChanges();
                return RedirectToAction("SMTPview");
            }
            return View(s);
        }

        //get email by id
        [HttpGet]
        public ActionResult Delete(int id)
        {
            SMTP s = dbcontext.SMTPs.Where(el => el.Id == id).FirstOrDefault();
            return View(s);
        }

        //delete email by id from database
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SMTP s = dbcontext.SMTPs.Where(el => el.Id == id).FirstOrDefault();
            dbcontext.SMTPs.Remove(s);
            dbcontext.SaveChanges();
            return RedirectToAction("SMTPview");
        }

    }
}