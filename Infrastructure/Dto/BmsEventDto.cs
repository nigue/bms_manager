using bms_manager.Domain.Model;

namespace bms_manager.Infrastructure.Dto;

public record BmsEventDto(int number, string name, string date)
{
    public BmsEvent ToModel() => new(number, name, date);
}