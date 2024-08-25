using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALMSystem2.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Login
        public ActionResult AdminLogin()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(string username, string password)
        {
            if (username == "admin" && password == "password") // Example validation
            {
                return RedirectToAction("AdminDashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }

        // GET: Admin/Dashboard
        public ActionResult AdminDashboard()
        {
            return View();
        }

        // GET: Admin/EmployeeDetails
        public ActionResult EmployeeDetails()
        {
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
            return View();
        }

        // POST: Admin/CreateNewEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewEmployee(FormCollection form)
        {
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
            // Simulate delete logic here (replace with actual delete logic)

            return RedirectToAction("EmployeeDetails");
        }

        // GET: Admin/ProjectDetails
        public ActionResult ProjectDetails()
        {
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
            return View();
        }

        // POST: Admin/CreateNewProject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewProject(FormCollection form)
        {
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
            // Simulate delete logic here (replace with actual delete logic)

            return RedirectToAction("ProjectDetails");
        }

        // GET: Admin/AttendanceRequests
        public ActionResult AttendanceRequests()
        {
            var attendanceRequests = new List<object>
            {
                new { EmployeeName = "John Doe", Date = "2024-08-20", Status = "Pending" },
                new { EmployeeName = "Jane Smith", Date = "2024-08-21", Status = "Pending" }
            };

            ViewBag.AttendanceRequests = attendanceRequests; // Pass data to view
            return View();
        }

        // POST: Admin/ApproveAttendanceRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveAttendanceRequest(int id)
        {
            // Logic to approve the attendance request
            TempData["Message"] = "Attendance request approved.";
            return RedirectToAction("AttendanceRequests");
        }

        // POST: Admin/RejectAttendanceRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectAttendanceRequest(int id)
        {
            // Logic to reject the attendance request
            TempData["Message"] = "Attendance request rejected.";
            return RedirectToAction("AttendanceRequests");
        }

        // GET: Admin/LeaveRequests
        public ActionResult LeaveRequests()
        {
            var leaveRequests = new List<object>
            {
                new { EmployeeName = "Michael Brown", LeaveType = "Sick Leave", StartDate = "2024-08-20", EndDate = "2024-08-22", Status = "Pending" },
                new { EmployeeName = "Emily Davis", LeaveType = "Annual Leave", StartDate = "2024-08-25", EndDate = "2024-08-30", Status = "Pending" }
            };

            ViewBag.LeaveRequests = leaveRequests; // Pass data to view
            return View();
        }

        // POST: Admin/ApproveLeaveRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveLeaveRequest(int id)
        {
            // Logic to approve the leave request
            TempData["Message"] = "Leave request approved.";
            return RedirectToAction("LeaveRequests");
        }

        // POST: Admin/RejectLeaveRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectLeaveRequest(int id)
        {
            // Logic to reject the leave request
            TempData["Message"] = "Leave request rejected.";
            return RedirectToAction("LeaveRequests");
        }
    }
}