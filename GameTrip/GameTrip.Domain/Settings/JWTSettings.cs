namespace GameTrip.Domain.Settings;

public class JWTSettings
{
    #region Properties
    public string Secret { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty; 
    public int DurationTime { get; set; }

    #endregion
}
