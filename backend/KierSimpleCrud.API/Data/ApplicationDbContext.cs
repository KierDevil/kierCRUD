using KierSimpleCrud.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<SchoolYear> SchoolYears => Set<SchoolYear>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(student => student.Studid);
            entity.Property(student => student.Studid).HasMaxLength(30);
            entity.Property(student => student.StudentName).HasMaxLength(150).IsRequired();
            entity.Property(student => student.Status).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<SchoolYear>(entity =>
        {
            entity.HasKey(schoolYear => schoolYear.Sycode);
            entity.Property(schoolYear => schoolYear.Sycode).HasMaxLength(20);
            entity.Property(schoolYear => schoolYear.SchoolYearName).HasColumnName("SchoolYear").HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(course => course.Courscode);
            entity.Property(course => course.Courscode).HasMaxLength(20);
            entity.Property(course => course.CourseName).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(semester => semester.Semcode);
            entity.Property(semester => semester.Semcode).HasMaxLength(20);
            entity.Property(semester => semester.SemesterName).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(enrollment => enrollment.EnrollmentId);
            entity.Property(enrollment => enrollment.Status).HasMaxLength(30).IsRequired();
            entity.Property(enrollment => enrollment.EnrollmentDate).IsRequired();

            entity.HasIndex(enrollment => new { enrollment.Studid, enrollment.Sycode, enrollment.Semcode }).IsUnique();

            entity.HasOne(enrollment => enrollment.Student)
                .WithMany(student => student.Enrollments)
                .HasForeignKey(enrollment => enrollment.Studid)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(enrollment => enrollment.SchoolYear)
                .WithMany(schoolYear => schoolYear.Enrollments)
                .HasForeignKey(enrollment => enrollment.Sycode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(enrollment => enrollment.Course)
                .WithMany(course => course.Enrollments)
                .HasForeignKey(enrollment => enrollment.Courscode)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(enrollment => enrollment.Semester)
                .WithMany(semester => semester.Enrollments)
                .HasForeignKey(enrollment => enrollment.Semcode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SchoolYear>().HasData(
            new SchoolYear { Sycode = "SY2025", SchoolYearName = "2025-2026" },
            new SchoolYear { Sycode = "SY2026", SchoolYearName = "2026-2027" });

        modelBuilder.Entity<Course>().HasData(
            new Course { Courscode = "BSCS", CourseName = "Bachelor of Science in Computer Science" },
            new Course { Courscode = "BSIS", CourseName = "Bachelor of Science in Information Systems" },
            new Course { Courscode = "BLIS", CourseName = "Bachelor of Library and Information Science" });

        modelBuilder.Entity<Semester>().HasData(
            new Semester { Semcode = "1ST", SemesterName = "1st Semester" },
            new Semester { Semcode = "2ND", SemesterName = "2nd Semester" },
            new Semester { Semcode = "SUM", SemesterName = "Summer" });
    }
}
