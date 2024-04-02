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
        public DbSet<TeamBoard> TeamBoards { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Project> Projects { get; set; }

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
                .HasForeignKey<EstimatedAndSpentTime>(est => est.Id);

                issues.HasOne(issue => issue.IssueType)
                .WithMany(issueTypes => issueTypes.IssuesList)
                .HasForeignKey(issueType => issueType.IssueTypeId);

                issues.HasOne(issue => issue.Parent)
                .WithMany(parentIssue => parentIssue.ChildIssues)
                .HasForeignKey(issue => issue.ParentId);

                issues.HasOne(issue => issue.TeamBoard)
                .WithMany(teamBoard => teamBoard.IssuesList)
                .HasForeignKey(issue => issue.TeamBoardId);

            });


            modelBuilder.Entity<IssueRelease>(issueRelease =>
            {
                issueRelease.HasKey(ir => new { ir.IssueId, ir.ReleaseId });
                
                issueRelease.HasOne(ir => ir.Issue)
                .WithMany(issue => issue.FixVersions)
                .HasForeignKey(ir => ir.IssueId);

                issueRelease.HasOne(ir => ir.Release)
                .WithMany(release => release.IssueReleases)
                .HasForeignKey(ir => ir.ReleaseId);

            });

            modelBuilder.Entity<Project>(projects =>
            {
                projects.HasIndex(project => project.Key).IsUnique(true);
            });

        }
    }
}
