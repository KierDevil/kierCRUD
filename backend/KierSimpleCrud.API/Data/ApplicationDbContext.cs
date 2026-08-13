using KierSimpleCrud.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KierSimpleCrud.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<StudentRecord> StudentRecords => Set<StudentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentRecord>(entity =>
        {
            entity.Property(record => record.Name).HasMaxLength(150).IsRequired();
            entity.Property(record => record.Email).HasMaxLength(200).IsRequired();
            entity.Property(record => record.Amount).HasPrecision(18, 2);
        });
    }
}
