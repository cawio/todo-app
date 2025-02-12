using Infrastructure.Entities;

namespace Presentation.Contracts.Responses;

public class TagResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public static TagResponse FromEntity(Tag entity)
    {
        return new TagResponse() { Id = entity.Id, Name = entity.Name, };
    }

    public static Tag ToEntity(TagResponse response)
    {
        return new Tag() { Id = response.Id, Name = response.Name, };
    }
}
