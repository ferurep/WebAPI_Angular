using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Angular.DTO.StudentDTO;
using WebAPI_Angular.Models;


namespace WebAPI_Angular.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentContext _studentContext;
        private readonly IConfiguration _configuration;
        public StudentController(StudentContext studentContext , IConfiguration configuration)
        {
           _studentContext =  studentContext;
            _configuration = configuration;

        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(LoginDTO request)
        {
            if (request is null)
            {
                return BadRequest("Empty");
            }


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new Login
            {
                UserName = request.UserName,
                Password = passwordHash
            };

            _studentContext.Logins.Add(newUser);
            _studentContext.SaveChanges();

            return Ok(newUser);
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            var findUser = _studentContext.Logins.SingleOrDefault(x => x.UserName == request.UserName);

            if (findUser is null)
            {
                return BadRequest("User Not Found");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password , findUser.Password))
            {
                return BadRequest("Wrong password or Username");
            }

            string token = CreateToken(request);

            return Ok(token);
        }

        //Generate Token
        private string CreateToken(LoginDTO user)
        {
            List<Claim> claims  = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(
                _configuration.GetSection("JwtSettings:Token").Value!
                ));
                
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );  

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
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
                 ).OrderBy(studentDTO => studentDTO.Id)
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
