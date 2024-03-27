using Microsoft.EntityFrameworkCore;
using Quartz;
using WebApplication7.Models;
using WebApplication7.Quartz.Jobs;
using WebApplication7.Repository;
using WebApplication7.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SourceService>();
builder.Services.AddScoped<IssuesService>();
builder.Services.AddScoped<CustomFieldsService>();
builder.Services.AddScoped<IssueMapperService>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<SourceCredentialsRepository>();
builder.Services.AddScoped<CustomFieldRepository>();

// Schedule Quartz Jobs
builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(opts => opts.WaitForJobsToComplete = true);

// Configure custom logging
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
//builder.Logging.AddDebug();

string connectionString = builder.Configuration.GetConnectionString("SQLConnectionString") ?? throw new InvalidOperationException("Connection string of name SQLConnectionString not found");
builder.Services.AddDbContext<DatabaseContext>(conn => conn.UseSqlServer(connectionString));

var app = builder.Build();

//// Schedule Quartz Jobs
var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

// create a job and tie it to our JiraIssuesSyncJob class
var jiraSyncJob = JobBuilder.Create<JiraIssuesSyncJob>()
    .WithIdentity("JiraIssuesSyncJob", "JiraGroup")
    .Build();

// trigger the job to run now, and then every 40 seconds
var trigger = TriggerBuilder.Create()
    .WithIdentity("JiraIssuesSyncJobTrigger", "JiraGroup")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(30)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(jiraSyncJob, trigger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
