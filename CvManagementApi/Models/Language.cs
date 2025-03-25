public class Language
{
    public int Id { get; set; }
     
    public required  string Name { get; set; }
    public required string Proficiency { get; set; }
    public int CVId { get; set; }
    public required CV CV { get; set; }
}
