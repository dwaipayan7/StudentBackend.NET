using System;
using System.Reflection.Metadata;

namespace StudentApp.DTO;

public class StudentDTO
{

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<string> Hobbies { get; set; } = new();  

}
