using System;

namespace StudentApp.DTO;

public class RegisterDTO
{

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<string> Hobbies { get; set; } = null!;

}
