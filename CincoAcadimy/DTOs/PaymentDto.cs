using System;

namespace CincoAcadimy.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string ScreenshotPath { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public decimal CoursePrice { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}