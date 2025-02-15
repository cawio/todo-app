using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application;

public class UserService(ApplicationDbContext dbContext) : IUserService
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<User> GetUserAsync(int id)
    {
        return await _dbContext.Users.FindAsync(id)
            ?? throw new KeyNotFoundException("User not found");
    }

    public async Task<User?> GetUserByDiscordIdAsync(ulong discordId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        user.UpdatedAt = DateTimeOffset.UtcNow;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user =
            await _dbContext.Users.FindAsync(id)
            ?? throw new KeyNotFoundException("User not found");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}
