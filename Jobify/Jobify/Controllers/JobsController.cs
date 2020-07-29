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
    public class JobsController : Controller
    {
        private Entities db = new Entities();

        // GET: Jobs
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var jobs = db.Jobs.Include(j => j.Recruiter);
            return View(await jobs.ToListAsync());
        }

        // GET: Jobs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }





        // GET: Jobs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            var ja = db.JobApplies.Where(m => m.JobId == id).ToList(); 
            foreach(JobApply i in ja)
            {
                db.JobApplies.Remove(i);
            }
            db.SaveChanges();
            db.Jobs.Remove(job); 
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
