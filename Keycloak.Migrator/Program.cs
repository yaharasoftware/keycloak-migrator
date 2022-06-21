// See https://aka.ms/new-console-template for more information
using Keycloak.Migrator;
using Keycloak.Migrator.Extensions;
using NLog;
using System;
using System.CommandLine;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Logger? logger = LogManager.GetCurrentClassLogger();

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
    }
}
