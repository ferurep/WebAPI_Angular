using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Angular.DTO.StudentDTO;


namespace WebAPI_Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentContext _studentContext;
        public StudentController(StudentContext studentContext)
        {
           _studentContext =  studentContext;

        }

        [HttpGet("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {

            var studentDTOs = await _studentContext.StudentProfiles
                 .Join(
                     _studentContext.Subjects,
                     student => student.SubjectId,
                     subject => subject.Id,
                     (student, subject) => new StudentDTO
                     {
                         Id = student.Id,
                         Surname = student.Surname,
                         Name = student.Name,
                         MiddleName = student.MiddleName,
                         Age = student.Age,
                         SubjectId = student.SubjectId,
                         subjects = new List<SubjectDTO>
                         {
                                 new SubjectDTO
                            {
                                subjectName = subject.SubjectName
                            }
                         }
                     }
                 )
                 .ToListAsync();

            return Ok(studentDTOs);
        }

        [HttpGet("SearchStudent/{id}")]
        public async Task<IActionResult> SearchStudent(int id)
        {

            var StudentDTO = await _studentContext.StudentProfiles.Where(s => s.Id == id)
           .Join(
               _studentContext.Subjects,
               student => student.SubjectId,
               subject => subject.Id,
               (student, subject) => new StudentDTO
               {
                   Id = student.Id,
                   Surname = student.Surname,
                   Name = student.Name,
                   MiddleName = student.MiddleName,
                   Age = student.Age,
                   SubjectId = student.SubjectId,
                   subjects = new List<SubjectDTO>
                   {
                                 new SubjectDTO
                            {
                                subjectName = subject.SubjectName
                            }
                   }
               }
           )
           .ToListAsync();

            if (StudentDTO.Count > 0 )
            {
                return Ok(StudentDTO);
            }
            return NotFound("Studen Not Found");
        }


        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(StudentPostDTO StudentPost)
        {
            if (StudentPost is null)
            {
                return BadRequest("No data Input");
            }

            var addStudent = new StudentProfile
            {
                Surname = StudentPost.Surname,
                Name = StudentPost.Name,
                MiddleName = StudentPost.MiddleName,
                Age = StudentPost.Age,
                SubjectId = StudentPost.SubjectId

            };


            try
            {
                _studentContext.StudentProfiles.Add(addStudent);
                _studentContext.SaveChanges();

                return Ok("Student Has been Created");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }


        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(int id, StudentPostDTO studentEdit)
        {
            var getStudentProfile = _studentContext.StudentProfiles.SingleOrDefault(student => student.Id == id);

            if (getStudentProfile is null)
            {
                return BadRequest("No record has been retrieve");
            }



            try
            {
                getStudentProfile.Surname = studentEdit.Surname;
                getStudentProfile.Name = studentEdit.Name;
                getStudentProfile.MiddleName = studentEdit.MiddleName;
                getStudentProfile.Age = studentEdit.Age;
                getStudentProfile.SubjectId = studentEdit.SubjectId;

                // Mark the entity as modified and save changes to the database
                _studentContext.Update(getStudentProfile);
                await _studentContext.SaveChangesAsync();

                return Ok("Student Has been Updated");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }




        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var getStudentProfile = _studentContext.StudentProfiles.SingleOrDefault(student => student.Id == id);

            if (id == 0 || getStudentProfile is null)
            {
                return BadRequest("No record has been deleted");
            }



            try
            {

                _studentContext.StudentProfiles.Remove(getStudentProfile);
                await _studentContext.SaveChangesAsync();

                return Ok("Student Has been Deleted");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

    }
}
