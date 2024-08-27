
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

public class ManagerController : Controller
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
    // GET: Manager/Login
    public ActionResult ManagerLogin()
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
        return RedirectToAction("ManagerLogin");
    }



    // GET: Manager/Dashboard
    public ActionResult ManagerDashboard()
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
            return RedirectToAction("ManagerLogin");
        }

        return View();
    }

    public async Task<ActionResult> ViewPendingAttendance()
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
            return RedirectToAction("ManagerLogin");
        }
        IEnumerable<Attendance> attendanceList = null;
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.GetAsync($"Attendance/Manager/{ViewBag.Employee.EmployeeID}");
            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStringAsync();
                attendanceList = JsonConvert.DeserializeObject<List<Attendance>>(resultData);
            }
            else
            {
                attendanceList = Enumerable.Empty<Attendance>();
            }
        }
        return View(attendanceList);
    }

    // POST: Admin/ApproveAttendanceRequest/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ApproveAttendanceRequest(int id)
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
            return RedirectToAction("ManagerLogin");
        }
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.PostAsync($"Attendance/Approve/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewPendingAttendance");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error occurred while Approving the attendance.");
            }
        }

        return RedirectToAction("ViewPendingAttendance");
    }

    // POST: Admin/RejectAttendanceRequest/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> RejectAttendanceRequest(int id)
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
            return RedirectToAction("ManagerLogin");
        }
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.PostAsync($"Attendance/Reject/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewPendingAttendance");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error occurred while Rejecting the attendance.");
            }
        }

        return RedirectToAction("ViewPendingAttendance");
    }

    // GET: Manager/ViewPendingLeave
    public async Task<ActionResult> ViewPendingLeave()
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
            return RedirectToAction("ManagerLogin");
        }
        IEnumerable<Leave> leaveList = null;
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.GetAsync($"Leave/Manager/{ViewBag.Employee.EmployeeID}");
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

    // POST: Admin/ApproveLeaveRequest/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ApproveLeaveRequest(int id)
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
            return RedirectToAction("ManagerLogin");
        }
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.PostAsync($"Leave/Approve/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewPendingLeave");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error occurred while Approving the Leave.");
            }
        }

        return RedirectToAction("ViewPendingLeave");
    }

    // POST: Admin/RejectLeaveRequest/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> RejectLeaveRequest(int id)
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
            return RedirectToAction("ManagerLogin");
        }
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.PostAsync($"Leave/Reject/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewPendingLeave");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error occurred while Rejecting the Leave.");
            }
        }

        return RedirectToAction("ViewPendingLeave");
    }



    // GET: Admin/Dashboard

    public async Task<ActionResult> EmployeeDetails()
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
            return RedirectToAction("ManagerLogin");
        }
        IEnumerable<MVCEmployees> emplist = null;
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.GetAsync($"Manager/{ViewBag.Employee.EmployeeID}");
            if (response.IsSuccessStatusCode)
            {
                var resultdata = response.Content.ReadAsStringAsync().Result;
                emplist = JsonConvert.DeserializeObject<List<MVCEmployees>>(resultdata);
            }
            else
            {
                emplist = Enumerable.Empty<MVCEmployees>();
            }
            return View(emplist);
        }
    }

    private async Task<IEnumerable<Role>> FetchRoles()
    {
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.GetAsync("Manager/Roles");
            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Role>>(resultData);
            }
            return Enumerable.Empty<Role>();
        }
    }

    private async Task<IEnumerable<Project>> FetchProjects()
    {
        using (var webclient = GetHttpClient())
        {
            var response = await webclient.GetAsync("Manager/Projects");
            if (response.IsSuccessStatusCode)
            {
                var resultData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Project>>(resultData);
            }
            return Enumerable.Empty<Project>();
        }
    }


}
