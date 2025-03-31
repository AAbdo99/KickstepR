using System;

public class Position
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Organization { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CVId { get; set; } 
    public required CV CV { get; set; } 
}
