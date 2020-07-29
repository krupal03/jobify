using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jobify.Models;
using System.Data.Entity;

namespace Jobify.Controllers
{
    public class JobseekerPanelController : Controller
    {
        Entities db = new Entities();
        // GET: JobseekerPanel
        [Authorize(Roles = "Jobseeker")]
        public ActionResult Index()
        {
            var user = db.AspNetUsers.FirstOrDefault(m => m.UserName == User.Identity.Name);

            return View(user);
        }
        [Authorize(Roles = "Jobseeker")]
        [HttpGet]
        public ActionResult Resume()
        {
           
            var r = db.Resumes.FirstOrDefault(m => m.AspNetUser.UserName ==User.Identity.Name);
            return View(r);
        }


        [Authorize(Roles = "Jobseeker")]
        public ActionResult ResumeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Resume resume = db.Resumes.Find(id);
            if (resume == null)
            {
                return HttpNotFound();
            }
           
            return View(resume);
        }
        [Authorize(Roles = "Jobseeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResumeEdit([Bind(Include = "Id,Name,Dob,Gender,Experience,Address,MobileNo,Qualification,City,UserId")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resume).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Resume");
            }
           
            return View(resume);
        }
        [Authorize(Roles = "Jobseeker")]
        [HttpGet]
          public ActionResult ResumeFile()
        {
           
            var resume = db.Resumes.FirstOrDefault(m => m.AspNetUser.UserName==User.Identity.Name);
            var rfile = db.ResumeFiles.FirstOrDefault(m=>m.ResumeId==resume.Id);
            return View(rfile);
        }


        [Authorize(Roles = "Jobseeker")]
        public ActionResult ResumeFileEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ResumeFile resumeFile = db.ResumeFiles.Find(id);
            if (resumeFile == null)
            {
                return HttpNotFound();
            }

            return View(resumeFile);
        }

        [Authorize(Roles = "Jobseeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResumeFileEdit([Bind(Include = "Id,ResumeId,PdfFile")] ResumeFile resumeFile)
        {
            string ext = System.IO.Path.GetExtension(resumeFile.PdfFile.FileName);
            ViewBag.ext = ext;
            if (resumeFile.PdfFile.ContentLength >0 && ext==".pdf")
            { 
            string filename="";
           

          
                filename = resumeFile.ResumeId + ext;
                resumeFile.Path = "../../UploadedResume/" + filename;
                filename = System.IO.Path.Combine(Server.MapPath("~/UploadedResume/"), filename);
                resumeFile.PdfFile.SaveAs(filename);
                resumeFile.UploadedAt = DateTime.Now;
                if (ModelState.IsValid)
                {
                    db.Entry(resumeFile).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ResumeFile");
                }
                ModelState.Clear();
            }
            else
            {
                ViewBag.ext = ViewBag.ext + " Not Valid Formate of File";
            }
            return View(resumeFile);
        }




        [Authorize(Roles = "Jobseeker")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }
            AspNetUser user =db.AspNetUsers.FirstOrDefault(m=>m.Id==id);
            if (user == null)
            {
                return HttpNotFound();
            }
           
            return View(user);
        }

        [Authorize(Roles = "Jobseeker")]
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

        [Authorize(Roles = "Jobseeker")]
        [HttpGet]
        public ActionResult AllJob()
        {
            var job = db.Jobs.Where(m=>m.JobSeeker>0).ToList();
            return View(job);
        }
        [Authorize(Roles = "Jobseeker")]
        public ActionResult AppyJob(int id)
        {
            var resume = db.Resumes.FirstOrDefault(m => m.AspNetUser.UserName == User.Identity.Name);
            var job = db.Jobs.Find(id);
            ViewBag.jobid = job.Id;
            ViewBag.resumeid = resume.Id;
            JobApply js = new JobApply();
            return View(js);
        }
        [Authorize(Roles = "Jobseeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AppyJob([Bind(Include = "ResumeId,JobId,ApplyAt,Why_Should_we_hire_you_,Whats_your_dream_job_,JobStatus")] JobApply jobApply)
        {
            if (ModelState.IsValid)
            { var exists = db.JobApplies.Any(m => m.ResumeId == jobApply.ResumeId && m.JobId == jobApply.JobId);
                if (!exists) { 
                db.JobApplies.Add(jobApply);
                db.SaveChanges();
                    var job = db.Jobs.Find(jobApply.JobId);
                
                    job.JobSeeker = job.JobSeeker - 1;
                    db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("AppliedJob");
                }
                else { ViewBag.msg = "Already applied"; }
            }

           
            return View(jobApply);
        }
        [Authorize(Roles = "Jobseeker")]
        public ActionResult AppliedJob()
        {
            var applyed = db.JobApplies.Where(m=>m.Resume.AspNetUser.UserName==User.Identity.Name).ToList();
            return View(applyed);
        }



        [Authorize(Roles = "Jobseeker")]
        public ActionResult ApplyJobDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            JobApply jobApply = db.JobApplies.Find(id);
            if (jobApply == null)
            {
                return HttpNotFound();
            }
            return View(jobApply);
        }

        [Authorize(Roles = "Jobseeker")]
        public ActionResult ApplyJobEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            JobApply jobApply =  db.JobApplies.Find(id);
            if (jobApply == null)
            {
                return HttpNotFound();
            }
            return View(jobApply);
        }
        [Authorize(Roles = "Jobseeker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyJobEdit([Bind(Include = "Id,ApplyAt,ResumeId,JobId,Why_Should_we_hire_you_,Whats_your_dream_job_,JobStatus")] JobApply jobApply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobApply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AppliedJob");
            }
            ViewBag.JobId = new SelectList(db.Jobs, "Id", "JobType", jobApply.JobId);
            ViewBag.ResumeId = new SelectList(db.Resumes, "Id", "Name", jobApply.ResumeId);
            return View(jobApply);
        }

        // GET: JobApplies/Delete/5
        [Authorize(Roles = "Jobseeker")]
        public ActionResult ApplyJobDelete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            JobApply jobApply = db.JobApplies.Find(id);
            if (jobApply == null)
            {
                return HttpNotFound();
            }
            return View(jobApply);
        }

        [Authorize(Roles = "Jobseeker")]
        [HttpPost, ActionName("ApplyJobDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyJobDeleteConfirmed(int id)
        {
            JobApply jobApply =db.JobApplies.Find(id);
            var job = db.Jobs.Find(jobApply.Job.Id);
            job.JobSeeker = job.JobSeeker + 1;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            db.JobApplies.Remove(jobApply);
            db.SaveChanges();
           
            return RedirectToAction("AppliedJob");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}