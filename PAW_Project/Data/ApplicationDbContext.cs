using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PAW_Project.Models;

namespace PAW_Project.Data
{
    // 1. THE CRITICAL FIX: We changed DbContext to IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Professor> Professors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Studies> CourseStudies { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 2. CRITICAL: This base call tells Entity Framework to build the AspNetUsers tables
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Studies>(entity =>
            {
                entity.HasKey(s => new { s.SCNP, s.CourseID });

                entity.HasOne(s => s.Student)
                      .WithMany(st => st.EnrolledCourses)
                      .HasForeignKey(s => s.SCNP);

                entity.HasOne(s => s.Course)
                      .WithMany(c => c.StudentEnrollments)
                      .HasForeignKey(s => s.CourseID);

                entity.HasData(
                    new Studies { SCNP = "S001", CourseID = 1, Grade = 10 }
                );
            });

            modelBuilder.Entity<Professor>()
                .HasMany(p => p.Courses)
                .WithMany(c => c.Professors)
                .UsingEntity<Dictionary<string, object>>(
                    "Teaches",
                    j => j.HasOne<Course>().WithMany().HasForeignKey("CoursesCourseID"),
                    j => j.HasOne<Professor>().WithMany().HasForeignKey("ProfessorsPCNP"),
                    j =>
                    {
                        j.HasKey("CoursesCourseID", "ProfessorsPCNP");
                        j.HasData(
                            new { CoursesCourseID = 1, ProfessorsPCNP = "P101" },
                            new { CoursesCourseID = 2, ProfessorsPCNP = "P102" },
                            new { CoursesCourseID = 3, ProfessorsPCNP = "P103" }
                        );
                    });

            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.Rooms)
                .WithMany(r => r.Schedules)
                .UsingEntity<Dictionary<string, object>>(
                    "isAt",
                    j => j.HasOne<Room>().WithMany().HasForeignKey("RoomsRoomID"),
                    j => j.HasOne<Schedule>().WithMany().HasForeignKey("SchedulesScheduleID"),
                    j =>
                    {
                        j.HasKey("RoomsRoomID", "SchedulesScheduleID");
                        j.HasData(
                            new { RoomsRoomID = 1, SchedulesScheduleID = 1 },
                            new { RoomsRoomID = 2, SchedulesScheduleID = 2 },
                            new { RoomsRoomID = 3, SchedulesScheduleID = 3 }
                        );
                    });

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomID = 1, Building = "K206" },
                new Room { RoomID = 2, Building = "G116" },
                new Room { RoomID = 3, Building = "K201" }
            );

            modelBuilder.Entity<Professor>().HasData(
                new Professor { PCNP = "P101", FirstName = "Elvira", LastName = "Popescu" },
                new Professor { PCNP = "P102", FirstName = "Anca", LastName = "Udristoiu" },
                new Professor { PCNP = "P103", FirstName = "Catalina", LastName = "Mancas" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = 1, Name = "Proiectarea Aplicatiilor Web", Year = 2026, Semester = 2 },
                new Course { CourseID = 2, Name = "Baze de Date", Year = 2026, Semester = 1 },
                new Course { CourseID = 3, Name = "Retele de Calculatoare", Year = 2026, Semester = 1 },
                new Course { CourseID = 4, Name = "Inteligenta Artificiala", Year = 2026, Semester = 2 }
            );

            modelBuilder.Entity<Schedule>().HasData(
                new Schedule
                {
                    ScheduleID = 1,
                    CourseID = 1,
                    Date = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    Hour = "10:00"
                },
                new Schedule
                {
                    ScheduleID = 2,
                    CourseID = 2,
                    Date = new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc),
                    Hour = "12:00"
                },
                new Schedule
                {
                    ScheduleID = 3,
                    CourseID = 3,
                    Date = new DateTime(2026, 10, 3, 0, 0, 0, DateTimeKind.Utc),
                    Hour = "14:00"
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { SCNP = "S001", FirstName = "Mohsen", LastName = "Darwish", Year = 2, Type = "Regular" }
            );

            modelBuilder.Entity<Professor>().HasData(
                new Professor { PCNP = "P104", FirstName = "Marius", LastName = "Iancu" }
            );

            modelBuilder.Entity("Teaches").HasData(
                new { CoursesCourseID = 4, ProfessorsPCNP = "P104" }
            );

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TargetStudent)
                .WithMany()
                .HasForeignKey(c => c.TargetStudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Announcement)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AnnouncementID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Assignment)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AssignmentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}