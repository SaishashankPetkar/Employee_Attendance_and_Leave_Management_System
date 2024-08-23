//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Newtonsoft.Json;
//using WebApi_Client.Models;

//namespace WebApi_Client.Controllers
//{
//    public class AttendanceController : Controller
//    {
//        public readonly string apiUrl = "https://localhost:44368/api/"; // Adjust API URL as needed

//        GET: Attendance
//        public ActionResult Index()
//        {
//            return View();
//        }

//        GET: Attendance/Display
//        public async Task<ActionResult> Display()
//        {
//            IEnumerable<MVC_Attendances> attendanceList = null;
//            using (var webClient = new HttpClient())
//            {
//                webClient.BaseAddress = new Uri("https://localhost:44368/api/");
//                var response = await webClient.GetAsync("");
//                if (response.IsSuccessStatusCode)
//                {
//                    var resultData = await response.Content.ReadAsStringAsync();
//                    attendanceList = JsonConvert.DeserializeObject<List<MVC_Attendances>>(resultData);
//                }
//                else
//                {
//                    attendanceList = Enumerable.Empty<MVC_Attendances>();
//                }
//            }
//            return View(attendanceList);
//        }

//        GET: Attendance/Create
//       [HttpGet]
//        public ActionResult Create()
//        {
//            return View();
//        }

//        POST: Attendance/Create
//       [HttpPost]
//       [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Create(MVC_Attendances attendance)
//        {
//            if (ModelState.IsValid)
//            {
//                using (var webClient = new HttpClient())
//                {
//                    webClient.BaseAddress = new Uri(apiUrl);
//                    var response = await webClient.GetAsync("", attendance);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        return RedirectToAction("Display");
//                    }
//                    ModelState.AddModelError(string.Empty, "Insertion Failed. Try again later.");
//                }
//            }
//            return View(attendance);
//        }

//        GET: Attendance/Edit/5
//        [HttpGet]
//        public async Task<ActionResult> Edit(int id)
//        {
//            if (id == 0)
//            {
//                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
//            }

//            MVC_Attendances attendance = null;
//            using (var webClient = new HttpClient())
//            {
//                webClient.BaseAddress = new Uri(apiUrl);
//                var response = await webClient.GetAsync($"{id}");
//                if (response.IsSuccessStatusCode)
//                {
//                    var resultData = await response.Content.ReadAsStringAsync();
//                    attendance = JsonConvert.DeserializeObject<MVC_Attendances>(resultData);
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Some error occurred while processing your request.");
//                }
//            }

//            if (attendance == null)
//            {
//                return HttpNotFound();
//            }

//            return View(attendance);
//        }

//        POST: Attendance/Edit
//       [HttpPost]
//       [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Edit([Bind(Include = "AttendanceID,EmployeeID,ProjectID,AttendanceDate,Status,ApprovalStatus,ManagerID")] Attendance attendance)
//        {
//            if (ModelState.IsValid)
//            {
//                using (var webClient = new HttpClient())
//                {
//                    webClient.BaseAddress = new Uri(apiUrl);
//                    var response = await webClient.PutAsJsonAsync($"{attendance.AttendanceID}", attendance);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        return RedirectToAction("Display");
//                    }
//                    ModelState.AddModelError(string.Empty, "Error occurred while updating.");
//                }
//            }
//            return View(attendance);
//        }

//        GET: Attendance/Delete/5
//        [HttpGet]
//        public async Task<ActionResult> Delete(int id)
//        {
//            if (id == 0)
//            {
//                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
//            }

//            MVC_Attendances attendance = null;
//            using (var webClient = new HttpClient())
//            {
//                webClient.BaseAddress = new Uri(apiUrl);
//                var response = await webClient.GetAsync($"{id}");
//                if (response.IsSuccessStatusCode)
//                {
//                    var resultData = await response.Content.ReadAsStringAsync();
//                    attendance = JsonConvert.DeserializeObject<MVC_Attendances>(resultData);
//                }
//                else
//                {
//                    ModelState.AddModelError(string.Empty, "Some error occurred while processing your request.");
//                }
//            }

//            if (attendance == null)
//            {
//                return HttpNotFound();
//            }

//            return View(attendance);
//        }

//        POST: Attendance/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> DeleteConfirmed(int id)
//        {
//            using (var webClient = new HttpClient())
//            {
//                webClient.BaseAddress = new Uri(apiUrl);
//                var response = await webClient.DeleteAsync($"{id}");
//                if (response.IsSuccessStatusCode)
//                {
//                    return RedirectToAction("Display");
//                }
//                ModelState.AddModelError(string.Empty, "Error occurred while deleting.");
//            }
//            return RedirectToAction("Display");
//        }
//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApi_Client.Models;

namespace WebApi_Client.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly string apiUrl = "https://localhost:44368/api/"; // Adjust API URL as needed

        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }

        // GET: Attendance/Display
        public async Task<ActionResult> Display()
        {
            IEnumerable<MVC_Attendances> attendanceList = null;
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                var response = await webClient.GetAsync(""); // GET all attendances
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    attendanceList = JsonConvert.DeserializeObject<List<MVC_Attendances>>(resultData);
                }
                else
                {
                    attendanceList = Enumerable.Empty<MVC_Attendances>();
                }
            }
            return View(attendanceList);
        }

        // GET: Attendance/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MVC_Attendances attendance)
        {
            if (ModelState.IsValid)
            {
                using (var webClient = new HttpClient())
                {
                    webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                    var response = await webClient.PostAsJsonAsync("", attendance);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Display");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Insertion Failed. Try again later.");
                    }
                }
            }
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            MVC_Attendances attendance = null;
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                var response = await webClient.GetAsync($"{id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    attendance = JsonConvert.DeserializeObject<MVC_Attendances>(resultData);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Some error occurred while processing your request.");
                }
            }

            if (attendance == null)
            {
                return HttpNotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AttendanceID,EmployeeID,ProjectID,AttendanceDate,Status,ApprovalStatus,ManagerID")] MVC_Attendances attendance)
        {
            if (ModelState.IsValid)
            {
                using (var webClient = new HttpClient())
                {
                    webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                    var response = await webClient.PutAsJsonAsync($"{attendance.AttendanceID}", attendance);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Display");
                    }
                    ModelState.AddModelError(string.Empty, "Error occurred while updating.");
                }
            }
            return View(attendance);
        }

        // GET: Attendance/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            MVC_Attendances attendance = null;
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                var response = await webClient.GetAsync($"{id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultData = await response.Content.ReadAsStringAsync();
                    attendance = JsonConvert.DeserializeObject<MVC_Attendances>(resultData);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Some error occurred while processing your request.");
                }
            }

            if (attendance == null)
            {
                return HttpNotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var webClient = new HttpClient())
            {
                webClient.BaseAddress = new Uri("https://localhost:44368/api/attendances");
                var response = await webClient.DeleteAsync($"{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Display");
                }
                ModelState.AddModelError(string.Empty, "Error occurred while deleting.");
            }
            return RedirectToAction("Display");
        }
    }
}

