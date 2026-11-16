using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Dto;

namespace bms_manager.Infrastructure.Repositories;

public sealed class InMemoryBmsEventRepository : IBmsEventRepository
{
    private static readonly IReadOnlyList<BmsEventDto> Events =
    [
        new(1, "Evento 1"),
        new(2, "Evento 2"),
        new(3, "Evento 3"),
        new(4, "Evento 4"),
        new(5, "Evento 5"),
        new(6, "Evento 6"),
        new(7, "Evento 7"),
        new(8, "Evento 8"),
        new(9, "Evento 9"),
        new(10, "Evento 10")
    ];

    public IReadOnlyList<BmsEvent> GetAll()
    {
        return Events
            .Select(bmsEvent => bmsEvent.ToModel())
            .ToArray();
    }
}
