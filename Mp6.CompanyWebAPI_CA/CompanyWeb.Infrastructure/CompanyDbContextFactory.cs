using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LMS.Infrastructure
{
    public class CompanyDbContextFactory : IDesignTimeDbContextFactory<CompanyDbContext>
    {
        public CompanyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseNpgsql("Server=localhost;Database=lms_asg5;Username=postgres;Password=12345");

            return new CompanyDbContext(optionsBuilder.Options);
        }
    }
}
