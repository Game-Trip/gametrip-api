using GameTrip.Domain.Models.Enum;

namespace GameTrip.Domain.Errors;
/// <summary>
/// The user message.
/// </summary>

public class UserMessage : StringEnumError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMessage"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="key">The key.</param>
    public UserMessage(string value, string key) : base(value, key)
    {
    }

    /// <summary>
    /// Gets the failed login.
    /// </summary>
    public static UserMessage FailedLogin => new("FailedLogin", "The login or the password is invalid");

    /// <summary>
    /// Gets the not found by id.
    /// </summary>
    public static UserMessage NotFoundById => new("NotFound", "No user found with provided ID");

    /// <summary>
    /// Gets the not found by user name.
    /// </summary>
    public static UserMessage NotFoundByUserName => new("NotFound", "No user found with provided UserName");

    /// <summary>
    /// Gets the not found by mail.
    /// </summary>
    public static UserMessage NotFoundByMail => new("NotFound", "No user found with provided Email");

    /// <summary>
    /// Gets the mail already exist.
    /// </summary>
    public static UserMessage MailAlreadyExist => new("MailAlreadyExist", "An account already exists with the Mail provided");

    /// <summary>
    /// Gets the user name already exist.
    /// </summary>
    public static UserMessage UserNameAlreadyExist => new("UserNameAlreadyExist", "An account already exists with the UserName provided");
}