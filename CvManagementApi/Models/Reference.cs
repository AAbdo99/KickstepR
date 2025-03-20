// Referanser relatert til en CV

public class Reference
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public required string ContactInfo { get; set; }

    public int CVId { get; set; }

    public required CV CV { get; set; }
    
}