using System;
using System.Collections.Generic;

namespace WebAPI_Angular.Models;

public partial class Login
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
}
