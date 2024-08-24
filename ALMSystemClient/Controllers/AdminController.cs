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

        public ActionResult EmployeeDetails()
        {
            var employees = new List<object>
    {
        new { EmployeeID = 1, EmployeeName = "John Doe", Email = "johndoe@example.com", Phone = "123-456-7890", HireDate = DateTime.Now.AddYears(-5), RoleID = 1, ManagerID = 2, ProjectID = 1, LeaveBalance = 10, No_of_leave = 5, Emp_status = "Active" }
        // Add more employees as necessary
    };
            return View();
        }
        

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
            // Example:
            // db.AddEmployee(employeeName, email, phone, hireDate, roleID, managerID, projectID, leaveBalance, no_of_leave, emp_status);

            // After adding the new employee, redirect to EmployeeDetails
            return RedirectToAction("EmployeeDetails");
        }


        // GET: Admin/EditEmployee/1
        public ActionResult EditEmployee(int id)
        {
            // Ideally, fetch the employee from the database using the id
            // Example (replace with actual data retrieval logic):
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

            // Pass the data to the view using ViewBag
            ViewBag.EmployeeName = employee.EmployeeName;
            ViewBag.Email = employee.Email;
            ViewBag.Phone = employee.Phone;
            ViewBag.HireDate = employee.HireDate.ToShortDateString();
            ViewBag.RoleID = employee.RoleID;
            ViewBag.ManagerID = employee.ManagerID;
            ViewBag.ProjectID = employee.ProjectID;
            ViewBag.LeaveBalance = employee.LeaveBalance;
            ViewBag.No_of_leave = employee.No_of_leave;
            ViewBag.Emp_status = employee.Emp_status;

            return View();
        }

        // POST: Admin/EditEmployee/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(int id, FormCollection form)
        {
            // You can extract form values like this (replace with actual logic)
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

            return RedirectToAction("EmployeeDetails");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(int id)
        {
            // Simulate delete logic here

            return RedirectToAction("EmployeeDetails");
        }
    }
}