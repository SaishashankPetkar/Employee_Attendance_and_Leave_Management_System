using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager/ManagerLogin
        public ActionResult ManagerLogin()
        {
            return View();
        }

        // POST: Manager/ManagerLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagerLogin(string username, string password)
        {
            if (IsValidUser(username, password))
            {
                Session["IsLoggedIn"] = true;
                Session["UserRole"] = "Manager"; // Optionally store role
                return RedirectToAction("ManagerDashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // GET: Manager/ManagerDashboard
        public ActionResult ManagerDashboard()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("ManagerLogin");
            }

            return View();
        }

        // GET: Manager/ViewEmployees
        public ActionResult ViewEmployees()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("ManagerLogin");
            }

            var employees = new List<object>
            {
                new { EmployeeID = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890", Role = "Developer" },
                new { EmployeeID = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "234-567-8901", Role = "Designer" },
                new { EmployeeID = 3, Name = "Bob Johnson", Email = "bob.johnson@example.com", Phone = "345-678-9012", Role = "Manager" }
            };

            return View(employees);
        }

        // GET: Manager/ViewPendingAttendance
        public ActionResult ViewPendingAttendance()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("ManagerLogin");
            }

            var attendanceRequests = new List<object>
            {
                new { RequestID = 1, EmployeeName = "John Doe", RequestDate = "2024-08-01", Status = "Pending" },
                new { RequestID = 2, EmployeeName = "Jane Smith", RequestDate = "2024-08-02", Status = "Pending" },
                new { RequestID = 3, EmployeeName = "Bob Johnson", RequestDate = "2024-08-03", Status = "Pending" }
            };

            return View(attendanceRequests);
        }

        // GET: Manager/ViewPendingLeave
        public ActionResult ViewPendingLeave()
        {
            if (!IsUserAuthenticated())
            {
                return RedirectToAction("ManagerLogin");
            }

            var leaveRequests = new List<object>
            {
                new { RequestID = 1, EmployeeName = "Alice Williams", LeaveType = "Annual Leave", StartDate = "2024-08-10", EndDate = "2024-08-15", Status = "Pending" },
                new { RequestID = 2, EmployeeName = "David Brown", LeaveType = "Sick Leave", StartDate = "2024-08-12", EndDate = "2024-08-13", Status = "Pending" },
                new { RequestID = 3, EmployeeName = "Emma Davis", LeaveType = "Personal Leave", StartDate = "2024-08-20", EndDate = "2024-08-22", Status = "Pending" }
            };

            return View(leaveRequests);
        }

        // GET: Manager/Logout
        public ActionResult Logout()
        {
            Session["IsLoggedIn"] = null;
            Session["UserRole"] = null;
            return RedirectToAction("ManagerLogin");
        }

        // Helper method to check if the user is authenticated
        private bool IsUserAuthenticated()
        {
            return Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];
        }

        // Simulated method for example purposes
        private bool IsValidUser(string username, string password)
        {
            // Replace with actual credential validation logic
            return username == "manager" && password == "password";
        }
    }
}

