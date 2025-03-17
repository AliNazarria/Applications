namespace Applications.Usecase.Application.Interfaces;

public interface IApplicationRepository
{
    Task<bool> IsUnique(int id, string key);
}
