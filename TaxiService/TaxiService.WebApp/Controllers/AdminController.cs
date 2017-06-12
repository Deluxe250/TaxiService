using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaxiService.WebApp.Models;

namespace TaxiService.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public AdminController()
        {

        }

        public AdminController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View(db.Users);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", user.Roles.FirstOrDefault().RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleId")] string id, string roleId)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(roleId))
            {
                var user = db.Users.Find(id);
                var role = db.Roles.Find(roleId);

                user.Roles.Clear();
                user.Roles.Add(new IdentityUserRole { UserId = id, RoleId = roleId });

                db.SaveChanges();
            }
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