// Arbeidserfaring relatert til en CV

public class Experience
{
    public int Id { get; set;}
    public required string JobTitle { get; set;}
    public required string Company { get; set;}
    public int CVId { get; set;}

    public required CV CV { get; set;}

}
    