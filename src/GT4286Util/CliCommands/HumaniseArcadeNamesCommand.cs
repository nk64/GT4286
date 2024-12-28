using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class HumaniseArcadeNamesCommand : AsyncCommand<HumaniseArcadeNamesCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<pathtosdcard>")]
            public required string PathToSdCardRoot { get; set; }
        }

        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public HumaniseArcadeNamesCommand(ILogger<HumaniseArcadeNamesCommand> logger, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            //var FBNeoGameDescriptionDictionary = FBNeoDatHelper.GetArcadeGameDescriptionDictionary();
            AnsiConsole.WriteLine("Humanising Arcade Rom Names...");
            AnsiConsole.MarkupLine("[red]TODO[/]");
            //FBNeoGameDescriptionDictionary.Dump();
            //Experiment_RedirectArcadeRomsWithHumanReadableNames(settings.PathToSdCardRoot);
            return Task.FromResult(0);
        }

        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (_fileSystem.Directory.Exists(settings.PathToSdCardRoot) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToSdCardRoot}");
            }

            string gameDbPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "game.db");
            if (_fileSystem.File.Exists(gameDbPath) == false)
            {
                return ValidationResult.Error($"GameDb not found in {settings.PathToSdCardRoot}");
            }

            return base.Validate(context, settings);
        }

        public static void Experiment_RedirectArcadeRomsWithHumanReadableNames(string retroArcadeBasePath)
        {
            var FBNeoGameDescriptionDictionary = FBNeoDatHelper.GetArcadeGameDescriptionDictionary();

            ISet<string> validRomFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".zip", ".7z" };

            var arcadeRomFiles = Directory
                .EnumerateFiles(Path.Combine(retroArcadeBasePath, "roms", "FBA", "redir"), "*")
                .Where(filePath=>validRomFileExtensions.Contains(Path.GetExtension(filePath)))
                .ToList();
            
            foreach (var filePath in arcadeRomFiles)
            {
                string gameRom = Path.GetFileNameWithoutExtension(filePath).ToLowerInvariant();
                string fileName = Path.GetFileName(filePath);

                if (FBNeoGameDescriptionDictionary.ContainsKey(gameRom))
                {
                    (string description, string sanetised_description) = FBNeoGameDescriptionDictionary[gameRom];
                
                    if (false && description != sanetised_description)
                    {
                        Console.WriteLine($"{gameRom,-10}: {description,-80}: {sanetised_description,-80}");
                    }

                    string redirstring = $"{fileName}";
                    string redirFilePath = Path.Combine(retroArcadeBasePath, "roms", "FBA", $"{sanetised_description}.redir");

                    if (File.Exists(redirFilePath))
                    {
                        if (File.ReadAllText(redirFilePath) != redirstring)
                        {
                            Console.WriteLine($"Redir file exists: {redirFilePath}, Updating contents to: {redirstring}");
                            File.WriteAllText(redirFilePath, redirstring);
                        } else {
                            Console.WriteLine($"Redir file exists: {redirFilePath}, Contents are good");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Writing {redirFilePath}");
                        File.WriteAllText(redirFilePath, redirstring);
                    }
                }
            }

            foreach (var filePath in arcadeRomFiles)
            {
                string gameRom = Path.GetFileNameWithoutExtension(filePath).ToLowerInvariant();
                string fileName = Path.GetFileName(filePath);

                if (FBNeoGameDescriptionDictionary.ContainsKey(gameRom))
                {
                    (string description, string sanetised_description) = FBNeoGameDescriptionDictionary[gameRom];

                    string redirFilePath = Path.Combine(retroArcadeBasePath, "roms", "FBA", $"{sanetised_description}.redir");
                    string descriptionBasedImagePath = Path.Combine(retroArcadeBasePath, "roms", "FBA", "image", $"{sanetised_description}.jpg");
                    string gameRomBasedImagePath = Path.Combine(retroArcadeBasePath, "roms", "FBA", "image", $"{gameRom}.jpg");

                    if (File.Exists(descriptionBasedImagePath))
                    {
                        Console.WriteLine($"Description based image exists: {descriptionBasedImagePath}");
                    } else {
                        if (File.Exists(gameRomBasedImagePath))
                        {
                            Console.WriteLine($"Renaming GameRom based image exists: {gameRomBasedImagePath}, renaming it to: {descriptionBasedImagePath}");
                            File.Move(gameRomBasedImagePath, descriptionBasedImagePath);
                        }
                        else {
                            Console.WriteLine($"No image found: {gameRomBasedImagePath} / {descriptionBasedImagePath}");
                        }
                    }
                }
            }
        }
    }
}
