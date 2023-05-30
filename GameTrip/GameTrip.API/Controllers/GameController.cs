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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Runtime.InteropServices;

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

    /// <summary>
    /// Create new Game
    /// </summary>
    /// <param name="dto">CreateGameDto</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("CreateGame")]
    public async Task<ActionResult<MessageDto>> CreateGame(CreateGameDto dto)
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

    /// <summary>
    /// Get All Games
    /// </summary>
    /// <param name="limit">Set the limit of number items return</param>
    [ProducesResponseType(typeof(List<ListGameDto>), (int)HttpStatusCode.OK)]
    [AllowAnonymous]
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<ListGameDto>>> GetGames([Optional][FromQuery] int limit)
    {
        IEnumerable<Game> games = await _gamePlatform.GetAllGamesAsync(limit);
        return games.ToList_ListGameDto();
    }

    /// <summary>
    /// Get Game by Id
    /// </summary>
    /// <param name="gameId">Id of Game</param>
    [ProducesResponseType(typeof(GameDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [AllowAnonymous]
    [HttpGet]
    [Route("Id/{gameId}")]
    public async Task<ActionResult<GameDto>> GetGameById([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
        {
            return NotFound(new MessageDto(GameMessage.NotFoundById));
        }

        return game.ToGameDto();
    }

    /// <summary>
    /// Get Game by Name
    /// </summary>
    /// <param name="gameName">Name of Game</param>
    [ProducesResponseType(typeof(GameDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return game.ToGameDto();
    }

    /// <summary>
    /// Get all Games by related location id
    /// </summary>
    /// <param name="locationId">Id of related location</param>
    [ProducesResponseType(typeof(List<ListGameDto?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return games.ToList_ListGameDto();
    }

    /// <summary>
    /// Get all Games by related location name
    /// </summary>
    /// <param name="locationName">Name of related location</param>
    [ProducesResponseType(typeof(List<ListGameDto?>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

        return game.ToList_ListGameDto();
    }

    /// <summary>
    /// Add Game to Location by Game Id and Location Id
    /// </summary>
    /// <param name="gameId">Id of added Game</param>
    /// <param name="locationId">Id of location to add Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

    /// <summary>
    /// Remove Game from Location by Game Id and Location Id
    /// </summary>
    /// <param name="gameId">Id of removed Game</param>
    /// <param name="locationId">Id of location to remove Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
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

    /// <summary>
    /// Update Game
    /// </summary>
    /// <param name="gameId">Id of game to update</param>
    /// <param name="dto">UpdateGameDto</param>
    [ProducesResponseType(typeof(GameDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpPut]
    [Route("{gameId}")]
    public async Task<ActionResult<GameDto>> UpdateGame([FromRoute] Guid gameId, [FromBody] UpdateGameDto dto)
    {
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
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        Game game = await _gamePlatform.UpdateGameAsync(entity, dto);
        return Ok(game.ToGameDto());
    }

    /// <summary>
    /// Delete Game by Id
    /// </summary>
    /// <param name="gameId">Id of deleted Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [HttpDelete]
    [Authorize(Roles = Roles.User)]
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