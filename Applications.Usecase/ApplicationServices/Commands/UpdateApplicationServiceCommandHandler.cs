using Applications.Usecase.Common.Interfaces;
using ErrorOr;
using MediatR;
using domain = Applications.Domain.Application;

namespace Applications.Usecase.ApplicationServices.Commands;

public class UpdateApplicationServiceCommandHandler(
    IGenericRepository<domain.Application, int> repository
    )
    : IRequestHandler<UpdateApplicationServiceCommand, ErrorOr<int>>
{
    Task<ErrorOr<int>> IRequestHandler<UpdateApplicationServiceCommand, ErrorOr<int>>.Handle(UpdateApplicationServiceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
