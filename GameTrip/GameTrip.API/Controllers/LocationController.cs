using GameTrip.API.Models;
using GameTrip.Domain.Entities;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationPlarform _locationPlarform;

    public LocationController(ILocationPlarform locationPlarform) => _locationPlarform = locationPlarform;

    [HttpPost]
    [Route("CreateLocation")]
    public async Task<ActionResult<LocationDTO>> CreateLocation(LocationDTO locationDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Location location = _locationPlarform.GetLocationByName(locationDTO.Name);

        //Location location = new()
        //{
        //    Name = locationDTO.Name,
        //    Description = locationDTO.Description,
        //    Latitude = locationDTO.Latitude,
        //    Longitude = locationDTO.Longitude,
        //    TripId = locationDTO.TripId
        //};

        //_context.Locations.Add(location);
        //await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("StaticLocations")]
    public ActionResult<List<LocationDTO>> GetStaticLocations()
    {
        List<LocationDTO> locations = new()
        {
            new LocationDTO("Tour Eiffel", "Monument emblématique de Paris, France", 48.8588443, 2.2943506),
            new LocationDTO("Statue de la Liberté", "Symbole de liberté à New York, États-Unis", 40.689247, -74.044502),
            new LocationDTO("Colisée", "Ancien amphithéâtre romain à Rome, Italie", 41.8902102, 12.4922315),
            new LocationDTO("Grande Muraille de Chine", "Merveille architecturale à Pékin, Chine", 40.431908, 116.570374),
            new LocationDTO("Opéra de Sydney", "Icone moderne de l'Australie à Sydney, Australie", -33.8568, 151.2153),
            new LocationDTO("Machu Picchu", "Site archéologique incas au Pérou", -13.1631, -72.5450),
            new LocationDTO("Pyramides de Gizeh", "Anciens monuments égyptiens près du Caire, Égypte", 29.9792, 31.1342),
            new LocationDTO("Cristo Redentor", "Statue du Christ rédempteur à Rio de Janeiro, Brésil", -22.9519, -43.2105),
            new LocationDTO("Acropole d'Athènes", "Site archéologique en Grèce", 37.9715, 23.7262),
            new LocationDTO("Mont Saint-Michel", "Monastère fortifié en France", 48.6361, -1.5113),
            new LocationDTO("Taj Mahal", "Mausolée à Agra, Inde", 27.1750, 78.04),
            new LocationDTO("Mur de Berlin", "Ancienne frontière divisant Berlin, Allemagne", 52.5200, 13.4050),
            new LocationDTO("Tour de Londres", "Château historique à Londres, Royaume-Uni", 51.5081, -0.0761),
            new LocationDTO("Place Rouge", "Place emblématique de Moscou, Russie", 55.7539, 37.6208),
            new LocationDTO("Château de Versailles", "Palais royal à Versailles, France", 48.8048, 2.1204),
            new LocationDTO("Central Park", "Parc emblématique de New York, États-Unis", 40.7851, -73.9683),
            new LocationDTO("Tour CN", "Gratte-ciel emblématique de Toronto, Canada", 43.6516, -79.3832),
            new LocationDTO("Angkor Wat", "Temple au Cambodge", 13.4125, 103.8660),
            new LocationDTO("Mont Everest", "Plus haute montagne du monde à la frontière du Népal et du Tibet", 27.9881, 86.9253)
        };

        return locations;
    }
}