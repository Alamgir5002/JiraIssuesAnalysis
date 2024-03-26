using Microsoft.EntityFrameworkCore;

namespace WebApplication7.Models
{
    public class DatabaseContext :DbContext
    {
        public DbSet<SourceCredentials> SourceCredentials { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }   

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SourceCredentials>(sourceCredentials =>
            {
                sourceCredentials.HasIndex(source => source.SourceUserEmail).IsUnique(true);
            });

            modelBuilder.Entity<CustomField>(customFields =>
            {
                customFields.HasIndex(cf => cf.CustomFieldKey).IsUnique(true);
            });
        }
    }
}
