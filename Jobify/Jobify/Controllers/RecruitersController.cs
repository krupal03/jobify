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
    public class RecruitersController : Controller
    {
        private Entities db = new Entities();

        // GET: Recruiters
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> Index()
        {
            var recruiters = db.Recruiters.Include(r => r.AspNetUser);
            return View(await recruiters.ToListAsync());
        }

        // GET: Recruiters/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruiter recruiter = await db.Recruiters.FindAsync(id);
            if (recruiter == null)
            {
                return HttpNotFound();
            }
            return View(recruiter);
        }


        // GET: Recruiters/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recruiter recruiter = await db.Recruiters.FindAsync(id);
            if (recruiter == null)
            {
                return HttpNotFound();
            }
            return View(recruiter);
        }

        // POST: Recruiters/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recruiter recruiter = await db.Recruiters.FindAsync(id);
            var job = db.Jobs.Where(m => m.RecruiterId == id).ToList();
           foreach(Job i in job)
            {
                var ja = db.JobApplies.Where(m=>m.JobId==i.Id).ToList();
                foreach(JobApply j in ja)
                {
                    db.JobApplies.Remove(j);
                }
                db.Jobs.Remove(i);
            }
            var user = db.AspNetUsers.Find(recruiter.UserId);
            db.AspNetUsers.Remove(user);
            db.SaveChanges();
            db.Recruiters.Remove(recruiter);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
