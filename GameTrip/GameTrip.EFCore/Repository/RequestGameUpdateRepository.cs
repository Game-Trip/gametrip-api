using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;
public class RequestGameUpdateRepository : GenericRepository<RequestGameUpdate>, IRequestGameUpdateRepository
{
    public RequestGameUpdateRepository(GameTripContext context) : base(context)
    {

    }

    public async Task DeleteAllUpdateRequestAsync(Guid gameId)
    {
        List<RequestGameUpdate> requestGameUpdates = await _context.RequestGameUpdate.Where(rg => rg.GameId == gameId).ToListAsync();
        _context.RequestGameUpdate.RemoveRange(requestGameUpdates);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUpdateRequestByIdAsync(Guid? requestUpdateId)
    {
        RequestGameUpdate? requestGameUpdate = await _context.RequestGameUpdate.FirstOrDefaultAsync(rg => rg.IdRequestGameUpdate == requestUpdateId);
        if (requestGameUpdate is not null)
            _context.RequestGameUpdate.Remove(requestGameUpdate);

        await _context.SaveChangesAsync();
    }

    public Task<RequestGameUpdate?> GetRequestGameUpdateByIdAsync(Guid requestUpdateId) => _context.RequestGameUpdate.FirstOrDefaultAsync(rgu => rgu.IdRequestGameUpdate == requestUpdateId);
}
