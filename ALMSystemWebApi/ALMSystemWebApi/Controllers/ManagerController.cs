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
    public class ManagerController : ApiController
    {
        private LeaveMasterEntities2 db = new LeaveMasterEntities2();

        [HttpPost]
        [Route("api/Manager/Login")]
        public IHttpActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid model");

            var employee = db.Employees
                .FirstOrDefault(e => e.Email == model.Email && e.Password == model.Password && e.Emp_status == "Active" && e.RoleID == 2);

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
                    employee.Project.ProjectName
                });
            }

            return Unauthorized(); // Return 401 Unauthorized if credentials are invalid
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }


        [HttpGet]
        [Route("api/Manager/{id}")]
        public IEnumerable<Employee> GetEmployeesForManager(int id)
        {
            return db.Employees.Where(e => e.ManagerID==id).Include(e => e.Role).Include(e => e.Project);
        }


        // GET: api/Roles
        [HttpGet]
        [Route("api/Manager/Roles")]
        public IQueryable<Role> GetRoles()
        {
            return db.Roles;
        }

        // GET: api/Projects
        [HttpGet]
        [Route("api/Manager/Projects")]
        public IQueryable<Project> GetProjects()
        {
            return db.Projects.Where(p => p.Prj_status == "Active");
        }
    }
}
