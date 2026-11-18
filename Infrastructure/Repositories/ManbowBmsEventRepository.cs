using System.IO;
using System.Text.Json;
using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Dto;

namespace bms_manager.Infrastructure.Repositories;

public sealed class ManbowBmsEventRepository : IBmsEventRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly IReadOnlyList<ManbowBmsEventDto> _events;

    public ManbowBmsEventRepository()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "Infrastructure",
            "Data",
            "manbow.json");
        string json = File.ReadAllText(path);

        _events = JsonSerializer.Deserialize<List<ManbowBmsEventDto>>(json, JsonOptions)
            ?? throw new InvalidDataException("The Manbow event data file contains no valid event list.");
    }

    public IReadOnlyList<BmsEvent> GetAll()
    {
        return _events
            .Where(bmsEvent => bmsEvent.Available)
            .Select(bmsEvent => bmsEvent.ToModel())
            .ToArray();
    }
}
