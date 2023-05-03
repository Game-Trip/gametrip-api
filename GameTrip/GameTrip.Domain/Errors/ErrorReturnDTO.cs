using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Errors;

public class ErrorResultDTO
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }

    public ErrorResultDTO(StringEnumError error)
    {
        ErrorCode = error.Key;
        Message = error.Message;
    }
}