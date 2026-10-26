using System;
using bms_manager.Domain.Repositories;

namespace bms_manager.Application.Services;

public sealed class NameService : INameService
{
    private readonly INameRepository _repository;

    public NameService(INameRepository repository)
    {
        _repository = repository;
    }

    public void SaveName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Debes escribir un nombre.",
                nameof(name));
        }

        string normalizedName = name.Trim();

        _repository.Save(normalizedName);
    }
}