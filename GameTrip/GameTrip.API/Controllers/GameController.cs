using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using GameTrip.Domain.Entities;
using GameTrip.Domain.HttpMessage;
using GameTrip.Domain.Models.GameModels;
using GameTrip.Platform.IPlatform;
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
    private readonly IValidator<CreateGameDto> _createGameValidator;

    public GameController(IValidator<CreateGameDto> createGameValidator, IGamePlatform gamePlatform)
    {
        _createGameValidator = createGameValidator;
        _gamePlatform = gamePlatform;
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("Game")]
    public async Task<ActionResult<Game>> CreateGame(CreateGameDto dto)
    {
        ValidationResult result = _createGameValidator.Validate(dto);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        Game? game = await _gamePlatform.GetGameByNameAsync(dto.Name);
        if (location is not null)
            return BadRequest(new MessageDto(LocationMessage.AlreadyExistByName));

        await _gamePlatform.CreateGameAsync(dto.ToEntity());
        return Ok();
    }
}