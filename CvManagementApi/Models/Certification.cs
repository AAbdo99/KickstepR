using System;

public class Certification
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string IssuedBy { get; set; } 
        public DateTime Date { get; set; }

        public int CVId { get; set; }
        public required  CV CV { get; set; } 
    }