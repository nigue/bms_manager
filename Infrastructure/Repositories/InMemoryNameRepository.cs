using System.Collections.Generic;
using bms_manager.Domain.Repositories;

namespace bms_manager.Infrastructure.Repositories;

public sealed class InMemoryNameRepository : INameRepository
{
    private readonly List<string> _names = new();

    public IReadOnlyList<string> SavedNames => _names.AsReadOnly();

    public void Save(string name)
    {
        _names.Add(name);
    }
}