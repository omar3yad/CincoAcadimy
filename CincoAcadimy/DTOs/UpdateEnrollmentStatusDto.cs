﻿namespace CincoAcadimy.DTOs
{
    public class UpdateEnrollmentStatusDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public bool IsEnrolled { get; set; }
    }
}
