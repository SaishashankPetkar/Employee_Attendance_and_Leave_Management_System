//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace WebApi_server.Controllers
//{
//    public class AttendanceController : Controller
//    {
//        // GET: Attendance
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ALMSystemWebApi.Models;

namespace ALMSystemWebApi.Controllers
{
    public class AttendanceController : ApiController
    {
        private LeaveMasterEntities2 db = new LeaveMasterEntities2();

        // GET: api/Attendance
        public IQueryable<Attendance> GetAttendances()
        {
            return db.Attendances.Where(a => a.Atd_Status == "Active" && a.ApprovalStatus== "Pending");
        }

        [HttpPost]
        [Route("api/Attendance/Approve/{id}")]
        public async Task<IHttpActionResult> Approve(int id)
        {
            var a_attendance = await db.Attendances.FindAsync(id);
            if (a_attendance == null)
            {
                return NotFound();
            }

            a_attendance.ApprovalStatus = "Approved";
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("api/Attendance/Reject/{id}")]
        public async Task<IHttpActionResult> Reject(int id)
        {
            var a_attendance = await db.Attendances.FindAsync(id);
            if (a_attendance == null)
            {
                return NotFound();
            }

            a_attendance.ApprovalStatus = "Rejected";
            await db.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Attendance/5
        public IQueryable<Attendance> GetAttendance(int id)
        {
            
            return db.Attendances.Where(a => a.Atd_Status == "Active" && a.EmployeeID == id);
        }

        //api/Attendance/Manager/id
        [HttpGet]
        [Route("api/Attendance/Manager/{id}")]
        public IQueryable<Attendance> GetAttendanceByManager(int id)
        {

            return db.Attendances.Where(a => a.Atd_Status == "Active" && a.ManagerID == id && a.ApprovalStatus == "Pending");
        }

        // PUT: api/Attendance/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutAttendance(int id, Attendance attendance)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != attendance.AttendanceID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(attendance).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AttendanceExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Attendance
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult PostAttendance(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(attendance.ApprovalStatus))
            {
                attendance.ApprovalStatus = "Pending";
            }
            if (string.IsNullOrEmpty(attendance.Atd_Status))
            {
                attendance.Atd_Status = "Active";
            }
            db.Attendances.Add(attendance);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        Console.WriteLine("Property: { 0}, Error: { 1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = attendance.AttendanceID }, attendance);
        }

        // DELETE: api/Attendance/5
        [HttpPost]
        [Route("api/Attendance/SoftDelete/{id}")]
        [ResponseType(typeof(Attendance))]
        public IHttpActionResult DeleteAttendance(int id)
        {
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return NotFound();
            }

            // Instead of actually deleting the record, set the status to 'Inactive'
            attendance.Atd_Status = "Inactive";
            db.Entry(attendance).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(attendance);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AttendanceExists(int id)
        {
            return db.Attendances.Count(e => e.AttendanceID == id) > 0;
        }
    }
}
