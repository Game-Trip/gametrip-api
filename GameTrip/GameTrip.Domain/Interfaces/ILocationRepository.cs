using GameTrip.Domain.Entities;

namespace GameTrip.Domain.Interfaces;

public interface ILocationRepository : IGenericRepository<Location>
{
    Location? GetLocationByName(string name);
    Location? GetLocationByPos(decimal latitude, decimal longitude);
}