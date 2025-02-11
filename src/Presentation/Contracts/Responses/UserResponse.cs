using System;
using Infrastructure.Entities;

namespace Presentation.Contracts.Responses;

public class UserResponse
{
    public int Id { get; set; }
    public string DiscordId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProfilePictureId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public static UserResponse FromEntity(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            DiscordId = user.DiscordId.ToString(),
            Name = user.Name,
            ProfilePictureId = user.ProfilePictureId,
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
            Name = Name,
            ProfilePictureId = ProfilePictureId,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}
