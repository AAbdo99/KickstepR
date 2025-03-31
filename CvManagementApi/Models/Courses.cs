using System;

public class Course
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Provider { get; set; } 
    public DateTime Date { get; set; }
    public int CVId { get; set; }

    public required CV CV { get; set; }
}