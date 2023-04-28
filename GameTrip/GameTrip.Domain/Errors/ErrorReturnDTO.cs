using GameTrip.Domain.Models;

namespace GameTrip.Domain.Errors;

public class ErrorReturnDTO
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }

    public ErrorReturnDTO(StringEnumError error)
    {
        ErrorCode = error.Key;
        Message = error.Value;
    }
}