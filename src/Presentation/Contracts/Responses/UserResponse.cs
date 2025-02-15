using Infrastructure.Entities;

namespace Presentation.Contracts.Responses;

public class UserResponse
{
    public int Id { get; set; }
    public string DiscordId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Discriminator = string.Empty;
    public string? Avatar { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static UserResponse FromEntity(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            DiscordId = user.DiscordId.ToString(),
            Name = user.Name,
            Discriminator = user.Discriminator,
            Avatar = user.Avatar,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public User ToEntity()
    {
        return new User
        {
            Id = Id,
            DiscordId = ulong.Parse(DiscordId),
            Discriminator = Discriminator,
            Name = Name,
            Avatar = Avatar,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}
