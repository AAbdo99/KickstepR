// Arbeidserfaring relater til en CV

public class Education
{
    public int Id { get; set; }
    public required string Institution { get; set; }
    public required string Degree { get; set; }
    public int CVId { get; set; }

    public required CV CV { get; set; }
}