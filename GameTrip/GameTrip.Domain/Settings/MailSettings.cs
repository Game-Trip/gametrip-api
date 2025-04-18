﻿namespace GameTrip.Domain.Settings;

public class MailSettings
{
    public string? Server { get; set; }
    public int Port { get; set; }
    public string? SenderMail { get; set; }
    public string? SenderName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}