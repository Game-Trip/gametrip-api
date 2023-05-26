using GameTrip.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore;

public class GameTripContext : IdentityDbContext<GameTripUser, IdentityRole<Guid>, Guid>
{
    #region Properties

    public DbSet<Comment> Comment { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<RequestGameUpdate> RequestGameUpdate { get; set; }
    public DbSet<GameTripUser> GameTripUser { get; set; }
    public DbSet<LikedLocation> LikedLocation { get; set; }
    public DbSet<LikedGame> LikedGame { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<RequestLocationUpdate> RequestLocationUpdate { get; set; }
    public DbSet<Picture> Picture { get; set; }

    #endregion Properties

    #region Constructor

    public GameTripContext()
    {
    }

    public GameTripContext(DbContextOptions<GameTripContext> options) : base(options) => Database.Migrate();

    #endregion Constructor

    #region Methods

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Comment>(c =>
        {
            c.HasKey(c => c.IdComment);

            c.Property(c => c.Text).HasMaxLength(255);
            c.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Game>(g =>
        {
            g.ToTable(g => g.IsTemporal());

            g.HasKey(g => g.IdGame);

            g.Property(g => g.Name).HasMaxLength(255);
            g.Property(g => g.Description).HasMaxLength(255);
            g.Property(g => g.Editor).HasMaxLength(255);
            g.Property(g => g.ReleaseDate).HasPrecision(9);

            g.HasMany(g => g.Pictures).WithOne(p => p.Game).HasForeignKey(p => p.GameId).OnDelete(DeleteBehavior.Cascade);
            g.HasMany(g => g.LikedGames).WithOne(lg => lg.Game).HasForeignKey(lg => lg.GameId).OnDelete(DeleteBehavior.Cascade);
            g.HasMany(g => g.RequestGameUpdates).WithOne(rgu => rgu.Game).HasForeignKey(rgu => rgu.GameId).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<RequestGameUpdate>(rg =>
        {
            rg.HasKey(rg => rg.IdRequestGameUpdate);
        });

        builder.Entity<LikedGame>(lg =>
        {
            lg.HasKey(lg => lg.IdLikedGame);
            lg.Property(lg => lg.Vote).HasPrecision(2, 1);
        });

        builder.Entity<LikedLocation>(ll =>
        {
            ll.HasKey(ll => ll.IdLikedLocation);
            ll.Property(ll => ll.Vote).HasPrecision(2, 1);
        });

        builder.Entity<Location>(l =>
        {
            l.ToTable(t => t.IsTemporal());

            l.HasKey(l => l.IdLocation);

            l.Property(l => l.Name).HasMaxLength(255);
            l.Property(l => l.Description).HasMaxLength(255);
            l.Property(l => l.Latitude).HasPrecision(18, 12);
            l.Property(l => l.Longitude).HasPrecision(18, 12);

            l.HasMany(l => l.Comments).WithOne(c => c.Location).HasForeignKey(c => c.LocationId).OnDelete(DeleteBehavior.Cascade);
            l.HasMany(l => l.Pictures).WithOne(p => p.Location).HasForeignKey(p => p.LocationId).OnDelete(DeleteBehavior.Cascade);
            l.HasMany(l => l.Games).WithMany(g => g.Locations);
            l.HasMany(l => l.LikedLocations).WithOne(ll => ll.Location).HasForeignKey(ll => ll.LocationId).OnDelete(DeleteBehavior.Cascade);
            l.HasMany(l => l.RequestLocationUpdates).WithOne(rl => rl.Location).HasForeignKey(rl => rl.LocationId).OnDelete(DeleteBehavior.NoAction);

        });

        builder.Entity<RequestLocationUpdate>(rl =>
        {
            rl.HasKey(rl => rl.IdRequestLocationUpdate);

            rl.Property(rl => rl.Latitude).HasPrecision(18, 12);
            rl.Property(rl => rl.Longitude).HasPrecision(18, 12);
        });

        builder.Entity<Picture>(p =>
        {
            p.HasKey(p => p.IdPicture);

            p.Property(p => p.Name).IsRequired().HasMaxLength(255);
            p.Property(p => p.Description).HasMaxLength(255);
            p.Property(p => p.Data).IsRequired().HasColumnType("varbinary(max)");

        });

        builder.Entity<GameTripUser>(u =>
        {
            u.HasKey(u => u.Id);

            u.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            u.HasMany(u => u.LikedLocations).WithOne(lg => lg.User).HasForeignKey(lg => lg.UserId).OnDelete(DeleteBehavior.Cascade);
            u.HasMany(u => u.LikedGames).WithOne(ll => ll.User).HasForeignKey(ll => ll.UserId).OnDelete(DeleteBehavior.Cascade);

            u.HasMany(u => u.CreatedGame).WithOne(g => g.Author).HasForeignKey(g => g.AuthorId).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(u => u.CreatedLocation).WithOne(l => l.Author).HasForeignKey(l => l.AuthorId).OnDelete(DeleteBehavior.NoAction);
            u.HasMany(u => u.CreatedPictures).WithOne(p => p.Author).HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.NoAction);

        });
    }

    #endregion Methods
}