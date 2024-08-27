using System;
using System.Collections.Generic;
using System.Data;
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
    public class EmployeesController : ApiController
    {
        private LeaveMasterEntities2 db = new LeaveMasterEntities2();

        [HttpPost]
        [Route("api/Employees/Login")]
        public IHttpActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var employee = db.Employees
                .FirstOrDefault(e => e.Email == model.Email && e.Password == model.Password && e.Emp_status == "Active" && e.RoleID != 2);

            if (employee != null)
            {
                // Return employee details
                return Ok(new
                {
                    employee.EmployeeID,
                    employee.EmployeeName,
                    employee.Email,
                    employee.Phone,
                    employee.Role.RoleName,
                    employee.ManagerID,
                    employee.ProjectID,
                    employee.Project.ProjectName,
                    employee.LeaveBalance,
                    employee.No_of_leave
                });
            }

            return Unauthorized(); // Return 401 Unauthorized if credentials are invalid
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // GET: api/Employees
        public IEnumerable<Employee> GetEmployees()
        {
            return db.Employees.Include(e => e.Role).Include(e => e.Project);
        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // GET: api/Roles
        [HttpGet]
        [Route("api/Roles")]
        public IQueryable<Role> GetRoles()
        {
            return db.Roles;
        }

        // GET: api/Projects
        [HttpGet]
        [Route("api/Projects")]
        public IQueryable<Project> GetProjects()
        {
            return db.Projects.Where(p => p.Prj_status == "Active");
        }

        // PUT: api/Employees/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!employee.LeaveBalance.HasValue)
            {
                employee.LeaveBalance = 3;
            }
            if (!employee.No_of_leave.HasValue)
            {
                employee.No_of_leave = 0;
            }
            if (string.IsNullOrEmpty(employee.Emp_status))
            {
                employee.Emp_status = "Active";
            }
            db.Employees.Add(employee);
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

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employees/5
        [HttpPost]
        [Route("api/Employees/SoftDelete/{id}")]
        public async Task<IHttpActionResult> SoftDelete(int id)
        {
            var employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Emp_status = "inactive"; // Or whatever status indicates "soft delete"
            await db.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}

