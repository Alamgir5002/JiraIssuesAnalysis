﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication7.Models;

#nullable disable

namespace WebApplication7.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240328111715_updated-column")]
    partial class updatedcolumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication7.Models.CustomField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomFieldKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomFieldValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomFieldKey")
                        .IsUnique();

                    b.ToTable("CustomFields");
                });

            modelBuilder.Entity("WebApplication7.Models.EstimatedAndSpentTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AggregateTimeEstimate")
                        .HasColumnType("float");

                    b.Property<int>("AggregatedTimeEstimateInDays")
                        .HasColumnType("int");

                    b.Property<double>("AggregatedTimeSpent")
                        .HasColumnType("float");

                    b.Property<int>("AggregatedTimeSpentInDays")
                        .HasColumnType("int");

                    b.Property<string>("IssueId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IssueId")
                        .IsUnique();

                    b.ToTable("EstimatedAndSpentTimes");
                });

            modelBuilder.Entity("WebApplication7.Models.Issue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssueTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IssueUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ProductivityRatio")
                        .HasColumnType("float");

                    b.Property<string>("ResolvedDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoryPoints")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamBoardId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IssueTypeId");

                    b.HasIndex("ParentId");

                    b.HasIndex("TeamBoardId");

                    b.ToTable("Issues");
                });

            modelBuilder.Entity("WebApplication7.Models.IssueRelease", b =>
                {
                    b.Property<string>("IssueId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReleaseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("IssueId", "ReleaseId");

                    b.HasIndex("ReleaseId");

                    b.ToTable("IssueRelease");
                });

            modelBuilder.Entity("WebApplication7.Models.IssueType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SubTask")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("IssueTypes");
                });

            modelBuilder.Entity("WebApplication7.Models.Parent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParentUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParentIssues");
                });

            modelBuilder.Entity("WebApplication7.Models.Release", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Released")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("WebApplication7.Models.SourceCredentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("SourceAuthToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceUserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SourceUserEmail")
                        .IsUnique();

                    b.ToTable("SourceCredentials");
                });

            modelBuilder.Entity("WebApplication7.Models.TeamBoard", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TeamBoards");
                });

            modelBuilder.Entity("WebApplication7.Models.EstimatedAndSpentTime", b =>
                {
                    b.HasOne("WebApplication7.Models.Issue", "Issue")
                        .WithOne("IssueEstimatedAndSpentTime")
                        .HasForeignKey("WebApplication7.Models.EstimatedAndSpentTime", "IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("WebApplication7.Models.Issue", b =>
                {
                    b.HasOne("WebApplication7.Models.IssueType", "IssueType")
                        .WithMany("IssuesList")
                        .HasForeignKey("IssueTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication7.Models.Parent", "Parent")
                        .WithMany("ChildIssues")
                        .HasForeignKey("ParentId");

                    b.HasOne("WebApplication7.Models.TeamBoard", "TeamBoard")
                        .WithMany("IssuesList")
                        .HasForeignKey("TeamBoardId");

                    b.Navigation("IssueType");

                    b.Navigation("Parent");

                    b.Navigation("TeamBoard");
                });

            modelBuilder.Entity("WebApplication7.Models.IssueRelease", b =>
                {
                    b.HasOne("WebApplication7.Models.Issue", "Issue")
                        .WithMany("FixVersions")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication7.Models.Release", "Release")
                        .WithMany("IssueReleases")
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");

                    b.Navigation("Release");
                });

            modelBuilder.Entity("WebApplication7.Models.Issue", b =>
                {
                    b.Navigation("FixVersions");

                    b.Navigation("IssueEstimatedAndSpentTime")
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication7.Models.IssueType", b =>
                {
                    b.Navigation("IssuesList");
                });

            modelBuilder.Entity("WebApplication7.Models.Parent", b =>
                {
                    b.Navigation("ChildIssues");
                });

            modelBuilder.Entity("WebApplication7.Models.Release", b =>
                {
                    b.Navigation("IssueReleases");
                });

            modelBuilder.Entity("WebApplication7.Models.TeamBoard", b =>
                {
                    b.Navigation("IssuesList");
                });
#pragma warning restore 612, 618
        }
    }
}
