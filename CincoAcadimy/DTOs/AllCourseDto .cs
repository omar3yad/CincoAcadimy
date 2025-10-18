namespace CincoAcadimy.DTOs
{
    public class AllCourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }

        // إضافات أساسية للعرض
        public string ImageUrl { get; set; }
        public string Duration { get; set; }

        public decimal Price { get; set; }

        // مرتبطة بالطالب
        public bool IsEnrolled { get; set; }
        public double Progress { get; set; } // نسبة مئوية
        public bool IsCompleted { get; set; }

        // إضافات اختيارية

    }
}
