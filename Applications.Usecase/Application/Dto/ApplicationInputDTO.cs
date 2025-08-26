namespace Applications.Usecase.Application.Dto;

public record ApplicationInputDTO(
    string Key,
    string Title,
    bool Active,
    string? Description = null,
    string? LogoAddress = null);
