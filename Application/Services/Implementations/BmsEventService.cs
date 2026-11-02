using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;

namespace bms_manager.Application.Services.Implementations;

public sealed class BmsEventService : IBmsEventService
{
    private readonly IBmsEventRepository _repository;

    public BmsEventService(IBmsEventRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<BmsEvent> GetAllEvents()
    {
        return _repository.GetAll();
    }
}
