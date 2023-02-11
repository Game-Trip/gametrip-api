using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace GameTrip.API.Data;

public class GameTripContext : DbContext
{
    #region Properties

    public DbSet<AA> aa{ get; set; }

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
        builder.Entity<AA>(f =>
        {
            f.HasKey(f => f.IdAA);
            
            f.Property(f => f.IdAA).HasDefaultValueSql("(newid())");
        });
    }

    #endregion Methods
}
