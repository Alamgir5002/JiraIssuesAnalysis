using Microsoft.EntityFrameworkCore;

namespace WebApplication7.Models
{
    public class DatabaseContext :DbContext
    {
        public DbSet<SourceCredentials> SourceCredentials { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }   
        public DbSet<EstimatedAndSpentTime> EstimatedAndSpentTimes { get; set; }
        public DbSet<Issue> Issues { get; set; }

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

            modelBuilder.Entity<Issue>(issues =>
            {
                issues.HasKey(issue => issue.Id);
                issues.HasOne(issue => issue.IssueEstimatedAndSpentTime)
                .WithOne(est => est.Issue)
                .HasForeignKey<EstimatedAndSpentTime>(est => est.IssueId);
            });
        }
    }
}
