using bms_manager.Domain.Model;

namespace bms_manager.Application.Services;

public interface IBmsEventService
{
    IReadOnlyList<BmsEvent> GetAllEvents();
}
