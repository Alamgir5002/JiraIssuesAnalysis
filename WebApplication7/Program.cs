using Microsoft.EntityFrameworkCore;
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
  
builder.Services.AddScoped<SourceCredentialsRepository>();
builder.Services.AddScoped<CustomFieldRepository>();
builder.Services.AddScoped<IssueRepository>();
builder.Services.AddScoped<ProjectRepository>();

string connectionString = builder.Configuration.GetConnectionString("SQLConnectionString") ?? throw new InvalidOperationException("Connection string of name SQLConnectionString not found");
builder.Services.AddDbContext<DatabaseContext>(conn => conn.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
