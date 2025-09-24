namespace CincoAcadimy.DTOs
{
    public class ResourceDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string FileType { get; set; }
        public bool IsDownloadable { get; set; }
    }
}