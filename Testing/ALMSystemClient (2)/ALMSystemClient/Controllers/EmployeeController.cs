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

        [HttpGet]
        public ActionResult ApplyLeave()
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
        public async Task<ActionResult> ApplyLeave(Leave leave)
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
                    return RedirectToAction("EmployeeDashboard");
                }
                ModelState.AddModelError(string.Empty, "Insertion Failed.. try later or Your leave Balance is Over check your balance in dashboard");
            }
            return View(leave);
        }

        [HttpGet]
        public ActionResult SubmitAttendance()
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
        public async Task<ActionResult> SubmitAttendance(Attendance attendance)
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
                    return RedirectToAction("EmployeeDashboard");
                }
                ModelState.AddModelError(string.Empty, "Insertion Failed.. try later");
            }
            return View(attendance);
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