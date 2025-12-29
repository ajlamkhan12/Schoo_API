using DbModels;
using DbModels.Students;
using Microsoft.EntityFrameworkCore;
namespace Data.Data
{
    public class School_ManagementContext: DbContext
    {
        public School_ManagementContext(DbContextOptions<School_ManagementContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
