namespace Unstore.DTO;

public record ToolReadDto(string Name, string Description, IEnumerable<int> toolTagsIds);