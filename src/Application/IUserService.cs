using Infrastructure.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application;

public interface IUserService
{
    Task<User> GetUserAsync(int id);
    Task<User?> GetUserByDiscordIdAsync(ulong discordId);
    Task<User> CreateUserAsync(ulong discordId, string name, string? pictureId);
    Task<User> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}
