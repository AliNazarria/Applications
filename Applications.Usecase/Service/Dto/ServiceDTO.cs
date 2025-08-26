namespace Applications.Usecase.Service.Dto;

public record ServiceDTO(
    string Key,
    string Name
) : BaseDTO<int>();
