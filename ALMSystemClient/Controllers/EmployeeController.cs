using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployeeLogin(string username, string password)
        {
            if (IsValidUser(username, password))
            {
                return RedirectToAction("EmployeeDashboard");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        public ActionResult EmployeeDashboard()
        {
            return View();
        }

        // Simulated method for example purposes
        private bool IsValidUser(string username, string password)
        {
            // Replace with actual credential validation logic
            return username == "employee" && password == "password";
        }
    }
}