using GameTrip.Domain.Entities;
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

        if (_context.Roles.Any() || _context.Users.Any())
            return false;

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

        //Adding Locations
        List<LocationDto> locations = new()
        {
            new LocationDto("Tour Eiffel", "Monument emblématique de Paris, France", 48.8588443m, 2.2943506m),
            new LocationDto("Statue de la Liberté", "Symbole de liberté à New York, États-Unis", 48.8588443m, 2.2943506m),
            new LocationDto("Colisée", "Ancien amphithéâtre romain à Rome, Italie", 48.8588443m, 2.2943506m),
            new LocationDto("Grande Muraille de Chine", "Merveille architecturale à Pékin, Chine", 48.8588443m, 2.2943506m),
            new LocationDto("Opéra de Sydney", "Icone moderne de l'Australie à Sydney, Australie", 48.8588443m, 2.2943506m),
            new LocationDto("Machu Picchu", "Site archéologique incas au Pérou", 48.8588443m, 2.2943506m),
            new LocationDto("Pyramides de Gizeh", "Anciens monuments égyptiens près du Caire, Égypte", 48.8588443m, 2.2943506m),
            new LocationDto("Cristo Redentor", "Statue du Christ rédempteur à Rio de Janeiro, Brésil", 48.8588443m, 2.2943506m),
            new LocationDto("Acropole d'Athènes", "Site archéologique en Grèce", 48.8588443m, 2.2943506m),
            new LocationDto("Mont Saint-Michel", "Monastère fortifié en France", 48.8588443m, 2.2943506m),
            new LocationDto("Taj Mahal", "Mausolée à Agra, Inde", 48.8588443m, 2.2943506m),
            new LocationDto("Mur de Berlin", "Ancienne frontière divisant Berlin, Allemagne", 48.8588443m, 2.2943506m),
            new LocationDto("Tour de Londres", "Château historique à Londres, Royaume-Uni", 48.8588443m, 2.2943506m),
            new LocationDto("Place Rouge", "Place emblématique de Moscou, Russie", 48.8588443m, 2.2943506m),
            new LocationDto("Château de Versailles", "Palais royal à Versailles, France", 48.8588443m, 2.2943506m),
            new LocationDto("Central Park", "Parc emblématique de New York, États-Unis", 48.8588443m, 2.2943506m),
            new LocationDto("Tour CN", "Gratte-ciel emblématique de Toronto, Canada", 48.8588443m, 2.2943506m),
            new LocationDto("Angkor Wat", "Temple au Cambodge", 48.8588443m, 2.2943506m),
            new LocationDto("Mont Everest", "Plus haute montagne du monde à la frontière du Népal et du Tibet", 48.8588443m, 2.2943506m)
        };
        foreach (LocationDto location in locations)
        {
            if (await _context.Location.FirstOrDefaultAsync(l => l.Name == location.Name) is null)
            {
                await _context.Location.AddAsync(new()
                {
                    Name = location.Name,
                    Description = location.Description,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                });
            }
        }

        await _context.SaveChangesAsync();

        return true;
    }
}