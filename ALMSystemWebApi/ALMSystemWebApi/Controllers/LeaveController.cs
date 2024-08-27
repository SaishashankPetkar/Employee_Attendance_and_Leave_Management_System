////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Web;
////using System.Web.Mvc;

////namespace WebApi_server.Controllers
////{
////    public class LeaveController : Controller
////    {
////        // GET: Leave
////        public ActionResult Index()
////        {
////            return View();
////        }
////    }

////}

using System;
using System.Collections.Generic;
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

namespace WebApi_server.Controllers
{
    public class LeaveController : ApiController
    {
        private LeaveMasterEntities2 db = new LeaveMasterEntities2();

        // GET api/leaves
        public IEnumerable<Leaf> GetLeaves()
        {
            return db.Leaves.Where(l => l.Leave_Status == "Active" && l.ApprovalStatus == "Pending");
        }

        [HttpPost]
        [Route("api/Leave/Approve/{id}")]
        public async Task<IHttpActionResult> Approve(int id)
        {
            var a_leave = await db.Leaves.FindAsync(id);
            if (a_leave == null)
            {
                return NotFound();
            }
            var emp = await db.Employees.FindAsync(a_leave.EmployeeID);
            if (emp == null)
            {
                return NotFound();
            }
            else
            {
                emp.LeaveBalance = emp.LeaveBalance - 1;
                emp.No_of_leave = emp.No_of_leave + 1;
            }
            a_leave.ApprovalStatus = "Approved";
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("api/Leave/Reject/{id}")]
        public async Task<IHttpActionResult> Reject(int id)
        {
            var a_leave = await db.Leaves.FindAsync(id);
            if (a_leave == null)
            {
                return NotFound();
            }

            a_leave.ApprovalStatus = "Rejected";
            await db.SaveChangesAsync();

            return Ok();
        }
        // GET api/leaves/5
        [HttpGet]
        public IQueryable<Leaf> GetLeaves(int id)
        {
            return db.Leaves.Where(l => l.Leave_Status == "Active" && l.EmployeeID == id);
        }

        //api/Leave/Manager/id
        [HttpGet]
        [Route("api/Leave/Manager/{id}")]
        public IQueryable<Leaf> GetLeaveByManager(int id)
        {

            return db.Leaves.Where(l => l.Leave_Status == "Active" && l.ManagerID == id && l.ApprovalStatus == "Pending");
        }

        // POST api/leaves
        [HttpPost]
        [ResponseType(typeof(Leaf))]
        public IHttpActionResult PostLeave(Leaf leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (string.IsNullOrEmpty(leave.ApprovalStatus))
            {
                leave.ApprovalStatus = "Pending";
            }
            if (string.IsNullOrEmpty(leave.Leave_Status))
            {
                leave.Leave_Status = "Active";
            }
            db.Leaves.Add(leave);
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
            return CreatedAtRoute("DefaultApi", new { id = leave.LeaveID }, leave);
        }

        // PUT api/leaves/5
        //public IHttpActionResult Put(int id, Leaf leave)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var existingLeave = db.Leaves.FirstOrDefault(l => l.LeaveID == id);
        //    if (existingLeave == null)
        //    {
        //        return NotFound();
        //    }

        //    existingLeave.LeaveType = leave.LeaveType;
        //    existingLeave.StartDate = leave.StartDate;
        //    existingLeave.EndDate = leave.EndDate;

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // DELETE api/leaves/5
        [HttpPost]
        [Route("api/Leave/SoftDelete/{id}")]
        public IHttpActionResult DeleteLeave(int id)
        {
            Leaf Leave = db.Leaves.Find(id);
            if (Leave == null)
            {
                return NotFound();
            }

            // Instead of actually deleting the record, set the status to 'Inactive'
            Leave.Leave_Status = "Inactive";
            db.Entry(Leave).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(Leave);
        }
    }
}

