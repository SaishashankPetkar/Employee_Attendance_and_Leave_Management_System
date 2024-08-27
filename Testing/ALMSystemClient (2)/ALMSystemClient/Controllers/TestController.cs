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
    public class TestController : Controller
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
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Display()
        {
            IEnumerable<MVCEmployees> emplist = null;
            using(var webclient = GetHttpClient())
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
        public async Task<ActionResult> Create()
        {
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
        public async Task<ActionResult> Create(EmployeeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var webclient = GetHttpClient())
                {
                    var response = await webclient.PostAsJsonAsync("Employees", viewModel.Employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Display");
                    }
                    ModelState.AddModelError(string.Empty, "Error occurred while saving the employee.");
                }
            }

            // Fetch roles and projects again if model is not valid
            viewModel.Roles = await FetchRoles();
            viewModel.Projects = await FetchProjects();

            return View(viewModel);
        }

        //[HttpPost]
        //public async Task<ActionResult> Create(MVCEmployees emp)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (var webclient = GetHttpClient())
        //        {
        //            var response = await webclient.PostAsJsonAsync("Employees", emp);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Display");
        //            }
        //            ModelState.AddModelError(string.Empty, "Insertion Failed.. try later");
        //        }
        //    }
        //    return View(emp);
        //}

        [HttpGet]
        public async Task<ActionResult> Edit(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MVCEmployees emp = null;
            IEnumerable<Role> roles = null;
            IEnumerable<Project> projects = null;
            using (var webclient = GetHttpClient())
            {
                var edittalk = await webclient.GetAsync($"Employees/{Id}");
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
            if(emp == null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var webclient = GetHttpClient())
                {
                    var response = await webclient.PutAsJsonAsync($"Employees/{viewModel.Employee.EmployeeID}", viewModel.Employee);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Display");
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
        public async Task<ActionResult> SoftDelete(int id)
        {
            using (var webclient = GetHttpClient())
            {
                var response = await webclient.PostAsync($"Employees/SoftDelete/{id}", null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Display");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error occurred while deleting the employee.");
                }
            }

            return RedirectToAction("Display");
        }
    }
}


