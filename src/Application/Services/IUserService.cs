using Infrastructure.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public interface IUserService
{
    Task<User> GetUserAsync(int id);
    Task<User?> GetUserByDiscordIdAsync(ulong discordId);
    Task<User> CreateUserAsync(ulong discordId, string name);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}
