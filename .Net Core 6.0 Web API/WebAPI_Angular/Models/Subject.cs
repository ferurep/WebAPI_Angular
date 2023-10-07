using System;
using System.Collections.Generic;

namespace WebAPI_Angular.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string? SubjectName { get; set; }

    public int? StudentId { get; set; }
}
