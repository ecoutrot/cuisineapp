using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cuisine.Infrastructure.Persistence.Data
{
    public class ContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            
            optionsBuilder.UseSqlServer("Server=localhost;Database=Cuisine;User Id=sa;Password=Password123!;");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
