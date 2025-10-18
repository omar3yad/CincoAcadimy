using System;
using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string ScreenshotPath { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; }
    }
}