using SharedKernel;

namespace Applications.Domain.Service.Events;

public record ServiceAddEvent(Service Service) : IDomainEvent;
public record ServiceUpdateEvent(Service Service) : IDomainEvent;
public record ServiceDeleteEvent(Service Service) : IDomainEvent;
