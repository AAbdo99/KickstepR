    public class RoleOverview
    {
        public int Id { get; set; }
        public required string Role { get; set; }
        public required string Description { get; set; }

        public int CVId { get; set; }
        public required CV CV { get; set; }
    }