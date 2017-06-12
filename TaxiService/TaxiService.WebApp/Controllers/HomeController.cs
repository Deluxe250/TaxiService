using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.WebApp.Models;

namespace TaxiService.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.Admin.ToString()))
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Orders");
        }
    }
}