using Microsoft.EntityFrameworkCore;

class SchoolDb : DbContext
{
    public SchoolDb(DbContextOptions<SchoolDb> options) : base(options) {}

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
}