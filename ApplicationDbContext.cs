using System;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApp.Service
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.userid);
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.email).IsRequired();
                entity.Property(e => e.password).IsRequired();
                entity.Property(e => e.salt).IsRequired();
                entity.Property(e => e.user_type).IsRequired();
                entity.Property(e => e.date_created).HasDefaultValue(DateTime.UtcNow);
            });
        }
    }

    public class User
    {
        public int userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public int user_type { get; set; }
        public DateTime date_created { get; set; } = DateTime.UtcNow;
        public string? jwt_token {  get; set; }
    }
}
