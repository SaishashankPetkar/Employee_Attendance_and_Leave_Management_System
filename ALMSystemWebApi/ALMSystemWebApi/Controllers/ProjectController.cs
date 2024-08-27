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
    public class ProjectController : ApiController
    {
        private LeaveMasterEntities2 db = new LeaveMasterEntities2(); 

        // GET: api/Project
        // Fetches the list of all projects
        [HttpGet]
        public IQueryable<Project> GetProjects()
        {
            return db.Projects;
        }

        // GET: api/Project/5
        // Fetches a single project by ID
        [HttpGet]
        [ResponseType(typeof(Project))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // POST: api/Project
        // Creates a new project
        [HttpPost]
        [ResponseType(typeof(Project))]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if (String.IsNullOrEmpty(project.Prj_status))
            {
                project.Prj_status = "Active";
            }

            db.Projects.Add(project);
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
                return BadRequest("Validation errors occurred.");
            }

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectID }, project);
        }

        // PUT: api/Project/5
        // Updates an existing project by ID
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (String.IsNullOrEmpty(project.Prj_status))
            {
                project.Prj_status = "Active";
            }
            if (id != project.ProjectID)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

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

        // DELETE: api/Project/5
        // Deletes a project by ID
        [HttpPost]
        [Route("api/Project/SoftDelete/{id}")]
        public async Task<IHttpActionResult> SoftDelete(int id)
        {
            var project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            project.Prj_status = "inactive"; // Or whatever status indicates "soft delete"
            await db.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Project/UpdateMultiple
        // Update multiple projects (custom implementation required)
        [HttpPut]
        [Route("api/Project/UpdateMultiple")]
        public IHttpActionResult UpdateMultipleProjects([FromBody] Project[] projects)
        {
            if (projects == null || projects.Length == 0)
            {
                return BadRequest("No projects to update.");
            }

            foreach (var project in projects)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.Entry(project).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency issues
                return StatusCode(HttpStatusCode.Conflict);
            }

            return Ok();
        }

        // DELETE: api/Project/DeleteAll
        // Deletes all projects
        [HttpDelete]
        [Route("api/Project/DeleteAll")]
        public IHttpActionResult DeleteAllProjects()
        {
            var projects = db.Projects.ToList();
            if (!projects.Any())
            {
                return NotFound();
            }

            db.Projects.RemoveRange(projects);
            db.SaveChanges();

            return Ok("All projects have been deleted.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectID == id) > 0;
        }
    }
}

