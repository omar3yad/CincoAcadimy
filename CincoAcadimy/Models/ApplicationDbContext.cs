using CincoAcadimy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CincoAcadimy.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<HR> HRs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentSession> StudentSessions { get; set; }
        public DbSet<StudentAssessment> StudentAssessments { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =====================
            // Student ↔ ApplicationUser (1:1)
            // =====================
            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Instructor ↔ ApplicationUser (1:1)
            // =====================
            builder.Entity<Instructor>()
                .HasOne(i => i.User)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // HR ↔ ApplicationUser (1:1)
            // =====================
            builder.Entity<HR>()
                .HasOne(h => h.User)
                .WithOne(u => u.HR)
                .HasForeignKey<HR>(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Admin ↔ ApplicationUser (1:1)
            // =====================
            builder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Instructor ↔ Courses (1:M)
            // =====================
            builder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================
            // Student ↔ Courses (M:M) via StudentCourse
            // =====================
            builder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            // =====================
            // Course ↔ Sessions (1:M)
            // =====================
            builder.Entity<Session>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Session ↔ Assessments (1:M)
            // =====================
            builder.Entity<Assessment>()
                .HasOne(a => a.Session)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Session ↔ Attendance (1:M)
            // =====================
            builder.Entity<Attendance>()
                .HasOne(at => at.Session)
                .WithMany(s => s.Attendances)
                .HasForeignKey(at => at.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Student ↔ Attendance (1:M)
            // =====================
            builder.Entity<Attendance>()
                .HasOne(at => at.Student)
                .WithMany(st => st.Attendances)
                .HasForeignKey(at => at.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Assessment ↔ Session (1:M)
            // =====================
            builder.Entity<Assessment>()
                .HasOne(a => a.Session)
                .WithMany(s => s.Assessments)
                .HasForeignKey(a => a.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // StudentAssessment ↔ Student (M:1)
            // =====================
            builder.Entity<StudentAssessment>()
                .HasOne(sa => sa.Student)
                .WithMany(st => st.StudentAssessments)
                .HasForeignKey(sa => sa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // StudentAssessment ↔ Assessment (M:1)
            // =====================
            builder.Entity<StudentAssessment>()
                .HasOne(sa => sa.Assessment)
                .WithMany(a => a.StudentAssessments)
                .HasForeignKey(sa => sa.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================
            // Composite Key (StudentId + AssessmentId)
            // =====================
            builder.Entity<StudentAssessment>()
                .HasKey(sa => new { sa.StudentId, sa.AssessmentId });
            // تكوين العلاقة Many-to-Many بين Student و Session
            builder.Entity<StudentSession>()
                .HasKey(ss => ss.Id); // لو عايز Id كـ PK
                                      // لو تحب تستخدم composite key بدل Id:
                                      // .HasKey(ss => new { ss.StudentId, ss.SessionId });

            builder.Entity<StudentSession>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSessions)
                .HasForeignKey(ss => ss.StudentId);


            builder.Entity<Resource>()
       .HasOne(r => r.Session)
       .WithMany(s => s.Resources)
       .HasForeignKey(r => r.SessionId)
       .OnDelete(DeleteBehavior.Cascade);
        }
    
    }
}