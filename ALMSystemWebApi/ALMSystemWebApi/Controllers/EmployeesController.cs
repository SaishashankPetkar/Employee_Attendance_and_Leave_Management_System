using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
<<<<<<< HEAD
using System.Linq;
using System.Net;
using System.Net.Http;
=======
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
using System.Web.Http;
using System.Web.Http.Description;
using ALMSystemWebApi.Models;

namespace ALMSystemWebApi.Controllers
{
    public class EmployeesController : ApiController
    {
<<<<<<< HEAD
        private LeaveMasterEntities db = new LeaveMasterEntities();

        // GET: api/Employees
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
=======
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
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
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

<<<<<<< HEAD
        // PUT: api/Employees/5
=======
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
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
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
<<<<<<< HEAD
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
=======
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        Console.WriteLine("Property: { 0}, Error: { 1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
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
<<<<<<< HEAD

            db.Employees.Add(employee);
            db.SaveChanges();
=======
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
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employees/5
<<<<<<< HEAD
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
=======
        [HttpPost]
        [Route("api/Employees/SoftDelete/{id}")]
        public async Task<IHttpActionResult> SoftDelete(int id)
        {
            var employee = await db.Employees.FindAsync(id);
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
            if (employee == null)
            {
                return NotFound();
            }

<<<<<<< HEAD
            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
=======
            employee.Emp_status = "inactive"; // Or whatever status indicates "soft delete"
            await db.SaveChangesAsync();

            return Ok();
>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
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
<<<<<<< HEAD
}
=======
}

>>>>>>> 7f696cdbb8726d085feec7d422b8c0b4898de8d0
