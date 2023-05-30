using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Repository;
public class RequestLocationUpdateRepository : GenericRepository<RequestLocationUpdate>, IRequestLocationUpdateRepository
{
    public RequestLocationUpdateRepository(GameTripContext context) : base(context)
    {
    }

    public async Task DeleteAllUpdateRequestAsync(Guid locationId)
    {
        List<RequestLocationUpdate> requestLocationUpdates = await _context.RequestLocationUpdate.Where(rl => rl.LocationId == locationId).ToListAsync();
        _context.RequestLocationUpdate.RemoveRange(requestLocationUpdates);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUpdateRequestByIdAsync(Guid? requestUpdateId)
    {
        RequestLocationUpdate requestLocationUpdate = await _context.RequestLocationUpdate.FirstOrDefaultAsync(rl => rl.IdRequestLocationUpdate == requestUpdateId);
        _context.RequestLocationUpdate.Remove(requestLocationUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task<RequestLocationUpdate?> GetRequestUpdateLocationByIdAsync(Guid requestUpdateId) => await _context.RequestLocationUpdate.FirstOrDefaultAsync(rlu => rlu.IdRequestLocationUpdate == requestUpdateId);
}
