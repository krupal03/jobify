using Jobify.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Jobify.Controllers
{
    public class RecruiterPanelController : Controller
    {
        Entities db = new Entities();
        // GET: RecruiterPanel
        [Authorize(Roles ="Recruiter")]
        public ActionResult Index()
        {
            var recruiter = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name);
            return View(recruiter);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            AspNetUser user = db.AspNetUsers.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Address,CreatedAt,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Role")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }
        [Authorize(Roles = "Recruiter")]
        public ActionResult RecruiterProfile()
        {

            Recruiter recruiter = db.Recruiters.FirstOrDefault(m => m.AspNetUser.UserName == User.Identity.Name);
            if (recruiter == null)
            {
                return HttpNotFound();
            }
            return View(recruiter);
        }

        [Authorize(Roles = "Recruiter")]
        public ActionResult RecruiterProfileEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Recruiter recruiter = db.Recruiters.Find(id);
            if (recruiter == null)
            {
                return HttpNotFound();
            }
            return View(recruiter);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecruiterProfileEdit([Bind(Include = "Id,Name,Link,UserId")] Recruiter recruiter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recruiter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RecruiterProfile");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", recruiter.UserId);
            return View(recruiter);
        }



        //job
        // GET: Jobs
        [Authorize(Roles = "Recruiter")]
        public ActionResult Job()
        {
            var r = db.Recruiters.FirstOrDefault(m => m.AspNetUser.UserName == User.Identity.Name);

            var jobs = db.Jobs.Where(m => m.RecruiterId == r.Id);
            return View(jobs);
        }


        [Authorize(Roles = "Recruiter")]
        [HttpGet]
        public ActionResult JobCreate()
        {
            Recruiter r = db.Recruiters.FirstOrDefault(m => m.AspNetUser.UserName == User.Identity.Name);
            ViewBag.RecruiterId = r.Id;
            Job j = new Models.Job();
            return View(j);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobCreate([Bind(Include = "Id,JobType,JobLocation,JobTitle,Salary,Experience,JobSeeker,RecruiterId,Description")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Job");
            }

            ViewBag.RecruiterId = new SelectList(db.Recruiters, "Id", "Name", job.RecruiterId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Recruiter")]
        public ActionResult JobEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            Recruiter r = db.Recruiters.FirstOrDefault(m => m.AspNetUser.UserName == User.Identity.Name);
            ViewBag.recruiter = r.Id;
            return View(job);
        }
        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobEdit([Bind(Include = "Id,JobType,JobLocation,JobTitle,Salary,Experience,JobSeeker,RecruiterId,Description")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Job");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Recruiter")]
        public ActionResult JobDelete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [Authorize(Roles = "Recruiter")]
        [HttpPost, ActionName("JobDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Job");
        }
        [Authorize(Roles = "Recruiter")]
        public ActionResult ApplyDetails(int id)
        {
            var jobapply = db.JobApplies.Where(m => m.JobId == id).ToList();
            return View(jobapply);
        }

        [Authorize(Roles = "Recruiter")]
        public ActionResult ResumeInfo(int id)
        {
            var resume = db.Resumes.FirstOrDefault(m => m.Id == id);
            var rfile = db.ResumeFiles.FirstOrDefault(m => m.ResumeId == resume.Id);
            ViewBag.path = rfile.Path;
            return View(resume);
        }

        [HttpGet]
        [Authorize(Roles = "Recruiter")]
        public ActionResult ChangeStatus(int id)
        {
            var apply = db.JobApplies.Find(id);
            return View(apply);
        }

        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus([Bind(Include = "Id,ApplyAt,ResumeId,JobId,Why_Should_we_hire_you_,Whats_your_dream_job_,JobStatus")] JobApply jobApply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobApply).State = EntityState.Modified;
                db.SaveChanges();



                WebMail.SmtpServer = "smtp.gmail.com";

                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;

                WebMail.EnableSsl = true;

                WebMail.UserName = "ramsky2021@gmail.com";
                WebMail.Password = "ramsky@12345";


                WebMail.From = "ramsky@gmail.com";


                WebMail.Send(to:jobApply.Resume.AspNetUser.UserName, subject: "status apdated", body: "your job ="+jobApply.Job.JobTitle+"status changed to"+jobApply.JobStatus+"so please chaeck your account", isBodyHtml: true);
                ViewBag.Status = "Email Sent Successfully.";



                return RedirectToAction("Job");
            }
             return View(jobApply);
        }

        [Authorize(Roles = "Recruiter")]
        public ActionResult DeleteApply(int id)
        {
            var a = db.JobApplies.Find(id);
            db.JobApplies.Remove(a);
            db.SaveChanges();
            return RedirectToAction("Job");
        }
    }
}