using Hangfire;
using IssueAnalysisExtended.Repository;
using IssueAnalysisExtended.Repository.Interfaces;
using IssueAnalysisExtended.Services.SyncService;
using IssueAnalysisExtended.Services.SyncService.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication7.Models;
using WebApplication7.Repository;
using WebApplication7.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<SourceService>();
builder.Services.AddScoped<IssuesService>();
builder.Services.AddScoped<CustomFieldsService>();
builder.Services.AddScoped<IssueMapperService>();
builder.Services.AddScoped<HttpClientService>();
  
builder.Services.AddScoped<ISourceCredentialsRepository, SourceCredentialsRepository>();
builder.Services.AddScoped<ICustomFieldRepository, CustomFieldRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IReleaseRepository, ReleasesRespository>();
builder.Services.AddScoped<IParentRepository, ParentRepository>();
builder.Services.AddScoped<ITeamboardRepository, TeamboardRepository>();
builder.Services.AddScoped<IIssueTypeRepository, IssueTypeRepository>();
builder.Services.AddScoped<IIssueEstimatedAndSpentTimeRepository, IssueEstimatedAndSpentTimeRepository>();
builder.Services.AddScoped<SyncedReleasesRespository>();

builder.Services.AddScoped<JiraSyncService>();
builder.Services.AddScoped<JiraIssuesSyncServiceJob>();

// Configure Hangfire
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

// Configure custom logging
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
//builder.Logging.AddDebug();

string connectionString = builder.Configuration.GetConnectionString("SQLConnectionString") ?? throw new InvalidOperationException("Connection string of name SQLConnectionString not found");
builder.Services.AddDbContext<DatabaseContext>(conn => conn.UseSqlServer(connectionString));

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

// Create a recurring job to fetch data against a particular release
using var scope = app.Services.CreateScope();
var releaseIssuesSyncServiceJob = scope.ServiceProvider.GetRequiredService<JiraIssuesSyncServiceJob>();
//BackgroundJob.Enqueue(() => releaseIssuesSyncServiceJob.Execute());
//RecurringJob.AddOrUpdate("ReleaseIssuesSyncJob", () => releaseIssuesSyncServiceJob.Execute(), Cron.HourInterval(2)); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
