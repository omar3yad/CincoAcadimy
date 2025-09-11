namespace CincoAcadimy.DTOs
{
    public class AssessmentDto
    {
        public int Id { get; set; }            // ID للتقييم
        public string Title { get; set; }      // العنوان
        public string Description { get; set; }// وصف
        public int SessionId { get; set; }     // الجلسة المرتبطة
    }
}

