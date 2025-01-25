using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;

namespace Cuisine.Infrastructure.Repositories;

public class UserRepository(UserDbContext context): IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await context.Users.FindAsync(id);
            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
