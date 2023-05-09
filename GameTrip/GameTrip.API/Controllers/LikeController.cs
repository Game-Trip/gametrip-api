using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.Extension;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.LikeModels.Game;
using GameTrip.Domain.Models.LikeModels.Location;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameTrip.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
    private readonly IValidator<AddLikeLocationDto> _addLikeLocationValidator;
    private readonly IValidator<AddLikeGameDto> _addLikeGameValidator;
    private readonly ILocationPlarform _locationPlaform;
    private readonly IGamePlatform _gamePlatfrom;
    private readonly UserManager<GameTripUser> _userManager;
    private readonly ILikePlatform _likePlatform;

    public LikeController(IValidator<AddLikeLocationDto> addLikeLocationValidator,
                          ILocationPlarform locationPlaform,
                          UserManager<GameTripUser> userManager,
                          ILikePlatform likePlatform,
                          IValidator<AddLikeGameDto> addLikeGameValidator,
                          IGamePlatform gamePlatfrom)
    {
        _addLikeLocationValidator = addLikeLocationValidator;
        _locationPlaform = locationPlaform;
        _userManager = userManager;
        _likePlatform = likePlatform;
        _addLikeGameValidator = addLikeGameValidator;
        _gamePlatfrom = gamePlatfrom;
    }

    #region LocationLike
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddLikeToLocation")]
    public async Task<ActionResult<LikedLocationDto>> LikeLocation([FromBody] AddLikeLocationDto dto)
    {
        ValidationResult validationResult = _addLikeLocationValidator.Validate(dto);
        if (validationResult.IsValid is false)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Location? location = await _locationPlaform.GetLocationByIdAsync((Guid)dto.LocationId!);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(dto.UserId!.ToString()!);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (location.LikedLocations!.Any(l => l.UserId == dto.UserId))
            return BadRequest(new MessageDto(LikeMessage.UserAlreadyLikeLocation));

        await _likePlatform.AddLikeToLocationAsync(location, user, (decimal)dto.Value);

        IEnumerable<LikedLocation> likedLocations = _likePlatform.GetAllLikedLocationByLocation(location);

        return Ok(likedLocations.ToDto());
    }

    /// <summary>
    /// Remove Like from Location
    /// </summary>
    /// return <see cref="LikedLocationDto"/>
    /// <response code = "204">Location provided did not have like from any users</response>
    /// 
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("RemoveLikeToLocation/{locationId}/{userId}")]
    public async Task<ActionResult<LikedLocationDto>> UnlikeLocation([FromRoute] Guid locationId, Guid userId)
    {
        Location? location = await _locationPlaform.GetLocationByIdAsync(locationId);
        if (location is null)
            return NotFound(new MessageDto(LocationMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (!location.LikedLocations!.Any(l => l.UserId == userId))
            return BadRequest(new MessageDto(LikeMessage.UserNotLikeLocation));

        await _likePlatform.RemoveLikeToLocationAsync(location, user);

        IEnumerable<LikedLocation> likedLocations = _likePlatform.GetAllLikedLocationByLocation(location);

        if (!likedLocations.Any())
            return NoContent();

        return Ok(likedLocations.ToDto());
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("LikedLocations/{userId}")]
    public async Task<ActionResult<IEnumerable<ListLikedLocationDto>>> GetAllLikedLocationByUserId([FromRoute] Guid userId)
    {
        GameTripUser? user = await _userManager.Users.Include(u => u.LikedLocations).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (!user.LikedLocations!.Any())
            return NotFound(UserMessage.NeverLikeAnyLocation);

        IEnumerable<LikedLocation> likedLocations = user.LikedLocations!.Select(ll => _likePlatform.GetLikeLocation(ll));

        return Ok(likedLocations.ToListLikedLocationDto());
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("AllLikedLocations")]
    public async Task<ActionResult<IEnumerable<LikedLocationDto>>> GetAllLikedLocation()
    {
        IEnumerable<LikedLocation> likedLocations = await _likePlatform.GetAllLikedLocationIncludeAllAsync();

        IEnumerable<IGrouping<Guid, LikedLocation>> likedLocationGroupByLocation = likedLocations.GroupBy(ll => ll.LocationId);
        List<LikedLocationDto> likedLocationDtos = new();

        foreach (IGrouping<Guid, LikedLocation> group in likedLocationGroupByLocation)
        {
            LikedLocationDto likedLocationDto = new()
            {
                LocationId = group.Key,
                Location = group.First().Location.ToLocationNameDto(),
                UsersIds = group.Select(ll => ll.UserId).AsEnumerable(),
                NbVote = group.Count(),
                MaxValue = group.OrderBy(ll => ll.Vote).First().Vote,
                MinValue = group.OrderByDescending(ll => ll.Vote).First().Vote,
                AverageValue = group.Average(ll => ll.Vote)
            };
            likedLocationDtos.Add(likedLocationDto);
        }

        return Ok(likedLocationDtos.AsEnumerable());
    }
    #endregion

    #region GameLike
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("AddLikeToGame")]
    public async Task<ActionResult<LikedLocationDto>> LikeGame([FromBody] AddLikeGameDto dto)
    {
        ValidationResult validationResult = _addLikeGameValidator.Validate(dto);
        if (validationResult.IsValid is false)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Game? game = await _gamePlatfrom.GetGameByIdAsync((Guid)dto.GameId!);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(dto.UserId!.ToString()!);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (game.LikedGames!.Any(l => l.UserId == dto.UserId))
            return BadRequest(new MessageDto(LikeMessage.UserAlreadyLikeGame));

        await _likePlatform.AddLikeToGameAsync(game, user, (decimal)dto.Value!);

        IEnumerable<LikedGame> likedGames = _likePlatform.GetAllLikedGameByGame(game);

        return Ok(likedGames.ToDto());
    }

    /// <summary>
    /// Remove Like from Location
    /// </summary>
    /// return <see cref="LikedLocationDto"/>
    /// <response code = "204">Game provided did not have like from any users</response>
    /// 
    [Authorize(Roles = Roles.User)]
    [HttpPost]
    [Route("RemoveLikeToGame/{gameId}/{userId}")]
    public async Task<ActionResult<LikedLocationDto>> UnlikeGame([FromRoute] Guid gameId, Guid userId)
    {
        Game? game = await _gamePlatfrom.GetGameByIdAsync(gameId);
        if (game is null)
            return NotFound(new MessageDto(GameMessage.NotFoundById));

        GameTripUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (!game.LikedGames!.Any(l => l.UserId == userId))
            return BadRequest(new MessageDto(LikeMessage.UserNotLikeGame));

        await _likePlatform.RemoveLikeToGameAsync(game, user);

        IEnumerable<LikedGame> likedGames = _likePlatform.GetAllLikedGameByGame(game);

        if (!likedGames.Any())
            return NoContent();

        return Ok(likedGames.ToDto());
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("LikedGames/{userId}")]
    public async Task<ActionResult<IEnumerable<ListLikedLocationDto>>> GetAllLikedGamebyUserId([FromRoute] Guid userId)
    {
        GameTripUser? user = await _userManager.Users.Include(u => u.LikedGames).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            return NotFound(new MessageDto(UserMessage.NotFoundById));

        if (!user.LikedGames!.Any())
            return NotFound(UserMessage.NeverLikeAnyGame);

        IEnumerable<LikedGame> likedGames = user.LikedGames!.Select(lg => _likePlatform.GetLikeGame(lg));

        return Ok(likedGames.ToListLikedGameDto());
    }

    [Authorize(Roles = Roles.User)]
    [HttpGet]
    [Route("AllLikedGames")]
    public async Task<ActionResult<IEnumerable<LikedGameDto>>> GetAllLikedGames()
    {
        IEnumerable<LikedGame> likedGames = await _likePlatform.GetAllLikedGameIncludeAllAsync();

        IEnumerable<IGrouping<Guid, LikedGame>> likedLocationGroupByLocation = likedGames.GroupBy(lg => lg.GameId);
        List<LikedGameDto> likedGameDtos = new();

        foreach (IGrouping<Guid, LikedGame> group in likedLocationGroupByLocation)
        {
            LikedGameDto likedGameDto = new()
            {
                GameId = group.Key,
                Game = group.First().Game.ToGameNameDto(),
                UsersIds = group.Select(ll => ll.UserId).AsEnumerable(),
                NbVote = group.Count(),
                MaxValue = group.OrderBy(ll => ll.Vote).First().Vote,
                MinValue = group.OrderByDescending(ll => ll.Vote).First().Vote,
                AverageValue = group.Average(ll => ll.Vote)
            };
            likedGameDtos.Add(likedGameDto);
        }

        return Ok(likedGameDtos.AsEnumerable());
    }
    #endregion
}
