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
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.User.IsInRole(Roles.User.ToString()))
                {
                    RedirectToAction("");
                }
                else if (HttpContext.User.IsInRole(Roles.Admin.ToString()))
                {
                    RedirectToAction("");
                }
                else if (HttpContext.User.IsInRole(Roles.Driver.ToString()))
                {
                    RedirectToAction("");
                }
                else if (HttpContext.User.IsInRole(Roles.Dispatcher.ToString()))
                {
                    RedirectToAction("");
                }

            }
            return RedirectToAction("Account", "Login");
        }
    }
}