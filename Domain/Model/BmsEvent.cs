using bms_manager.Presentation.Models;

namespace bms_manager.Domain.Model;

public record BmsEvent(int number, string name)
{
    public BmsEventModel ToModel() => new(number, name);
}