using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Domain.Models.LocationModels;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<ActionResult<MessageDto>> CreateGame([FromBody] CreateGameDto dto, [Optional][FromQuery] bool force)
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

        await _gamePlatform.CreateGameAsync(dto.ToEntity(force));
        return new MessageDto(GameMessage.SuccesCreated);
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
    public async Task<ActionResult<GameDto>> GetGameByName([FromRoute] string gameName)
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
    /// Create request to Add Game to Location by Game Id and Location Id
    /// </summary>
    /// <param name="gameId">Id of added Game</param>
    /// <param name="locationId">Id of location to add Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("RequestAddGameToLocation/Game/{gameId}/Location/{locationId}")]
    public async Task<ActionResult<MessageDto>> CreateRequestToAddGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        if (location.Games!.Contains(game))
            return BadRequest(new MessageDto(LocationMessage.AlreadyContainGameById));

        LocationUpdateRequestDto locationUpdateRequestDto = new()
        {
            LocationId = locationId,
            IdGame = gameId,
            Game = game,
            isAddedGame = true,
        };

        await _gamePlatform.RequestToAddOrRemoveGameToLocationByIdAsync(locationUpdateRequestDto.ToEntity());
        return new MessageDto(GameMessage.RequestAddToLocationSuccess);
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
    public async Task<ActionResult<MessageDto>> AddGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
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
        return new MessageDto(GameMessage.AddedToLocation);
    }

    /// <summary>
    /// Create request to remove Game from Location by Game Id and Location Id
    /// </summary>
    /// <param name="gameId">Id of removed Game</param>
    /// <param name="locationId">Id of location to remove Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("RequestToRemoveGameToLocation/Game/{gameId}/Location/{locationId}")]
    public async Task<ActionResult<MessageDto>> CreateRequestRemoveGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        Location? location = await _locationPlatform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        if (!location.Games!.Contains(game))
            return BadRequest(new MessageDto(LocationMessage.NotContainGameById));

        ICollection<Game> newGameList = location.Games;
        newGameList.Remove(game);

        LocationUpdateRequestDto locationUpdateRequestDto = new()
        {
            LocationId = locationId,
            IdGame = gameId,
            Game = game,
            isAddedGame = false
        };
        await _gamePlatform.RequestToAddOrRemoveGameToLocationByIdAsync(locationUpdateRequestDto.ToEntity());
        return new MessageDto(GameMessage.RequestRemoveToLocationSuccess);
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
    public async Task<ActionResult<MessageDto>> RemoveGameToLocationById([FromRoute] Guid gameId, [FromRoute] Guid locationId)
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
        return new MessageDto(GameMessage.RemovedToLocation);
    }

    /// <summary>
    /// Make a request to update a game
    /// </summary>
    /// <param name="gameId">Id of game to request an update</param>
    /// <param name="dto">GameUpdateRequestDto</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("{gameId}")]
    public async Task<ActionResult<MessageDto>> CreateUpdateRequest([FromRoute] Guid gameId, [FromBody] GameUpdateRequestDto dto)
    {
        if (gameId != dto.GameId)
            return BadRequest(new MessageDto(GameMessage.IdWithQueryAndDtoAreDifferent));

        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
            return BadRequest(new MessageDto(GameMessage.NotFoundById));

        await _gamePlatform.CreateUpdateRequestAsync(dto.ToEntity());
        return new MessageDto(GameMessage.GameUpdateRequestSuccess);
    }

    /// <summary>
    /// Get game with all request update
    /// </summary>
    /// <param name="gameId">Id of game wanted</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(NotFound), (int)HttpStatusCode.BadRequest)]
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    [Route("Request_Update/{gameId}")]
    public async Task<ActionResult<ListGameUpdateRequest>> GetGameWithAllRequestUpdate([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameWithRequestUpdateAsync(gameId);

        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        if (game.RequestGameUpdates is null || !game.RequestGameUpdates.Any())
            return BadRequest(new MessageDto(GameMessage.NotFoundUpdateRequest));

        return game.ToListGameUpdateRequest();
    }

    /// <summary>
    /// Update Game
    /// </summary>
    /// <param name="gameId">Id of game to update</param>
    /// <param name="dto">UpdateGameDto</param>
    /// <param name="requestUpdateId">If used, this means that the update is performed following validation of a request</param>
    [ProducesResponseType(typeof(GameDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ModelStateDictionary), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [Authorize(Roles = Roles.Admin)]
    [HttpPut]
    [Route("{gameId}")]
    public async Task<ActionResult<GameDto>> UpdateGame([FromRoute] Guid gameId, [FromBody] UpdateGameDto dto, [Optional][FromQuery] Guid? requestUpdateId)
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

        if (requestUpdateId is not null)
            await _gamePlatform.DeleteUpdateGameRequestAsync(requestUpdateId);
        return game.ToGameDto();
    }

    /// <summary>
    /// Delete Game by Id
    /// </summary>
    /// <param name="gameId">Id of deleted Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("Delete/{gameId}")]
    public async Task<ActionResult<MessageDto>> DeleteGameById([FromRoute] Guid gameId)
    {
        Game? game = await _gamePlatform.GetGameByIdAsync(gameId);
        if (game is null)
        {
            return NotFound(new MessageDto(GameMessage.NotFoundById));
        }

        await _gamePlatform.DeleteGameAsync(game);

        return new MessageDto(GameMessage.SuccesDeleted);
    }

    /// <summary>
    /// Request Update Game by Id
    /// </summary>
    /// <param name="requestUpdateId">Id of request UpdateId Game</param>
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(MessageDto), (int)HttpStatusCode.NotFound)]
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    [Route("DeleteRequestUpdate/{requestUpdateId}")]
    public async Task<ActionResult<MessageDto>> DeleteRequestUpdateIdGameById([FromRoute] Guid requestUpdateId)
    {
        RequestGameUpdate? requestGameUpdate = await _gamePlatform.GetRequestUpdateGameByIdAsync(requestUpdateId);
        if (requestGameUpdate is null)
        {
            return NotFound(new MessageDto(GameMessage.RequestUpdateNotFoundById));
        }

        await _gamePlatform.DeleteRequestGameUpdateAsync(requestGameUpdate);

        return new MessageDto(GameMessage.RequestUpdateSuccesDeleted);
    }
}