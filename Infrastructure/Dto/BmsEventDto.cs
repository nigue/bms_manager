using bms_manager.Domain.Model;

namespace bms_manager.Infrastructure.Dto;

public record BmsEventDto(Guid identifier, int number, string name)
{
    public BmsEvent ToModel() => new (identifier, number, name);
}