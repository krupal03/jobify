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
    public class AspNetUserRolesController : Controller
    {
        private Entities db = new Entities();

        // GET: AspNetUserRoles
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var aspNetUserRoles = db.AspNetUserRoles.Include(a => a.AspNetRole).Include(a => a.AspNetUser);
            return View(await aspNetUserRoles.ToListAsync());
        }

        // GET: AspNetUserRoles/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = await db.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName");
            return View();
        }

        // POST: AspNetUserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,RoleId,AssignAt")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUserRoles.Add(aspNetUserRole);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }


        // GET: AspNetUserRoles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = await db.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,RoleId,AssignAt")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUserRole).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.AspNetRoles, "Id", "Name", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        // GET: AspNetUserRoles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = await db.AspNetUserRoles.FindAsync(id);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserRole);
        }

        // POST: AspNetUserRoles/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUserRole aspNetUserRole = await db.AspNetUserRoles.FindAsync(id);
            db.AspNetUserRoles.Remove(aspNetUserRole);
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
