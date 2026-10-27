using System;
using bms_manager.Domain.Repositories;
using bms_manager.Presentation.Localization;

namespace bms_manager.Application.Services;

public sealed class NameService : INameService
{
    private readonly INameRepository _repository;

    public NameService(INameRepository repository, LocalizationProvider localization)
    {
        _repository = repository;
        Localization = localization;
    }
    
    public LocalizationProvider Localization { get; }

    public void SaveName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(Localization.SaveError, nameof(name));
        }

        string normalizedName = name.Trim();

        _repository.Save(normalizedName);
    }
}