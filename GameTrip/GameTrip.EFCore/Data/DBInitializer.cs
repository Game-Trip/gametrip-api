using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.EFCore.Data;
/// <summary>
/// The d b initializer.
/// </summary>

public class DBInitializer
{
    private readonly GameTripContext _context;
    private readonly UserManager<GameTripUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DBInitializer"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="roleManager">The role manager.</param>
    public DBInitializer(GameTripContext context, UserManager<GameTripUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Initializes the.
    /// </summary>
    /// <returns>A Task.</returns>
    public async Task<bool> Initialize()
    {
        _context.Database.EnsureCreated();
        //await _context.LikedGame.ExecuteDeleteAsync();
        //await _context.LikedLocation.ExecuteDeleteAsync();
        //await _context.Comment.ExecuteDeleteAsync();
        //await _context.Picture.ExecuteDeleteAsync();
        //await _context.Game.ExecuteDeleteAsync();
        //await _context.Location.ExecuteDeleteAsync();
        //await _context.Users.ExecuteDeleteAsync();
        //await _context.Roles.ExecuteDeleteAsync();

        if (_context.Roles.Any() || _context.Users.Any())
            return false;

        #region AddRoles
        //Adding roles
        string[] roles = Roles.GetAllRoles();

        foreach (string role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                IdentityResult resultAddRole = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }
        await _context.SaveChangesAsync();
        #endregion

        #region AddAdmin
        //Adding Admin
        GameTripUser admin = new()
        {
            UserName = "Dercraker",
            Email = "antoine.capitain@gmail.com",
            EmailConfirmed = true,
        };

        string pwd = "NMdRx$HqyT8jX6";

        IdentityResult? resultAddUser = await _userManager.CreateAsync(admin, pwd);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + resultAddUser.Errors);

        IdentityResult resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + resultAddRoleToUser.Errors);

        resultAddRoleToUser = await _userManager.AddToRoleAsync(admin, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);
        await _context.SaveChangesAsync();
        admin = await _userManager.FindByNameAsync(admin.UserName);
        #endregion

        #region AddUser
        //Adding Admin
        GameTripUser user = new()
        {
            UserName = "DercrakerDev",
            Email = "antoine.capitain+Dev@gmail.com",
            EmailConfirmed = true,
        };

        pwd = "NMdRx$HqyT8jX6";

        resultAddUser = await _userManager.CreateAsync(user, pwd);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + user.UserName + "' failed with error(s): " + resultAddUser.Errors);

        resultAddRoleToUser = await _userManager.AddToRoleAsync(user, Roles.User);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + user.UserName + "' to role '" + Roles.User + "' failed with error(s): " + resultAddRoleToUser.Errors);
        await _context.SaveChangesAsync();
        #endregion

        //Adding Locations
        #region AddLocation
        List<LocationDto> locations = new()
        {
            new LocationDto("Tour Eiffel", "Monument emblématique de Paris, France", 48.8588443m, 2.2943506m, admin.Id, true),
            new LocationDto("Statue de la Liberté", "Symbole de liberté à New York, États-Unis", 40.6892494m, -74.0445004m, admin.Id, true),
            new LocationDto("Colisée", "Ancien amphithéâtre romain à Rome, Italie", 41.8902102m, 12.4922309m, admin.Id, true),
            new LocationDto("Grande Muraille de Chine", "Merveille architecturale à Pékin, Chine", 40.4319089m, 116.570374m, admin.Id, true),
            new LocationDto("Opéra de Sydney", "Icone moderne de l'Australie à Sydney, Australie", -33.8567844m, 151.213108m, admin.Id, true),
            new LocationDto("Machu Picchu", "Site archéologique incas au Pérou", -13.1631412m, -72.5449637m, admin.Id, true),
            new LocationDto("Pyramides de Gizeh", "Anciens monuments égyptiens près du Caire, Égypte", 29.9792345m, 31.1342019m, admin.Id, true),
            new LocationDto("Cristo Redentor", "Statue du Christ rédempteur à Rio de Janeiro, Brésil", -22.951916m, -43.2104872m, admin.Id, true),
            new LocationDto("Acropole d'Athènes", "Site archéologique en Grèce", 37.9715327m, 23.7257493m, admin.Id, true),
            new LocationDto("Mont Saint-Michel", "Monastère fortifié en France", 48.635935m, -1.510712m, admin.Id, true),
            new LocationDto("Taj Mahal", "Mausolée à Agra, Inde", 27.1750151m, 78.0421552m, admin.Id, true),
            new LocationDto("Mur de Berlin", "Ancienne frontière divisant Berlin, Allemagne", 52.5200066m, 13.404954m, admin.Id, true),
            new LocationDto("Tour de Londres", "Château historique à Londres, Royaume-Uni", 51.5081126m, -0.0759493m, admin.Id, true),
            new LocationDto("Place Rouge", "Place emblématique de Moscou, Russie", 55.7539303m, 37.620795m, admin.Id, true),
            new LocationDto("Château de Versailles", "Palais royal à Versailles, France", 48.8048649m, 2.1203555m, admin.Id, true),
            new LocationDto("Central Park", "Parc emblématique de New York, États-Unis", 40.7828647m, -73.9653551m, admin.Id, true),
            new LocationDto("Tour CN", "Gratte-ciel emblématique de Toronto, Canada", 43.642567m, -79.387054m, admin.Id, true),
            new LocationDto("Angkor Wat", "Temple au Cambodge", 13.412469m, 103.866986m, admin.Id, true),
            new LocationDto("Mont Everest", "Plus haute montagne du monde à la frontière du Népal et du Tibet", 27.988119m, 86.925277m, admin.Id, true)
        };
        foreach (LocationDto loc in locations)
        {
            if (await _context.Location.FirstOrDefaultAsync(l => l.Name == loc.Name) is null)
            {
                await _context.Location.AddAsync(new()
                {
                    Name = loc.Name,
                    Description = loc.Description,
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    AuthorId = loc.AuthorId,
                    Author = admin,
                    IsValid = loc.IsValidate
                });
            }
        }
        #endregion

        //Adding Games
        #region AddGames
        DateTime date = new(2023, 5, 19); // The date you want to get the timestamp of
        DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // The Unix epoch
        TimeSpan timeSpan = date.ToUniversalTime() - unixEpoch; // Get the time span between the date and the Unix epoch
        long timestamp = (long)timeSpan.TotalSeconds; // Get the total number of seconds as a long integer

        List<CreateGameDto> games = new()
        {
            new CreateGameDto
            {
                Name = "Assassin's Creed Unity",
                Description = "Un jeu vidéo d'action-aventure en monde ouvert développé par Ubisoft",
                Editor = "Ubisoft",
                ReleaseDate = (long)(new DateTime(2014, 11, 11).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Call of Duty: Modern Warfare 3",
                Description = "Un jeu vidéo de tir à la première personne développé par Infinity Ward et édité par Activision",
                Editor = "Activision",
                ReleaseDate = (long)(new DateTime(2011, 11, 8).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Assassin's Creed: Brotherhood",
                Description = "Un jeu vidéo d'action-aventure en monde ouvert développé par Ubisoft",
                Editor = "Ubisoft",
                ReleaseDate = (long)(new DateTime(2010, 11, 16).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Age of Empires II",
                Description = "Un jeu vidéo de stratégie en temps réel développé par Ensemble Studios et édité par Microsoft",
                Editor = "Microsoft",
                ReleaseDate = (long)(new DateTime(1999, 9, 30).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Uncharted 2: Among Thieves",
                Description = "Un jeu vidéo d'action-aventure développé par Naughty Dog et édité par Sony Computer Entertainment",
                Editor = "Sony Computer Entertainment",
                ReleaseDate = (long)(new DateTime(2009, 10, 13).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Indiana Jones and the Fate of Atlantis",
                Description = "Un jeu vidéo d'aventure graphique développé et édité par LucasArts",
                Editor = "LucasArts",
                ReleaseDate = (long)(new DateTime(1992, 6, 1).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "Civilization V",
                Description = "Un jeu vidéo de stratégie au tour par tour développé par Firaxis Games et édité par 2K Games",
                Editor = "2K Games",
                ReleaseDate = (long)(new DateTime(2010, 9, 21).ToUniversalTime() - unixEpoch).TotalSeconds,
            },
            new CreateGameDto
            {
                Name = "FIFA 21",
                Description = "Un jeu vidéo de simulation de football développé et édité par Electronic Arts",
                Editor = "Electronic Arts",
                ReleaseDate = (long)(new DateTime(2020, 10, 9).ToUniversalTime() - unixEpoch).TotalSeconds,
            }
        };

        foreach (CreateGameDto gam in games)
        {
            if (await _context.Game.FirstOrDefaultAsync(g => g.Name == gam.Name) is null)
            {
                await _context.Game.AddAsync(new()
                {
                    Name = gam.Name,
                    Description = gam.Description,
                    Editor = gam.Editor,
                    ReleaseDate = gam.ReleaseDate,
                    AuthorId = admin.Id,
                    Author = admin,
                    IsValidate = true
                });
            }
        }
        await _context.SaveChangesAsync();
        #endregion

        #region AddOneGameToLocation
        Location? location = _context.Location.Include(l => l.Games).FirstOrDefault(l => l.Name == "Tour Eiffel");
        Game? game = _context.Game.FirstOrDefault(g => g.Name == "Assassin's Creed Unity");
        if (location is null && game is null)
            throw new ApplicationException("Location : Tour Eiffel or Game : Assassin's Creed Unity not found");

        location!.Games!.Add(game!);
        await _context.SaveChangesAsync();
        #endregion

        return true;
    }
}