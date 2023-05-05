using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.HttpMessage;
/// <summary>
/// The error dto.
/// </summary>

public class MessageDto
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string MessageCode { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDto"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    public MessageDto(StringEnumError error)
    {
        MessageCode = error.Key;
        Message = error.Message;
    }
}