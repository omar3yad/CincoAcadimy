using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CincoAcadimy.DTOs
{
    public class CreatePaymentDto
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        public IFormFile Screenshot { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public decimal CoursePrice { get; set; }

        
 
        public string Status { get; set; } = "Pending";

    }
}