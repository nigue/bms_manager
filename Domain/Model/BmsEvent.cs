using bms_manager.Presentation.Models;

namespace bms_manager.Domain.Model;

public record BmsEvent(Guid identifier, int number, string name)
{
    public BmsEventModel ToModel() => new(identifier, number, name);
}