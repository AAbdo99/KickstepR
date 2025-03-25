    public class Presentation
    {
        public int Id { get; set; }
        public required string Title { get; set; } 
        public required string Event { get; set; }
        public DateTime Date { get; set; }

        public int CVId { get; set; } 
        public required  CV CV { get; set; }
    }