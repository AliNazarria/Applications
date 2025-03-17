namespace Applications.Usecase.Common.Interfaces;

public interface IDateTimeProvider
{
    DateTime Now();
    DateTime Min();
    DateTime Max();
    DateTime UtcNow();
    int NowTimeStampInSecound();
    DateTime Get(int timestamp);
}