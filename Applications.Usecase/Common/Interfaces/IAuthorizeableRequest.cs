using MediatR;

namespace Applications.Usecase.Common.Interfaces;

public interface IAuthorizeableRequest<T> : IRequest<T>
{
}