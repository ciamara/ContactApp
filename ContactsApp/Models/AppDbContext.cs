using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CategoryDictionary> Dictionaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // unique e-mail address
            modelBuilder.Entity<Contact>().HasIndex(c => c.Email).IsUnique();
        }
    }
}
