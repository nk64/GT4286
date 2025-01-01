using System.IO.Abstractions;
using GT4286Util.CliCommands;
using GT4286Util.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            app.Configure(config => {
                config.SetApplicationName("GT4286Util");
                
                config.UseAssemblyInformationalVersion();

                config.Settings.MaximumIndirectExamples = 50;

                config.AddCommand<IdentifySdCardCommand>("identify-card")
                    .WithDescription("Attempt to identify the generation of the SD Card (or backup folder)")
                    .WithExample("identify-card", @"D:")
                    .WithExample("identify-card", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("identify-card", @"/mnt/sdcard")
                    ;

                config.AddCommand<AuditSdCardCommand>("audit-card")
                    .IsHidden()
                    .WithDescription("Audit each file on the SD Card (or backup folder)")
                    .WithExample("audit-card", @"D:")
                    .WithExample("audit-card", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("audit-card", @"/mnt/sdcard")
                    ;

                config.AddCommand<RefreshGameDbCommand>("refresh-gamedb")
                    .WithDescription("Update the game.db with new and removed roms in /roms and /download. The default ordering mimics the original ordering which is first by class (in some weird order) then by name. Use --alphabetical to sort the entire list alphabetically")
                    .WithExample("refresh-gamedb", @"D:")
                    .WithExample("refresh-gamedb", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("refresh-gamedb", @"/mnt/sdcard")
                    ;

                config.AddCommand<HumaniseArcadeNamesCommand>("humanise-game-names")
                    .WithDescription("Upgrade FBNeo roms (built-in only at this stage) from cryptic 8 character file names to human readble ones")
                    .WithExample("humanise-game-names", @"D:")
                    .WithExample("humanise-game-names", @"C:\RetroControllerCardBackups\SDCARD1")
                    .WithExample("humanise-game-names", @"/mnt/sdcard")
                    ;

                config.AddCommand<BrotliCompressCommand>("brotli-compress")
                    .IsHidden()
                    .WithDescription("Compress a file using Brotli compression")
                    ;

                config.AddCommand<GeneratePatchCommand>("generate-patch")
                    .WithDescription("Create a patch file")
                    .WithExample("generate-patch",
                        @"D:\emus\fbneo\fbneo",
                        @"D:\emus\fbneo\fbneo.patched_for_abc",
                        @"D:\emus\fbneo\fbneo.fix_abc.patch"
                    )
                    ;

                config.AddCommand<ApplyPatchCommand>("apply-patch")
                    .WithDescription("apply a patch to a file")
                    .WithExample("apply-patch",
                        @"D:\emus\fbneo\fbneo.fix_abc.patch",
                        @"D:\emus\fbneo\fbneo",
                        @"D:\emus\fbneo\fbneo.newly_patched_for_abc"
                    )
                    ;

                config.AddCommand<GameDbExperimentCommand>("experiment")
                    .IsHidden()
                    .WithDescription("Perform an experiment")
                    .WithExample("experiment", "dumpbuiltingames")
                    ;

                #if DEBUG
                //config.PropagateExceptions();
                config.ValidateExamples();
                #endif

                // config.SetExceptionHandler((ex, resolver) =>
                // {
                //     AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                //     return -99;
                // });
            });

            return await app.RunAsync(args);
        }
    }
}


