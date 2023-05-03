using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Errors;
using GameTrip.Domain.Extension;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
#if !DEBUG
[Authorize]
#endif
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationPlarform _locationPlarform;
    private readonly IValidator<LocationDto> _locationValidator;

    public LocationController(ILocationPlarform locationPlarform, IValidator<LocationDto> locationValidator)
    {
        _locationPlarform = locationPlarform;
        _locationValidator = locationValidator;
    }

    [HttpPost]
    [Route("CreateLocation")]
    public IActionResult CreateLocation(LocationDto dto)
    {
        ValidationResult result = _locationValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);
            return BadRequest(ModelState);
        }

        Location? location = _locationPlarform.GetLocationByName(dto.Name);
        if (location is not null)
            return BadRequest(new ErrorResultDTO(LocationErrors.AlreadyExistByName));

        location ??= _locationPlarform.GetLocationByPosition(dto.Latitude, dto.Longitude);
        if (location is not null)
            return BadRequest(new ErrorResultDTO(LocationErrors.AlreadyExistByPos));

        _locationPlarform.CreateLocation(dto.ToEntity());
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("StaticLocations")]
    public ActionResult<List<LocationDto>> GetStaticLocations()
    {
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

        return locations;
    }
}