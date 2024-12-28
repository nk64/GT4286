using System.IO.Abstractions;
using System.Reflection;
using GT4286Util.CliCommands;
using GT4286Util.Entities;
using GT4286Util.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var temp1 = new IdentifySdCardCommand.Settings() { PathToSdCardRoot = "D:"};

            // Configuration
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();


            // Services
            var registrations = new ServiceCollection()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                });

            //var font = FigletFont.Load(ResourceReader.ReadManifestData("Spectre.Console/Widgets/Figlet/Fonts/Standard.flf"));
            //AnsiConsole.Write(new FigletText(FigletFont.Default, "GT4286 Util"));

            // A type registrar is an adapter for a DI framework.
            var registrar = new TypeRegistrar(registrations);

            var app = new CommandApp(registrar);

            var informationalVersionString = typeof(Game).Assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion!;

            app.Configure(config => {
                config.SetApplicationName("GT4286Util");
                config.SetApplicationVersion(informationalVersionString);
                //config.PropagateExceptions();

                config.AddCommand<IdentifySdCardCommand>("identify")
                    .WithDescription("Attempt to identify the generation of the SD Card (or backup folder)")
                    .WithExample("identify", @"D:")
                    .WithExample("identify", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("identify", @"/mnt/sdcard")
                    ;

                config.AddCommand<RefreshGameDbCommand>("refreshgamedb")
                    .WithDescription("Update the game.db with new and removed roms in /roms and /download")
                    .WithExample("refreshgamedb", @"D:")
                    .WithExample("refreshgamedb", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("refreshgamedb", @"/mnt/sdcard")
                    ;

                config.AddCommand<HumaniseArcadeNamesCommand>("humanisearcaderomnames")
                    .IsHidden()
                    .WithDescription("")
                    .WithExample("humanisearcaderomnames", @"D:")
                    .WithExample("humanisearcaderomnames", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("humanisearcaderomnames", @"/mnt/sdcard")
                    ;

                config.AddCommand<BrotliCompressCommand>("brotlicompress")
                    .IsHidden()
                    .WithDescription("Compress a file using Brotli compression")
                    ;

                config.AddCommand<GameDbExperimentCommand>("experiment")
                    .IsHidden()
                    .WithDescription("Perform an experiment")
                    .WithExample("experiment", "dumpbuiltingames")
                    ;

                // config.SetExceptionHandler((ex, resolver) =>
                // {
                //     AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                //     return -99;
                // });
            });

            return await app.RunAsync(args);

            //Experiments.Experiment1();
            //AnsiConsole.Markup("[underline red]Hello[/] World!");
            //Experiments.Experiment_CheckIfMAMEFavouriteGamesExist();
            //Experiments.Experiment_DumpGames();
        }
    }
}


