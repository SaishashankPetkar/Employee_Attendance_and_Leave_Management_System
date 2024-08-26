using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Login
        public ActionResult AdminLogin()
        {
            if (Session["User"] != null) // Check if the user is already authenticated
            {
                return RedirectToAction("AdminDashboard");
            }
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string username, string password)
        {
            if (IsValidAdmin(username, password))
            {
                Session["User"] = username; // Store user information in session
                return RedirectToAction("AdminDashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // GET: Admin/Dashboard
        public ActionResult AdminDashboard()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        // GET: Admin/Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session data
            Session.Abandon(); // End the session
            return RedirectToAction("AdminLogin");
        }

        // GET: Admin/EmployeeDetails
        public ActionResult EmployeeDetails()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            var employees = new List<object>
            {
                new { EmployeeID = 1, EmployeeName = "John Doe", Email = "johndoe@example.com", Phone = "123-456-7890", HireDate = DateTime.Now.AddYears(-5), RoleID = 1, ManagerID = 2, ProjectID = 1, LeaveBalance = 10, No_of_leave = 5, Emp_status = "Active" }
                // Add more employees as necessary
            };

            ViewBag.Employees = employees; // Pass data to view
            return View();
        }

        // GET: Admin/CreateNewEmployee
        public ActionResult CreateNewEmployee()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        // POST: Admin/CreateNewEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewEmployee(FormCollection form)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Extract form values
            string employeeName = form["EmployeeName"];
            string email = form["Email"];
            string phone = form["Phone"];
            DateTime hireDate = Convert.ToDateTime(form["HireDate"]);
            int roleID = Convert.ToInt32(form["RoleID"]);
            int managerID = Convert.ToInt32(form["ManagerID"]);
            int projectID = Convert.ToInt32(form["ProjectID"]);
            int leaveBalance = Convert.ToInt32(form["LeaveBalance"]);
            int no_of_leave = Convert.ToInt32(form["No_of_leave"]);
            string emp_status = form["Emp_status"];

            // Add the new employee to the database (replace with actual data insertion logic)
            // Example: db.AddEmployee(employeeName, email, phone, hireDate, roleID, managerID, projectID, leaveBalance, no_of_leave, emp_status);

            return RedirectToAction("EmployeeDetails");
        }

        // GET: Admin/EditEmployee/1
        public ActionResult EditEmployee(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Fetch the employee from the database using the id (replace with actual data retrieval logic)
            var employee = new
            {
                EmployeeID = id,
                EmployeeName = "John Doe",
                Email = "johndoe@example.com",
                Phone = "123-456-7890",
                HireDate = DateTime.Now.AddYears(-5),
                RoleID = 1,
                ManagerID = 2,
                ProjectID = 1,
                LeaveBalance = 10,
                No_of_leave = 5,
                Emp_status = "Active"
            };

            // Pass the data to the view
            ViewBag.Employee = employee;
            return View();
        }

        // POST: Admin/EditEmployee/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(int id, FormCollection form)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Extract form values (replace with actual logic)
            string employeeName = form["EmployeeName"];
            string email = form["Email"];
            string phone = form["Phone"];
            DateTime hireDate = Convert.ToDateTime(form["HireDate"]);
            int roleID = Convert.ToInt32(form["RoleID"]);
            int managerID = Convert.ToInt32(form["ManagerID"]);
            int projectID = Convert.ToInt32(form["ProjectID"]);
            int leaveBalance = Convert.ToInt32(form["LeaveBalance"]);
            int no_of_leave = Convert.ToInt32(form["No_of_leave"]);
            string emp_status = form["Emp_status"];

            // Update employee in the database (replace with actual data update logic)

            return RedirectToAction("EmployeeDetails");
        }

        // POST: Admin/DeleteEmployee/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate delete logic here (replace with actual delete logic)

            return RedirectToAction("EmployeeDetails");
        }

        // GET: Admin/ProjectDetails
        public ActionResult ProjectDetails()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            var projects = new List<object>
            {
                new { ProjectID = 1, ProjectName = "Project Alpha", StartDate = DateTime.Now.AddMonths(-6), EndDate = DateTime.Now.AddMonths(6), Status = "Active" }
                // Add more projects as necessary
            };

            ViewBag.Projects = projects; // Pass data to view
            return View();
        }

        // GET: Admin/CreateNewProject
        public ActionResult CreateNewProject()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        // POST: Admin/CreateNewProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProject(FormCollection form)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Extract form values
            string projectName = form["ProjectName"];
            DateTime startDate = Convert.ToDateTime(form["StartDate"]);
            DateTime endDate = Convert.ToDateTime(form["EndDate"]);
            string status = form["Status"];

            // Add the new project to the database (replace with actual data insertion logic)
            // Example: db.AddProject(projectName, startDate, endDate, status);

            return RedirectToAction("ProjectDetails");
        }

        // GET: Admin/EditProject/1
        public ActionResult EditProject(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Fetch the project from the database using the id (replace with actual data retrieval logic)
            var project = new
            {
                ProjectID = id,
                ProjectName = "Project Alpha",
                StartDate = DateTime.Now.AddMonths(-6),
                EndDate = DateTime.Now.AddMonths(6),
                Status = "Active"
            };

            // Pass the data to the view
            ViewBag.Project = project;
            return View();
        }

        // POST: Admin/EditProject/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProject(int id, FormCollection form)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Extract form values (replace with actual logic)
            string projectName = form["ProjectName"];
            DateTime startDate = Convert.ToDateTime(form["StartDate"]);
            DateTime endDate = Convert.ToDateTime(form["EndDate"]);
            string status = form["Status"];

            // Update project in the database (replace with actual data update logic)

            return RedirectToAction("ProjectDetails");
        }

        // POST: Admin/DeleteProject/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProject(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate delete logic here (replace with actual delete logic)

            return RedirectToAction("ProjectDetails");
        }

        // GET: Admin/AttendanceRequests
        public ActionResult AttendanceRequests()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            var requests = new List<object>
            {
                new { RequestID = 1, EmployeeName = "John Doe", RequestDate = DateTime.Now.AddDays(-1), Status = "Pending" }
                // Add more requests as necessary
            };

            ViewBag.Requests = requests; // Pass data to view
            return View();
        }

        // POST: Admin/ApproveAttendanceRequest/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveAttendanceRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate approval logic here (replace with actual approval logic)

            return RedirectToAction("AttendanceRequests");
        }

        // POST: Admin/RejectAttendanceRequest/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectAttendanceRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate rejection logic here (replace with actual rejection logic)

            return RedirectToAction("AttendanceRequests");
        }

        // GET: Admin/LeaveRequests
        public ActionResult LeaveRequests()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            var requests = new List<object>
            {
                new { RequestID = 1, EmployeeName = "Jane Doe", RequestDate = DateTime.Now.AddDays(-2), Status = "Pending" }
                // Add more requests as necessary
            };

            ViewBag.Requests = requests; // Pass data to view
            return View();
        }

        // POST: Admin/ApproveLeaveRequest/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveLeaveRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate approval logic here (replace with actual approval logic)

            return RedirectToAction("LeaveRequests");
        }

        // POST: Admin/RejectLeaveRequest/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectLeaveRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }

            // Simulate rejection logic here (replace with actual rejection logic)

            return RedirectToAction("LeaveRequests");
        }

        // Helper method to validate admin credentials
        private bool IsValidAdmin(string username, string password)
        {
            // Replace with actual credential validation logic
            return username == "admin" && password == "password";
        }
    }
}
