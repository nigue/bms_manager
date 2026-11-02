using bms_manager.Domain.Model;

namespace bms_manager.Domain.Repositories;

public interface IBmsEventRepository
{
    IReadOnlyList<BmsEvent> GetAll();
}
