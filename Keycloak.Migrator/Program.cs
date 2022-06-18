// See https://aka.ms/new-console-template for more information
using Keycloak.Migrator;
using Keycloak.Migrator.Extensions;
using NLog;
using System.CommandLine;

var logger = LogManager.GetCurrentClassLogger();

logger.Info("Starting migrations application.");

try
{
    var rootCommand = new RootCommand("Migrates Keycloak Realm.");

    rootCommand.AddMigrateCommand();

    await rootCommand.InvokeAsync(args);
}
catch (Exception ex)
{
    // NLog: catch any exception and log it.
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    logger.Info("End Of Line");
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
