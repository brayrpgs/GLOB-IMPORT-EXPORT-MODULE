namespace api.Models
{
    public class UploadedBase64
    {
        public string? FileName { get; set; } = null;
        public string? Base64Content { get; set; } = null;
        public int? UserProject { get; set; } = null;
    }
}