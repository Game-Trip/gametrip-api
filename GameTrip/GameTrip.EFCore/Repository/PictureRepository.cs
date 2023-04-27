using GameTrip.Domain.Entities;
using GameTrip.Domain.Interfaces;

namespace GameTrip.EFCore.Repository;

public class PictureRepository : GenericRepository<Picture>, IPictureRepository
{
    public PictureRepository(GameTripContext context) : base(context)
    {
    }
}