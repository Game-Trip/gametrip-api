using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;
public interface IRequestGameUpdateRepository : IGenericRepository<RequestGameUpdate>
{
    Task DeleteAllUpdateRequestAsync(Guid gameId);
    Task DeleteUpdateRequestByIdAsync(Guid? requestUpdateId);
    Task<RequestGameUpdate?> GetRequestGameUpdateByIdAsync(Guid requestUpdateId);
}
