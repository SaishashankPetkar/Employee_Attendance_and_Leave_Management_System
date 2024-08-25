using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ALMSystemWebApi.Models;

namespace ALMSystemWebApi.Controllers
{
    public class LeavesController : ApiController
    {
        private LeaveMasterEntities db = new LeaveMasterEntities();

        // GET: api/Leaves
        public IQueryable<Leave> GetLeaves()
        {
            return db.Leaves;
        }

        // GET: api/Leaves/5
        [ResponseType(typeof(Leave))]
        public IHttpActionResult GetLeave(int id)
        {
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return NotFound();
            }

            return Ok(leave);
        }

        // PUT: api/Leaves/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLeave(int id, Leave leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != leave.LeaveID)
            {
                return BadRequest();
            }

            db.Entry(leave).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Leaves
        [ResponseType(typeof(Leave))]
        public IHttpActionResult PostLeave(Leave leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                        Console.WriteLine("Property: {0}, Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = leave.LeaveID }, leave);
        }

        // DELETE: api/Leaves/5
        [ResponseType(typeof(Leave))]
        public IHttpActionResult DeleteLeave(int id)
        {
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return NotFound();
            }

            db.Leaves.Remove(leave);
            db.SaveChanges();

            return Ok(leave);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeaveExists(int id)
        {
            return db.Leaves.Count(e => e.LeaveID == id) > 0;
        }
    }
}
