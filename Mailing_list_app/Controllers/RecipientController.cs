using Mailing_list_app.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mailing_list_app.Controllers
{
    public class RecipientController : Controller
    {
        dbContext dbcontext = new dbContext();
        Recipient recipient = new Recipient();

        //Create recipient for current user
        [HttpGet]
        public ActionResult Create()
        {
            Recipient r = new Recipient();
            return View(r);
        }

        //write recipient for current user in database
        [HttpPost, ActionName("Create")]
        public ActionResult Create(Recipient r)
        {
            if (ModelState.IsValid)
            {
                //check exist recipient in table of recipients
                recipient = dbcontext.Recipients.Where(rec => rec.Email == r.Email).FirstOrDefault();

                //get user profile of current user
                UserProfile up = dbcontext.UserProfiles.Where(re => re.UserName == User.Identity.Name).Select(re => re).FirstOrDefault();
                r.UserProfiles = dbcontext.UserProfiles.Where(re => re.UserName == User.Identity.Name).Select(re => re).ToList();

                if (recipient == null)
                {
                    //create new row in table recipients
                    dbcontext.Recipients.Add(r);
                }
                else
                {
                    //add only new relation between exist recipient and current user
                    up.Recipients.Add(recipient);
                    dbcontext.Entry(up).State = EntityState.Modified;
                }
                dbcontext.SaveChanges();
                return RedirectToAction("ViewRecipients");
            }
            return View(r);
        }

        //get all recipients
        public ActionResult ViewRecipients()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                List<Recipient> rec = (dbcontext.UserProfiles.Where(u => u.UserName == User.Identity.Name).Select(u => u).FirstOrDefault()).Recipients.ToList();
                return View(rec);
            }
            return View();
        }

        //find recipient by id  
        [HttpGet]
        public ActionResult Edit(int id)
        {
            recipient = dbcontext.Recipients.Where(re=>re.Id==id).Select(re=>re).FirstOrDefault();
            if (recipient == null)
                return HttpNotFound();

            return View(recipient);
        }

        //edit recipients and save in database
        [HttpPost, ActionName("Edit")]
        public ActionResult EditConfirmed(Recipient r)
        {
            if(ModelState.IsValid)
            {
                dbcontext.Entry(r).State = EntityState.Modified;
                dbcontext.SaveChanges();
                return RedirectToAction("ViewRecipients");
            }
            return View(r);
        }

        //get recipient by id
        [HttpGet]
        public ActionResult Delete(int id)
        {
            recipient = dbcontext.Recipients.Where(re => re.Id == id).Select(re => re).FirstOrDefault();
            return View(recipient);
        }

        //delete recipient by id from database
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //get recipients by id
            recipient = dbcontext.Recipients.Where(re => re.Id == id).Select(re => re).FirstOrDefault();
            //get user profile current user
            UserProfile userProfile = dbcontext.UserProfiles.Where(re => re.UserName == User.Identity.Name).Select(re => re).FirstOrDefault();

            //delete only current relation
            userProfile.Recipients.Remove(recipient);
            dbcontext.Entry(userProfile).State = EntityState.Modified;

            dbcontext.SaveChanges();
            return RedirectToAction("ViewRecipients");
        }
    }
}