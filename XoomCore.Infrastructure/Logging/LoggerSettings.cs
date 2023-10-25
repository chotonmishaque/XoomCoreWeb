namespace XoomCore.Infrastructure.Logging;


public class LoggerSettings
{
    public string AppName { get; set; } = "XoomCoreWeb";
    public bool WriteToFile { get; set; } = true;
    public bool WriteToDb { get; set; } = true;
    //public bool WriteToMSSQL { get; set; } = true;
    public string MinimumLogLevel { get; set; } = "Information";
}

