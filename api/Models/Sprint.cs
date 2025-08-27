namespace api.Models
{
    public class Sprint
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateInit { get; set; }
        public DateTime DateEnd { get; set; }
    }
}