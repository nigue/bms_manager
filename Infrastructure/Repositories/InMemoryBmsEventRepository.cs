using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Dto;

namespace bms_manager.Infrastructure.Repositories;

public sealed class InMemoryBmsEventRepository : IBmsEventRepository
{
    private static readonly IReadOnlyList<BmsEventDto> Events =
    [
        new(Guid.NewGuid(), 1, "Evento 1"),
        new(Guid.NewGuid(), 2, "Evento 2"),
        new(Guid.NewGuid(), 3, "Evento 3"),
        new(Guid.NewGuid(), 4, "Evento 4"),
        new(Guid.NewGuid(), 5, "Evento 5"),
        new(Guid.NewGuid(), 6, "Evento 6"),
        new(Guid.NewGuid(), 7, "Evento 7"),
        new(Guid.NewGuid(), 8, "Evento 8"),
        new(Guid.NewGuid(), 9, "Evento 9"),
        new(Guid.NewGuid(), 10, "Evento 10")
    ];

    public IReadOnlyList<BmsEvent> GetAll()
    {
        return Events
            .Select(bmsEvent => bmsEvent.ToModel())
            .ToArray();
    }
}
