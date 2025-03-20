// Ferdigheter relater til en CV

public class Skill
{
    public int Id { get; set;}
    public required string Name { get; set;}

    public required string Description { get; set;}

    public int CVId { get; set;}
    public required CV CV { get; set;}
    
}