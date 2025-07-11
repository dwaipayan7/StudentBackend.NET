using System;

namespace StudentApp.Models;

public class StudentDatabaseSettings
{

    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string StudentCollection { get; set; } = string.Empty;

}
