using Microsoft.EntityFrameworkCore;

namespace WebApplication7.Models
{
    public class DatabaseContext :DbContext
    {
        public DbSet<SourceCredentials> SourceCredentials { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }   
        public DbSet<EstimatedAndSpentTime> EstimatedAndSpentTimes { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }    
        public DbSet<Parent> ParentIssues { get; set; }

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

                issues.HasOne(issue => issue.IssueType)
                .WithMany(issueTypes => issueTypes.IssuesList)
                .HasForeignKey(issueType => issueType.IssueTypeId);

                issues.HasOne(issue => issue.Parent)
                .WithMany(parentIssue => parentIssue.ChildIssues)
                .HasForeignKey(issue => issue.ParentId);
            });

            modelBuilder.Entity<IssueType>(issueTypes =>
            {
                issueTypes.HasIndex(it => it.Id).IsUnique(true);
            });

            modelBuilder.Entity<Parent>(parentIssue =>
            {
                parentIssue.HasIndex(it => it.ParentId).IsUnique(true);
            });
        }
    }
}
