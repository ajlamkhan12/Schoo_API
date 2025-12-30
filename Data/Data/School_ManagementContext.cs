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
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
    }
}
