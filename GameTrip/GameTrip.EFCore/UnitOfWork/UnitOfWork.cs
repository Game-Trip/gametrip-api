using GameTrip.Domain.Interfaces;
using GameTrip.EFCore.Repository;

namespace GameTrip.EFCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameTripContext _context;

    public ICommentRepository Comments { get; private set; }
    public IGameRepository Games { get; private set; }
    public ILikedGameRepository LikedGames { get; private set; }
    public ILikedLocationRepository LikedLocations { get; private set; }
    public ILocationRepository Locations { get; private set; }
    public IPictureRepository Pictures { get; private set; }

    public UnitOfWork(GameTripContext context)
    {
        _context = context;
        Comments = new CommentRepository(_context);
        Games = new GameRepository(_context);
        LikedGames = new LikedGameRepository(_context);
        LikedLocations = new LikedLocationRepository(_context);
        Locations = new LocationRepository(_context);
        Pictures = new PictureRepository(_context);
    }

    public int Complet() => _context.SaveChanges();

    public async Task<int> CompletAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}