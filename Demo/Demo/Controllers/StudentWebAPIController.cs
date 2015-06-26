
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Demo.DAL;
using Demo.Models;

namespace Demo.Controllers
{
    public class StudentWebAPIController : ApiController
    {
        private SchoolContext db = new SchoolContext();

        // GET: api/StudentWebAPI
        public IQueryable<Student> GetStudents()
        {
                       return db.Students;
        }

        // GET: api/StudentWebAPI/5
        [ResponseType(typeof(Student))]
        public HttpResponseMessage GetStudent(int id)
        {
            Student student = db.Students.Find(id);
            //Employee emp = EmployeeContext.Employees.Where(e => e.Id == id).FirstOrDefault();
            if (student != null)
            {
                return Request.CreateResponse<Student>(HttpStatusCode.OK, student);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Student Not Found");
            }
            /*            if (student == null)
                        {
                            return NotFound();
                        }

                        return Ok(student);*/
        }

        // PUT: api/StudentWebAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudent(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.ID)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/StudentWebAPI
        [ResponseType(typeof(Student))]
        public IHttpActionResult PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Students.Add(student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = student.ID }, student);
        }

        // DELETE: api/StudentWebAPI/5
        [ResponseType(typeof(Student))]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.Students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(int id)
        {
            return db.Students.Count(e => e.ID == id) > 0;
        }
    }
}