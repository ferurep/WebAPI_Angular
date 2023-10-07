using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetStudent()
        {

            var getStudentProfile = _studentContext.StudentProfiles;

            var StudentDTO =await getStudentProfile.Select(s => new StudentDTO
            {
                Id = s.Id,
                Surname = s.Name,
                Name = s.Name,
                MiddleName = s.Name,
                Age = s.Age
            }).ToListAsync();


            return Ok(StudentDTO);
        }


    }
}
