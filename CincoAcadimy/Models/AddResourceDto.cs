namespace CincoAcadimy.Models
{
    
        public class AddResourceDto
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string FileType { get; set; }
            public int SessionId { get; set; }
            public bool IsDownloadable { get; set; } = false;
        }
    }
