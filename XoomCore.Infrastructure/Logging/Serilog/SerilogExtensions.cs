using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using XoomCore.Infrastructure.Persistence;

namespace XoomCore.Infrastructure.Logging.Serilog;

public static class SerilogExtensions
{
    public static void RegisterSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<LoggerSettings>().BindConfiguration(nameof(LoggerSettings));

        builder.Host.UseSerilog((_, sp, serilogConfig) =>
        {
            var loggerSettings = sp.GetRequiredService<IOptions<LoggerSettings>>().Value;
            var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            string appName = loggerSettings.AppName;
            bool writeToFile = loggerSettings.WriteToFile;
            bool writeToDb = loggerSettings.WriteToDb;
            //bool writeToMSSQL = loggerSettings.WriteToMSSQL;
            string minLogLevel = loggerSettings.MinimumLogLevel;
            ConfigureEnrichers(serilogConfig, appName);
            ConfigureWriteToFile(serilogConfig, writeToFile);
            ConfigureWriteToDb(serilogConfig, writeToDb, databaseSettings);
            //ConfigureWriteToMSSQL(serilogConfig, databaseSettings.ConnectionString, writeToMSSQL);

            SetMinimumLogLevel(serilogConfig, minLogLevel);
            OverideMinimumLogLevel(serilogConfig);
        });
    }

    private static void ConfigureEnrichers(LoggerConfiguration serilogConfig, string appName)
    {
        serilogConfig
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", appName);
    }

    private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile)
    {
        if (writeToFile)
        {
            serilogConfig.WriteTo.File(
             new CompactJsonFormatter(),
             "Logs/logs.json",
             restrictedToMinimumLevel: LogEventLevel.Information,
             rollingInterval: RollingInterval.Day,
             retainedFileCountLimit: 5);
        }
    }

    private static void ConfigureWriteToDb(LoggerConfiguration serilogConfig, bool writeToDb, DatabaseSettings databaseSettings)
    {
        if (writeToDb)
        {
            switch (databaseSettings.DBProvider?.ToLowerInvariant())
            {
                case DbProviderKeys.SqlServer:
                    serilogConfig.WriteTo.MSSqlServer(
                    connectionString: databaseSettings.ConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "_Logs", // Replace with the name of your log table
                        AutoCreateSqlTable = true, // If the table doesn't exist, create it
                    });
                    break;
                default: throw new InvalidOperationException($"DB Provider {databaseSettings.DBProvider} is not supported.");
            }

        }
    }
    private static void ConfigureWriteToMSSQL(LoggerConfiguration serilogConfig, string connectionString, bool writeToMSSQL)
    {
        if (writeToMSSQL)
        {
            serilogConfig.WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "_Logs", // Replace with the name of your log table
                    AutoCreateSqlTable = true, // If the table doesn't exist, create it
                });
        }
    }

    private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
    {
        serilogConfig
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("MongoDB.Driver", LogEventLevel.Warning)
        .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
    }

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        switch (minLogLevel.ToLower())
        {
            case "debug":
                serilogConfig.MinimumLevel.Debug();
                break;
            case "information":
                serilogConfig.MinimumLevel.Information();
                break;
            case "warning":
                serilogConfig.MinimumLevel.Warning();
                break;
            default:
                serilogConfig.MinimumLevel.Information();
                break;
        }
    }
}
