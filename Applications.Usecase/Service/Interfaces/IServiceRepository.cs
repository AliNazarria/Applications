namespace Applications.Usecase.Service.Interfaces;

public interface IServiceRepository
{
    Task<bool> IsUnique(int id, string key);
}