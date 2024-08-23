
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class ManagerController : Controller
    {
        private const string ManagerUsername = "manager";
        private const string ManagerPassword = "password";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (username == ManagerUsername && password == ManagerPassword)
            {
                Session["Manager"] = username;
                return RedirectToAction("ManagerDashboard"); // Redirect to ManagerDashboard
            }

            ViewBag.Message = "Invalid login attempt.";
            return View();
        }

        [HttpGet]
        public ActionResult ManagerDashboard()
        {
            if (Session["Manager"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ViewEmployees()
        {
            if (Session["Manager"] != null)
            {
                var employees = new List<string> { "Employee1", "Employee2", "Employee3" };
                return View(employees);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ViewPendingAttendance()
        {
            if (Session["Manager"] != null)
            {
                var attendanceRequests = new List<string> { "Request1", "Request2" };
                return View(attendanceRequests);
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ViewPendingLeave()
        {
            if (Session["Manager"] != null)
            {
                var leaveRequests = new List<string> { "Request1", "Request2" };
                return View(leaveRequests);
            }
            return RedirectToAction("Login");
        }
    }
}
