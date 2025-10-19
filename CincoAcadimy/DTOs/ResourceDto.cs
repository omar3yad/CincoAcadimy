namespace CincoAcadimy.DTOs
{
    public class ResourceDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public int SessionId { get; set; }
        public bool IsDownloadable { get; set; }
    }
}