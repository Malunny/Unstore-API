namespace Unstore.DTO;

public record ServiceReadDto(int ClientId, int EmployeeId, decimal Cost, string Details, string Address);