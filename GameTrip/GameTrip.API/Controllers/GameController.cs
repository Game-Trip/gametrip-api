using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameTrip.API.Controllers;

[Route("[controller]")]
[Authorize]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGamePlatform _gamePlatform;
    private readonly ILocationPlarform _locationPlatform;
    private readonly IValidator<CreateGameDto> _createGameValidator;
    private readonly IValidator<UpdateGameDto> _updateGameValidator;

    public GameController(IValidator<CreateGameDto> createGameValidator, IGamePlatform gamePlatform, ILocationPlarform locationPlarform, IValidator<UpdateGameDto> updateGameValidator)
    {
        _createGameValidator = createGameValidator;
        _gamePlatform = gamePlatform;
        _locationPlatform = locationPlarform;
        _updateGameValidator = (IValidator<UpdateGameDto>?)updateGameValidator;
    }

    [Authorize(Roles = Roles.User)]
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
    public async Task<ActionResult<List<ListGameDto>>> GetGames()
    {
        IEnumerable<Game> games = await _gamePlatform.GetAllGamesAsync();
        return games.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Id/{gameId}")]
    public async Task<ActionResult<GameDto>> GetGaleById([FromRoute] Guid gameId)
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
    public async Task<ActionResult<GameDto>> GetLocationByName([FromRoute] string gameName)
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
    public async Task<ActionResult<List<ListGameDto?>>> GetGamesByLocationId([FromRoute] Guid locationId)
    {
        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        IEnumerable<Game?> games = await _gamePlatform.GetGamesByLocationIdAsync(location.IdLocation);
        if (!games.Any())
            return NotFound(new MessageDto(GameMessage.NotFoundByLocationId));

        return games.ToDtoList();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("Location/Name/{locationName}")]
    public async Task<ActionResult<List<ListGameDto?>>> GetGamesByLocationName([FromRoute] string locationName)
    {
        Location? location = await _locationPlatform.GetLocationByNameAsync(locationName);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundByName));

        IEnumerable<Game?> game = await _gamePlatform.GetGamesByLocationNameAsync(locationName);
        if (!game.Any())
        {
            return NotFound(new MessageDto(GameMessage.NotFoundByLocationName));
        }

        return game.ToDtoList();
    }

    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddGameToLocation/Game/{gameId}/Location/{locationId}")]
    public async Task<IActionResult> AddGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        if (location.Games!.Contains(game))
            return BadRequest(new MessageDto(LocationMessage.AlreadyContainGameById));

        await _gamePlatform.AddGameToLocationByIdAsync(game, location);
        return Ok(new MessageDto(GameMessage.AddedToLocation));
    }

    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("RemoveGameToLocation/Game/{gameId}/Location/{locationId}")]
    public async Task<IActionResult> RemoveGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        if (!location.Games!.Contains(game))
            return BadRequest(new MessageDto(LocationMessage.NotContainGameById));

        await _gamePlatform.RemoveGameToLocationByIdAsync(game, location);
        return Ok(new MessageDto(GameMessage.RemovedToLocation));
    }

    //Todo à test le sort
    [AllowAnonymous]
    [HttpGet]
    [Route("SortLikedCount")]
    public async Task<ActionResult<List<ListGameDto>>> GetGameSortByLikeCount([FromQuery] int? limit, [FromQuery] bool? asc = true)
    {
        IEnumerable<Game> games = await _gamePlatform.GetAllGamesAsync();
        if (limit is not null)
            games = _gamePlatform.LimitList(games, (int)limit);

        games.OrderBy(g => g.LikedGames.Count());

        if (asc is false)
            games.Reverse();

        return games.ToDtoList();
    }

    //Todo à test le sort
    [AllowAnonymous]
    [HttpGet]
    [Route("SortLikedValue")]
    public async Task<ActionResult<List<ListGameDto>>> GetGameSortByLikeScore([FromQuery] int? limit, [FromQuery] bool? asc = true)
    {
        IEnumerable<Game> games = await _gamePlatform.GetAllGamesAsync();
        if (limit is not null)
            games = _gamePlatform.LimitList(games, (int)limit);

        games = _gamePlatform.SortLikedGamesByScore(games);

        if (asc is false)
            games.Reverse();

        return games.ToDtoList();
    }

    [Authorize(Roles = Roles.User)]
    [HttpPut]
    [Route("{gameId}")]
    public async Task<ActionResult<GameDto>> UpdateGame([FromRoute] Guid gameId, [FromBody] UpdateGameDto dto)
    {
        System.Security.Claims.ClaimsPrincipal uwu = HttpContext.User;
        ValidationResult result = _updateGameValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        if (gameId != dto.gameId)
            return BadRequest(new MessageDto(GameMessage.IdWithQueryAndDtoAreDifferent));

        Game? entity = await _gamePlatform.GetGameByNameAsync(dto.Name);
        if (entity is null)
            return BadRequest(new MessageDto(GameMessage.NotFoundById));

        Game game = await _gamePlatform.UpdateGameAsync(entity, dto);
        return Ok(game.ToDto());
    }

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("Delete/{gameId}")]
    public async Task<IActionResult> DeleteGameById([FromRoute] Guid gameId)
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