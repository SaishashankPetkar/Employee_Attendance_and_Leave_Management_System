using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class AdminController : Controller
    {
        // Hardcoded admin credentials
        private string AdminUsername => "admin";
        private string AdminPassword => "securepassword123";

        // Login Action
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                Session["Admin"] = username;
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Message = "Invalid login attempt.";
            return View();
        }

        // Dashboard
        public ActionResult Index()
        {
            //if (Session["Admin"] == null)
            //{
            //    return RedirectToAction("Login");
            //}

            return View();
        }

        // Add/Update Employee
        [HttpGet]
        public ActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(string name, string role)
        {
            // Placeholder for adding/updating employee
            // Use ViewBag or TempData to pass messages if needed
            return RedirectToAction("ListEmployees");
        }

        // Edit Employee
        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            // Placeholder for editing employee
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(int id, string name, string role)
        {
            // Placeholder for updating employee
            return RedirectToAction("ListEmployees");
        }

        // Delete Employee
        public ActionResult DeleteEmployee(int id)
        {
            // Placeholder for deleting employee
            return RedirectToAction("ListEmployees");
        }

        // List Employees
        public ActionResult ListEmployees()
        {
            // Placeholder for listing employees
            return View();
        }

        // Add/Update Project
        [HttpGet]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(string projectName)
        {
            // Placeholder for adding/updating project
            return RedirectToAction("ListProjects");
        }

        // Edit Project
        [HttpGet]
        public ActionResult EditProject(int id)
        {
            // Placeholder for editing project
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(int id, string projectName)
        {
            // Placeholder for updating project
            return RedirectToAction("ListProjects");
        }

        // Delete Project
        public ActionResult DeleteProject(int id)
        {
            // Placeholder for deleting project
            return RedirectToAction("ListProjects");
        }

        // List Projects
        public ActionResult ListProjects()
        {
            // Placeholder for listing projects
            return View();
        }

        // Assign Project
        [HttpGet]
        public ActionResult AssignProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProject(int employeeId, int projectId)
        {
            // Placeholder for assigning project to employee
            return RedirectToAction("Index");
        }

        // View Pending Attendance
        public ActionResult ViewPendingAttendance()
        {
            // Placeholder for viewing pending attendance requests
            return View();
        }

        // View Pending Leave
        public ActionResult ViewPendingLeave()
        {
            // Placeholder for viewing pending leave requests
            return View();
        }

        // View Project Attendance
        public ActionResult ViewProjectAttendance(string period)
        {
            // Placeholder for viewing project attendance for different periods
            return View();
        }
    }
}