using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace GameTrip.API.Data;

public class GameTripContext : DbContext
{
    #region Properties

    public DbSet<Comment> Comment{ get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<GameTripUser> GameTripUser { get; set; }
    public DbSet<LikedLocation> LikedLocation { get; set; }
    public DbSet<LikedGame> LikedGame { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<Picture> Picture { get; set; }

    #endregion Properties

    #region Constructor

    public GameTripContext()
    {
    }

    public GameTripContext(DbContextOptions<GameTripContext> options) : base(options)
    {
    }

    #endregion Constructor

    #region Methods

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<Comment>(c =>
        {
            c.HasKey(c => c.IdComment);

            c.Property(c => c.Text).HasMaxLength(255);
        });

        builder.Entity<Game>(g =>
        {
            g.HasKey(g => g.IdGame);

            g.Property(g => g.Name).HasMaxLength(255);
            g.Property(g => g.Description).HasMaxLength(255);
            g.Property(g => g.Editor).HasMaxLength(255);
            
            g.HasMany(g => g.Pictures).WithOne(p => p.Game).HasForeignKey(p => p.GameId).OnDelete(DeleteBehavior.Cascade);
            g.HasMany(g => g.LikedGames).WithOne(lg => lg.Game).HasForeignKey(lg => lg.GameId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<LikedGame>(lg =>
        {
            lg.HasKey(lg => lg.IdLikedGame);
        });

        builder.Entity<LikedLocation>(ll =>
        {
            ll.HasKey(ll => ll.IdLikedLocation);
        });

        builder.Entity<Location>(l =>
        {
            l.HasKey(l => l.IdLocation);

            l.Property(l => l.Name).HasMaxLength(255);
            l.Property(l => l.Description).HasMaxLength(255);
            l.Property(l => l.Latitude).HasPrecision(18,12);
            l.Property(l => l.Longitude).HasPrecision(18,12);

            l.HasMany(l => l.Comments).WithOne(c => c.Location).HasForeignKey(c => c.LocationId).OnDelete(DeleteBehavior.Cascade);
            l.HasMany(l => l.Pictures).WithOne(p => p.Location).HasForeignKey(p => p.LocationId).OnDelete(DeleteBehavior.Cascade);
            l.HasMany(l => l.Games).WithMany(g => g.Locations);
            l.HasMany(l => l.LikedLocations).WithOne(ll => ll.Location).HasForeignKey(ll => ll.LocationId).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Picture>(p =>
        {
            p.HasKey(p => p.IdPicture);

            p.Property(p => p.Name).HasMaxLength(255);
            p.Property(p => p.Description).HasMaxLength(255);
            p.Property(p => p.Path).HasMaxLength(255);
        });

        builder.Entity<GameTripUser>(u =>
        {
            u.HasKey(u => u.Id);
            
            u.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            u.HasMany(u => u.LikedLocations).WithOne(lg => lg.User).HasForeignKey(lg => lg.UserId).OnDelete(DeleteBehavior.Cascade);
            u.HasMany(u => u.LikedGames).WithOne(ll => ll.User).HasForeignKey(ll => ll.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    #endregion Methods
}
