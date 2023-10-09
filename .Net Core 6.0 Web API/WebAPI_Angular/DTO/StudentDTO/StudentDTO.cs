namespace WebAPI_Angular.DTO.StudentDTO
{

    public  class LoginDTO
    {

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }


    public class StudentDTO
    {
        public int Id { get; set; }

        public string? Surname { get; set; }

        public string? Name { get; set; }

        public string? MiddleName { get; set; }

        public int? Age { get; set; }
        public int? SubjectId { get; set; }

        public List<SubjectDTO> subjects { get; set; }

    }

    public class StudentPostDTO
    {

        public string? Surname { get; set; }

        public string? Name { get; set; }

        public string? MiddleName { get; set; }

        public int? Age { get; set; }

        public int? SubjectId { get; set; }

    }

    public class SubjectDTO
    {
        public string? subjectName { get; set; }
    }

}
