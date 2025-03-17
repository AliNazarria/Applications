using SharedKernel;

namespace Applications.Domain.Application.Events;

public record ApplicationAddEvent(Application app) : IDomainEvent;
public record ApplicationUpdateEvent(Application app) : IDomainEvent;
public record ApplicationDeleteEvent(Application app) : IDomainEvent;

public record ApplicationServiceAddEvent(ApplicationService service) : IDomainEvent;
public record ApplicationServiceUpdateEvent(ApplicationService service) : IDomainEvent;
public record ApplicationServiceDeleteEvent(ApplicationService service) : IDomainEvent;

