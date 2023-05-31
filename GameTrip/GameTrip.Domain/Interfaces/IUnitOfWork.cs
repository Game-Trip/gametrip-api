namespace GameTrip.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICommentRepository Comments { get; }
    IGameRepository Games { get; }
    IRequestGameUpdateRepository RequestGameUpdate { get; }
    ILikedGameRepository LikedGames { get; }
    ILikedLocationRepository LikedLocations { get; }
    ILocationRepository Locations { get; }
    IRequestLocationUpdateRepository RequestLocationUpdate { get; }
    IPictureRepository Pictures { get; }

    int Complet();

    Task<int> CompletAsync();
}