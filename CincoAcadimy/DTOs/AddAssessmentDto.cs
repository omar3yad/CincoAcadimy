namespace CincoAcadimy.DTOs
{
    public class AddAssessmentDto
    {
        public string Title { get; set; }          // عنوان التقييم (مثلاً Final Exam, Quiz 1)
        public string Description { get; set; }   // وصف للتقييم (اختياري)
        public int SessionId { get; set; }        // الجلسة المرتبط بيها التقييم
    }
}
