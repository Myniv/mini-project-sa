using System.Text;
using LibraryManagementSystem.Application.Service;
using LibraryManagementSystem.Domain.Models.Entities;
using LibraryManagementSystem.Domain.Service;
using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureInfrastructure(builder.Configuration);

// builder.Services.ConfigureIdentity();

builder.Services.AddAuthentication();

builder.Services.AddCookiePolicy(options =>
{
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Vite dev server
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // Penting jika menggunakan cookies/credentials
        });
});

// builder.Services.AddAuthorization();


var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var dataContext = serviceScope.ServiceProvider.GetService<LMSDbContext>();
dataContext?.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowViteApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
