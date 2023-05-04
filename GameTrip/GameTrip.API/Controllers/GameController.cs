using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
#if !DEBUG
[Authorize(Roles = User)]
#endif
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGamePlatform _gamePlatform;
    private readonly ILocationPlarform _locationPlatform;
    private readonly IValidator<CreateGameDto> _createGameValidator;

    public GameController(IValidator<CreateGameDto> createGameValidator, IGamePlatform gamePlatform, ILocationPlarform locationPlarform)
    {
        _createGameValidator = createGameValidator;
        _gamePlatform = gamePlatform;
        _locationPlatform = locationPlarform;
    }

#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
    [HttpPost]
    [Route("CreateGame")]
    public async Task<ActionResult<Game>> CreateGame(CreateGameDto dto)
    {
        ValidationResult result = _createGameValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Game? game = await _gamePlatform.GetGameByNameAsync(dto.Name);
        if (game is not null)
            return BadRequest(new MessageDto(GameMessage.AlreadyExist));

        await _gamePlatform.CreateGameAsync(dto.ToEntity());
        return Ok(new MessageDto(GameMessage.SuccesCreated));
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<ListGameDto>>> GetGamesAsync()
    {
        IEnumerable<Game> games = await _gamePlatform.GetAllGamesAsync();
        return games.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Id/{gameId}")]
    public async Task<ActionResult<GameDto>> GetGaleByIdAsync([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
        {
            return NotFound(new MessageDto(GameMessage.NotFoundById));
        }

        return game.ToDto();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Name/{gameName}")]
    public async Task<ActionResult<GameDto>> GetLocationByNameAsync([FromRoute] string gameName)
    {
        Game? game = await _gamePlatform.GetGameByNameAsync(gameName);
        if (game is null)
        {
            return NotFound(new MessageDto(GameMessage.NotFoundByName));
        }

        return game.ToDto();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Location/Id/{locationId}")]
    public async Task<ActionResult<List<ListGameDto?>>> GetGamesByLocationIdAsync([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        IEnumerable<Game?> games = await _gamePlatform.GetGamesByLocationIdAsync(location.IdLocation);
        if (games.Any())
            return NotFound(new MessageDto(GameMessage.NotFoundByLocationId));

        return games.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Location/Name/{locationName}")]
    public async Task<ActionResult<List<ListGameDto?>>> GetGamesByLocationNameAsync([FromRoute] string locationName)
    {
        Location? location = await _locationPlatform.GetLocationByNameAsync(locationName);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundByName));

        IEnumerable<Game?> game = await _gamePlatform.GetGamesByLocationNameAsync(locationName);
        if (game.Any())
        {
            return NotFound(new MessageDto(GameMessage.NotFoundByLocationName));
        }

        return game.ToDtoList();
    }

    [HttpDelete]
#if !DEBUG
[Authorize(Roles = "Admin")]
#endif
    [Route("Delete/{gameId}")]
    public async Task<IActionResult> DeleteGameByIdAsync([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
        {
            return NotFound(new MessageDto(GameMessage.NotFoundById));
        }

        await _gamePlatform.DeleteGameAsync(game);

        return Ok(new MessageDto(GameMessage.SuccesDeleted));
    }
}