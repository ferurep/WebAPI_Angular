using System;
using System.Collections.Generic;

namespace WebAPI_Angular.Models;

public partial class StudentProfile
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? MiddleName { get; set; }

    public int? Age { get; set; }

    public int? SubjectId { get; set; }
}
