namespace GameTrip.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICommentRepository Comments { get; }
    IGameRepository Games { get; }
    ILikedGameRepository LikedGames { get; }
    ILikedLocationRepository LikedLocations { get; }
    ILocationRepository Locations { get; }
    IPictureRepository Pictures { get; }

    int Complet();

    Task<int> CompletAsync();
}