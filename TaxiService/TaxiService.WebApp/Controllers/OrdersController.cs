using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaxiService.WebApp.Models;

namespace TaxiService.WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var orders = db.Orders
                .Include(o => o.Status)
                .Include(o => o.Driver)
                .OrderByDescending(o => o.Created)
                .ToList();

            return View(orders);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            ViewBag.DriverId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == db.Roles.FirstOrDefault(ro => ro.Name == Roles.Driver.ToString()).Id)), "Id", "FullName");
            var order = new Order { StartTime = DateTime.Now };
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DriverId,StatusId,StartPoint,EndPoint,StartTime,Created")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Status = db.OrderStatuses.FirstOrDefault(s => s.Mnemonic == OrderStatuses.New.ToString());
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.StatusId);
            return View(order);
        }

        [Authorize(Roles = "Admin, Driver, Dispatcher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var avaliableStatuses = db.OrderStatuses.AsQueryable();
            if (User.IsInRole(Roles.Driver.ToString()))
            {
                avaliableStatuses = avaliableStatuses.Where(s => s.Mnemonic != OrderStatuses.New.ToString());
            }
            ViewBag.StatusId = new SelectList(avaliableStatuses, "Id", "Name", order.StatusId);
            ViewBag.DriverId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == db.Roles.FirstOrDefault(ro => ro.Name == Roles.Driver.ToString()).Id)), "Id", "FullName", order.DriverId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Driver, Dispatcher")]
        public ActionResult Edit([Bind(Include = "Id,DriverId,StatusId,StartPoint,EndPoint,StartTime,Created")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var avaliableStatuses = db.OrderStatuses.AsQueryable();
            if (User.IsInRole(Roles.Driver.ToString()))
            {
                avaliableStatuses = avaliableStatuses.Where(s => s.Mnemonic != OrderStatuses.New.ToString());
            }

            ViewBag.StatusId = new SelectList(avaliableStatuses, "Id", "Name", order.StatusId);
            ViewBag.DriverId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == db.Roles.FirstOrDefault(ro => ro.Name == Roles.Driver.ToString()).Id)), "Id", "FullName", order.DriverId);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
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
