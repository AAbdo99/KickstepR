public class Award
{
    public int Id { get; set; }
    public required  string Name { get; set; } 
    public required string Organization { get; set; }
    public int Year { get; set; }

    public int CVId { get; set; }
    public required CV CV { get; set; } 
}
