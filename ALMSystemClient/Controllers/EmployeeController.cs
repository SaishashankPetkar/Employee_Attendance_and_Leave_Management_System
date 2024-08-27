using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ALMSystem2.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace ALMSystem2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string _baseAddress = "https://localhost:44323/api/";

        // Private method to get HttpClient
        private HttpClient GetHttpClient()
        {
            var webclient = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            return webclient;
        }
        public ActionResult EmployeeLogin()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                employeeCookie.Expires = DateTime.Now.AddDays(-1); // Set the expiration date to a past date
                Response.Cookies.Add(employeeCookie); // Add the cookie to the response to update it
            }

            // Redirect to the login page or any other page as needed
            return RedirectToAction("EmployeeLogin");
        }

        public ActionResult EmployeeDashboard()
        {
            // Retrieve employee data from session
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.Employee = employee;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }

            return View();
        }

        // GET: Employee/LeaveDetails
        public async Task<ActionResult> LeaveDetails()
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.Employee = employee;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }

            IEnumerable<Leave> leaveList = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync($"Leave/{ViewBag.Employee.EmployeeID}");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    leaveList = JsonConvert.DeserializeObject<List<Leave>>(resultData);
                }
                else
                {
                    leaveList = Enumerable.Empty<Leave>();
                }
            }
            return View(leaveList);
        }


        [HttpGet]
        public ActionResult CreateLeave()
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                var model = new Leave
                {
                    EmployeeID = Convert.ToInt32(employee.EmployeeID),
                    ManagerID = Convert.ToInt32(employee.ManagerID)
                };

                return View(model);
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLeave(Leave leave)
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.EmployeeId = employee.EmployeeID;
                ViewBag.ProjectID = employee.ProjectID;
                ViewBag.ManagerID = employee.ManagerID;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsJsonAsync("Leave", leave);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LeaveDetails");
                }
                ModelState.AddModelError(string.Empty, "Insertion Failed.. try later or Your leave Balance is Over check your balance in dashboard");
            }
            return View(leave);
        }

        // POST:/DeleteProject/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLeave(int id)
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.EmployeeId = employee.EmployeeID;
                ViewBag.ProjectID = employee.ProjectID;
                ViewBag.ManagerID = employee.ManagerID;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }
          
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Leave/SoftDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LeaveDetails");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleting leave.");
                }
            }

            return RedirectToAction("LeaveDetails");
        }

        // GET: Employee/AttendanceDetails
        public async Task<ActionResult> AttendanceDetails()
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.Employee = employee;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }

            IEnumerable<Attendance> AttdList = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync($"Attendance/{ViewBag.Employee.EmployeeID}");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    AttdList = JsonConvert.DeserializeObject<List<Attendance>>(resultData);
                }
                else
                {
                    AttdList = Enumerable.Empty<Attendance>();
                }
            }
            return View(AttdList);
        }
    
        [HttpGet]
        public ActionResult CreateAttendance()
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                var model = new Attendance
                {
                    EmployeeID = Convert.ToInt32(employee.EmployeeID),
                    ProjectID = Convert.ToInt32(employee.ProjectID),
                    ManagerID = Convert.ToInt32(employee.ManagerID)
                };
                // Initialize other properties if necessary
                return View(model);
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAttendance(Attendance attendance)
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.EmployeeId = employee.EmployeeID;
                ViewBag.ProjectID = employee.ProjectID;
                ViewBag.ManagerID = employee.ManagerID;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsJsonAsync("Attendance", attendance);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AttendanceDetails");
                }
                ModelState.AddModelError(string.Empty, "Insertion Failed.. try later");
            }
            return View(attendance);
        }

        //Delete Attendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAttendance(int id)
        {
            var employeeCookie = Request.Cookies["employee"];
            if (employeeCookie != null)
            {
                var employeeData = employeeCookie.Value;
                var employee = JsonConvert.DeserializeObject<dynamic>(HttpUtility.UrlDecode(employeeData));
                ViewBag.EmployeeId = employee.EmployeeID;
                ViewBag.ProjectID = employee.ProjectID;
                ViewBag.ManagerID = employee.ManagerID;
            }
            else
            {
                return RedirectToAction("EmployeeLogin");
            }

            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Attendance/SoftDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AttendanceDetails");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleting Attendance.");
                }
            }

            return RedirectToAction("AttendanceDetails");
        }




        //// GET: Employee/ViewProjects
        //public ActionResult ViewProjects()
        //{
        //    //if (!IsUserAuthenticated())
        //    //{
        //    //    return RedirectToAction("EmployeeLogin");
        //    //}

        //    var projects = new List<string> { "Project A", "Project B", "Project C" };
        //    ViewBag.Projects = projects;

        //    return View();
        //}


    }
}