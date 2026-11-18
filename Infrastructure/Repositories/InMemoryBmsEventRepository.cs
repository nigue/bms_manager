using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Dto;

namespace bms_manager.Infrastructure.Repositories;

public sealed class InMemoryBmsEventRepository : IBmsEventRepository
{
    private static readonly IReadOnlyList<BmsEventDto> Events =
    [
        new(1, "Evento 1", "2024-01-01"),
        new(2, "Evento 2", "2024-01-02"),
        new(3, "Evento 3", "2024-01-03"),
        new(4, "Evento 4", "2024-01-04"),
        new(5, "Evento 5", "2024-01-05"),
        new(6, "Evento 6", "2024-01-06"),
        new(7, "Evento 7", "2024-01-07"),
        new(8, "Evento 8", "2024-01-08"),
        new(9, "Evento 9", "2024-01-09"),
        new(10, "Evento 10", "2024-01-10")
    ];

    public IReadOnlyList<BmsEvent> GetAll()
    {
        return Events
            .Select(bmsEvent => bmsEvent.ToModel())
            .ToArray();
    }
}
