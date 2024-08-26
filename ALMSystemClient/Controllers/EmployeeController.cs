using System;
using System.Collections.Generic;
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
            if (IsValidEmployee(username, password))
            {
                Session["IsLoggedIn"] = true;
                Session["UserRole"] = "Employee"; // Set role to Employee
                return RedirectToAction("EmployeeDashboard");
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // GET: Employee/EmployeeDashboard
        public ActionResult EmployeeDashboard()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }

        // GET: Employee/ApplyLeave
        public ActionResult ApplyLeave()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return RedirectToAction("LeaveDetails"); // Redirecting to LeaveDetails page
        }

        // POST: Employee/ApplyLeave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyLeave(FormCollection form)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Simulate leave application submission
                TempData["Message"] = "Leave application submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while submitting the leave application: " + ex.Message;
            }

            return RedirectToAction("LeaveDetails"); // Redirecting to LeaveDetails page
        }

        // GET: Employee/SubmitAttendance
        public ActionResult SubmitAttendance()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            return RedirectToAction("AttendanceDetails"); // Redirecting to AttendanceDetails page
        }

        // POST: Employee/SubmitAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAttendance(FormCollection form)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Simulate attendance submission
                TempData["Message"] = "Attendance submitted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while submitting the attendance: " + ex.Message;
            }

            return RedirectToAction("AttendanceDetails"); // Redirecting to AttendanceDetails page
        }

        // GET: Employee/LeaveDetails
        public ActionResult LeaveDetails()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            // Simulate leave request data
            var leaveRequests = new List<object>
            {
                new { LeaveID = 1, EmployeeID = 1, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(-5), LeaveType = "Sick", Reason = "Flu", ApprovalStatus = "Approved", ManagerID = 2 }
                // Add more leave requests as necessary
            };

            ViewBag.LeaveRequests = leaveRequests;
            return View();
        }


        // GET: Employee/CreateLeave
        public ActionResult CreateLeave()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            // Retrieve list of employees and managers
            ViewBag.Employees = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "John Doe" }
        // Add more employees as necessary
    };

            ViewBag.Managers = new List<SelectListItem>
    {
        new SelectListItem { Value = "2", Text = "Jane Smith" }
        // Add more managers as necessary
    };

            return View();
        }

        // POST: Employee/CreateLeave
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLeave(FormCollection form)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Extract form values
                int employeeID = Convert.ToInt32(form["EmployeeID"]);
                DateTime startDate = Convert.ToDateTime(form["StartDate"]);
                DateTime endDate = Convert.ToDateTime(form["EndDate"]);
                string leaveType = form["LeaveType"];
                string reason = form["Reason"];
                string approvalStatus = form["ApprovalStatus"] ?? "Pending";
                int managerID = Convert.ToInt32(form["ManagerID"]);

                // Add the new leave request to the database
                TempData["SuccessMessage"] = "Leave request created successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the leave request: " + ex.Message;
            }

            return RedirectToAction("LeaveDetails");
        }

        // POST: Employee/DeleteLeave/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLeave(int id)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Simulate delete logic here
                TempData["SuccessMessage"] = "Leave request deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the leave request: " + ex.Message;
            }

            return RedirectToAction("LeaveDetails");
        }

        // GET: Employee/AttendanceDetails
        public ActionResult AttendanceDetails()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            // Simulate attendance data
            var attendanceRecords = new List<object>
            {
                new { AttendanceID = 1, EmployeeID = 1, ProjectID = 101, AttendanceDate = DateTime.Now.AddDays(-10), Status = "Present", ApprovalStatus = "Approved", ManagerID = 2 }
                // Add more attendance records as necessary
            };

            ViewBag.AttendanceRecords = attendanceRecords;
            return View();
        }

        // GET: Employee/CreateAttendance
        public ActionResult CreateAttendance()
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            // Retrieve list of employees, managers, and projects
            ViewBag.Employees = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "John Doe" }
                // Add more employees as necessary
            };

            ViewBag.Managers = new List<SelectListItem>
            {
                new SelectListItem { Value = "2", Text = "Jane Smith" }
                // Add more managers as necessary
            };

            ViewBag.Projects = new List<SelectListItem>
            {
                new SelectListItem { Value = "101", Text = "Project A" }
                // Add more projects as necessary
            };

            return View();
        }

        // POST: Employee/CreateAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAttendance(FormCollection form)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Extract form values
                int employeeID = Convert.ToInt32(form["EmployeeID"]);
                int projectID = Convert.ToInt32(form["ProjectID"]);
                DateTime attendanceDate = Convert.ToDateTime(form["AttendanceDate"]);
                string status = form["Status"];
                string approvalStatus = form["ApprovalStatus"] ?? "Pending";
                int managerID = Convert.ToInt32(form["ManagerID"]);

                // Add the new attendance record to the database
                TempData["SuccessMessage"] = "Attendance record created successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the attendance record: " + ex.Message;
            }

            return RedirectToAction("AttendanceDetails");
        }

        // POST: Employee/DeleteAttendance/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttendance(int id)
        {
            if (!IsEmployeeAuthenticated())
            {
                return RedirectToAction("EmployeeLogin");
            }

            try
            {
                // Simulate delete logic here
                TempData["SuccessMessage"] = "Attendance record deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the attendance record: " + ex.Message;
            }

            return RedirectToAction("AttendanceDetails");
        }

        private bool IsValidEmployee(string username, string password)
        {
            // Simulate employee validation
            return username == "employee" && password == "password"; // Adjust to actual employee validation logic
        }

        private bool IsEmployeeAuthenticated()
        {
            return Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"] && Session["UserRole"] == "Employee";
        }
    }
}
