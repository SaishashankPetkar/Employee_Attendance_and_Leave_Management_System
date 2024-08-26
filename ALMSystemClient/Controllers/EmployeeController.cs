using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee/EmployeeLogin
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        // POST: Employee/EmployeeLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeLogin(string username, string password)
        {
            if (IsValidUser(username, password))
            {
                Session["IsLoggedIn"] = true;
                Session["UserRole"] = "Employee"; // Optionally store role
                return RedirectToAction("EmployeeDashboard");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // GET: Employee/EmployeeDashboard
        public ActionResult EmployeeDashboard()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }

        // GET: Employee/ApplyLeave
        public ActionResult ApplyLeave()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ApplyLeave(FormCollection form)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            TempData["Message"] = "Leave application submitted successfully!";
            return RedirectToAction("EmployeeDashboard");
        }

        // GET: Employee/SubmitAttendance
        public ActionResult SubmitAttendance()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }

        [HttpPost]
        public ActionResult SubmitAttendance(FormCollection form)
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            TempData["Message"] = "Attendance submitted successfully!";
            return RedirectToAction("EmployeeDashboard");
        }

        // GET: Employee/ViewProjects
        public ActionResult ViewProjects()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            var projects = new List<string> { "Project A", "Project B", "Project C" };
            ViewBag.Projects = projects;

            return View();
        }

        // Helper method to check if the user is authenticated
        private bool IsUserAuthenticated()
        {
            return Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];
        }

        // GET: Employee/Logout
        public ActionResult Logout()
        {
            Session["IsLoggedIn"] = null;
            Session["UserRole"] = null;
            return RedirectToAction("EmployeeLogin");
        }

        // Simulated method for example purposes
        private bool IsValidUser(string username, string password)
        {
            // Replace with actual credential validation logic
            return username == "employee" && password == "password";
        }
    }
}
