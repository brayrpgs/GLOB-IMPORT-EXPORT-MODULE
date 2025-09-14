namespace api.Models
{
    public class UserProject
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? RolProject { get; set; }
        public decimal? Productivity { get; set; }
    }
}