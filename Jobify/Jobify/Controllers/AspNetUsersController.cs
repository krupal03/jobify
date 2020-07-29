using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobify.Models;

namespace Jobify.Controllers
{
    public class AspNetUsersController : Controller
    {
        private Entities db = new Entities();

        // GET: AspNetUsers
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.AspNetUsers.Where(m=>m.Role=="Jobseeker").ToListAsync());
        }

        // GET: AspNetUsers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FullName,Address,CreatedAt,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Role")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,Address,CreatedAt,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Role")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            Resume resume = db.Resumes.FirstOrDefault(m => m.UserId == id);
            ResumeFile resumefile = db.ResumeFiles.FirstOrDefault(m => m.Resume.UserId == id);
            if(System.IO.File.Exists(resumefile.Path))
            {
                System.IO.File.Delete(resumefile.Path);
            }
            var job = db.JobApplies.Where(m => m.Resume.UserId == id).ToList();

            foreach(JobApply i in job)
            {   var j= db.Jobs.FirstOrDefault(m => m.Id == i.JobId);
                j.JobSeeker = j.JobSeeker + 1;
                db.SaveChanges();
                db.JobApplies.Remove(i);
            }
            db.ResumeFiles.Remove(resumefile);
            db.Resumes.Remove(resume);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        //custom
        [Authorize(Roles = "Admin")]
        public ActionResult Resume(string id)
        {
            var user = db.Resumes.FirstOrDefault(m => m.UserId == id);

            var data = db.ResumeFiles.FirstOrDefault(m => m.Resume.UserId == id);
            ViewBag.Path = data.Path;
            return View(user);
        }




        [Authorize(Roles = "Admin")]
        public ActionResult AppliedJob(string id)
        {
            var jobs = db.JobApplies.Where(m => m.Resume.UserId==id).ToList();
            return View(jobs);
        }



        [Authorize(Roles = "Admin")]
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
