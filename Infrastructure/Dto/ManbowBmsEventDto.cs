using bms_manager.Domain.Model;

namespace bms_manager.Infrastructure.Dto;

public record ManbowBmsEventDto(
    int EventNumber,
    string Name,
    IReadOnlyList<string> FolderName,
    string NameLink,
    string RegistrationDate,
    string ImpressionDate,
    int Entry,
    int ImpressionPoints,
    string InformationLink,
    string DetailsLink,
    string ListLink,
    bool Available
)
{
    public BmsEvent ToModel() => new(EventNumber, Name);
};