using LMS.Core.Models;
using LMS.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure library options and infrastructure
var libraryConfig = builder.Configuration.GetSection(LibraryOptions.SettingName);
builder.Services.Configure<LibraryOptions>(libraryConfig);
builder.Services.ConfigureInfrastructure(builder.Configuration);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically (if using for development)
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<LMSDbContext>();
    context.Database.Migrate(); // Apply any pending migrations
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
