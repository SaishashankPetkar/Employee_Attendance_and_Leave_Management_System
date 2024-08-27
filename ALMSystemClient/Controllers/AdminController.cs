using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ALMSystem2.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ALMSystem2.Controllers
{
    public class AdminController : Controller
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

        private bool IsValidAdmin(string username, string password)
        {
            // Replace with actual credential validation logic
            return username == "admin" && password == "password";
        }
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

            // Set error message in ViewBag
            ViewBag.ErrorMessage = "Invalid login Credentials.";
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

        // GET: Admin/Dashboard

        public async Task<ActionResult> EmployeeDetails()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            IEnumerable<MVCEmployees> emplist = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync("Employees");
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
                var response = await webclient.GetAsync("Roles");
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
                var response = await webclient.GetAsync("Projects");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Project>>(resultData);
                }
                return Enumerable.Empty<Project>();
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateNewEmployee()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = new MVCEmployees(),
                Roles = await FetchRoles(),
                Projects = await FetchProjects()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewEmployee(EmployeeCreateViewModel viewModel)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            if (ModelState.IsValid)
            {
                using (var webclient = GetHttpClient())
                {
                    var response = await webclient.PostAsJsonAsync("Employees", viewModel.Employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("EmployeeDetails");
                    }
                    ModelState.AddModelError(string.Empty, "Error occurred while saving the employee.");
                }
            }

            // Fetch roles and projects again if model is not valid
            viewModel.Roles = await FetchRoles();
            viewModel.Projects = await FetchProjects();

            return View(viewModel);
        }


        // GET: Admin/EditEmployee/1
        public async Task<ActionResult> EditEmployee(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MVCEmployees emp = null;
            IEnumerable<Role> roles = null;
            IEnumerable<Project> projects = null;
            using (var webclient = GetHttpClient())
            {
                var edittalk = await webclient.GetAsync($"Employees/{id}");
                if (edittalk.IsSuccessStatusCode)
                {
                    var resultdata = await edittalk.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<MVCEmployees>(resultdata);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Some error occurred while processing your request");
                }

                var rolesResponse = await webclient.GetAsync("Roles");
                if (rolesResponse.IsSuccessStatusCode)
                {
                    var rolesData = await rolesResponse.Content.ReadAsStringAsync();
                    roles = JsonConvert.DeserializeObject<List<Role>>(rolesData);
                }

                var projectsResponse = await webclient.GetAsync("Projects");
                if (projectsResponse.IsSuccessStatusCode)
                {
                    var projectsData = await projectsResponse.Content.ReadAsStringAsync();
                    projects = JsonConvert.DeserializeObject<List<Project>>(projectsData);
                }
            }
            if (emp == null)
            {
                return HttpNotFound();
            }
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = emp,
                Roles = roles,
                Projects = projects
            };
            return View(viewModel);
        }

        // POST: Admin/EditEmployee/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditEmployee(EmployeeCreateViewModel viewModel)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            if (ModelState.IsValid)
            {
                using (var webclient = GetHttpClient())
                {
                    var response = await webclient.PutAsJsonAsync($"Employees/{viewModel.Employee.EmployeeID}", viewModel.Employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("EmployeeDetails");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error Occurred");
                    }
                }
            }

            viewModel.Roles = await FetchRoles();
            viewModel.Projects = await FetchProjects();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Employees/SoftDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("EmployeeDetails");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleting the employee.");
                }
            }

            return RedirectToAction("EmployeeDetails");
        }

        public async Task<ActionResult> ProjectDetails()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            IEnumerable<Project> projects = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync("Project");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    projects = JsonConvert.DeserializeObject<List<Project>>(resultData);
                }
                else
                {
                    projects = Enumerable.Empty<Project>();
                }
            }
            return View(projects);
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
        public async Task<ActionResult> CreateNewProject(Project project)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsJsonAsync("Project", project);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProjectDetails");
                }
                ModelState.AddModelError(string.Empty, "Insertion Failed.. try later");
            }
            return View(project);
        }

        // GET: Admin/EditProject/1
        public async Task<ActionResult> EditProject(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            // Fetch the project from the database using the id (replace with actual data retrieval logic)
            Project project = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync($"Project/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    project = JsonConvert.DeserializeObject<Project>(resultData);
                }
            }
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Admin/EditProject/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProject(Project project)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            if (ModelState.IsValid)
            {
                using (var webclient = GetHttpClient())
                {
                    var response = await webclient.PutAsJsonAsync($"Project/{project.ProjectID}", project);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("ProjectDetails");
                    }
                    ModelState.AddModelError(string.Empty, "Update Failed.. try later");
                }
            }
            return View(project);
        }

        // POST: Admin/DeleteProject/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProject(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Project/SoftDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProjectDetails");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleting the employee.");
                }
            }

            return RedirectToAction("ProjectDetails");
        }

        public async Task<ActionResult> AttendanceRequests()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            IEnumerable<Attendance> attendanceList = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync("Attendance");
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
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Attendance/Approve/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AttendanceRequests");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while Approving the attendance.");
                }
            }

            return RedirectToAction("AttendanceRequests");
        }

        // POST: Admin/RejectAttendanceRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectAttendanceRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Attendance/Reject/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AttendanceRequests");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while Rejecting the attendance.");
                }
            }

            return RedirectToAction("AttendanceRequests");
        }

        // GET: Admin/LeaveRequests
        public async Task<ActionResult> LeaveRequests()
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            IEnumerable<Leave> leaveList = null;
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.GetAsync("Leave");
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
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Leave/Approve/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LeaveRequests");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while Approving the Leave.");
                }
            }

            return RedirectToAction("LeaveRequests");
        }

        // POST: Admin/RejectLeaveRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RejectLeaveRequest(int id)
        {
            if (Session["User"] == null) // Check if the user is authenticated
            {
                return RedirectToAction("AdminLogin");
            }
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Leave/Reject/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LeaveRequests");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while Rejecting the Leave.");
                }
            }

            return RedirectToAction("LeaveRequests");
        }
    }
}