namespace Unstore.DTO;

public record EmployeeReadDto(string Name, string ContactNumber, string Email, string Position, DateTime StartedAt, bool Active);