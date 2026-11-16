namespace bms_manager.Infrastructure.Dto;

public record ManbowBmsEventDto(
    int EventNumber,
    string Name,
    string FolderName,
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
    
};