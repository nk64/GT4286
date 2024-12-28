using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class IdentifySdCardCommand : AsyncCommand<IdentifySdCardCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<pathtosdcard>")]
            public required string PathToSdCardRoot { get; set; }

            [CommandOption("--verbose")]
            public bool Verbose { get; set; }
        }

        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public IdentifySdCardCommand(ILogger<IdentifySdCardCommand> logger, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.WriteLine("Identifying SD Card...");

            AnsiConsole.WriteLine();

            AnsiConsole.WriteLine($"Checking /ui/gamelist.txt");
            string gameListPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "ui", "gamelist.txt");
            int gameListCount = 0;
            if (_fileSystem.File.Exists(gameListPath))
            {
                var lines = _fileSystem.File.ReadAllLines(gameListPath);
                gameListCount = lines.Length;
                //AnsiConsole.WriteLine($"/ui/gamelist.txt contains {gameListCount} lines");
            }

            AnsiConsole.WriteLine($"Checking /game.db");
            string gameDbPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "game.db");
            int gameDbBuiltinCount = 0;
            if (_fileSystem.File.Exists(gameDbPath))
            {
                var gameDbManager = new GameDbManager(_fileSystem, gameDbPath);

                var builtinGames = gameDbManager.GetBuiltinGames();
                gameDbBuiltinCount = builtinGames.Count;
                //AnsiConsole.WriteLine($"/game.db contains {gameDbBuiltinCount} builtin games");
            }

            AnsiConsole.WriteLine($"Checking /roms");
            string romDirPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "roms");

            Dictionary<string, int> romdirFileCountsDict = new Dictionary<string, int>();

            int totalRomCount = 0;

            foreach(var dir in _fileSystem.Directory.EnumerateDirectories(romDirPath))
            {
                var countOfRoms = _fileSystem.Directory.EnumerateFiles(dir).Count();

                string reldir = $"/{Path.GetFileName(romDirPath)}/{Path.GetFileName(dir)}";

                romdirFileCountsDict.Add(reldir, countOfRoms);

                //AnsiConsole.WriteLine($"{reldir} contains {countOfRoms} files");
                totalRomCount += countOfRoms;
            }

            {
                string reldir = $"/{Path.GetFileName(romDirPath)}";
                //AnsiConsole.WriteLine($"{reldir} contains a total of {totalRomCount} files");
            }

            string[] keyFileList = [
                _fileSystem.Path.Combine(@"ui/gamelist.txt"),
                _fileSystem.Path.Combine(@"bin/gmenu2x"),
                _fileSystem.Path.Combine(@"emus/fbneo/fbneo"),
                _fileSystem.Path.Combine(@"emus/fceux/fceux"),
                _fileSystem.Path.Combine(@"emus/gamebatte/gambatte_sdl"),
                _fileSystem.Path.Combine(@"emus/gbc/gambatte_sdl"),
                _fileSystem.Path.Combine(@"emus/gpsp/gpsp"),
                _fileSystem.Path.Combine(@"emus/mame/fbneo"),
                _fileSystem.Path.Combine(@"emus/pcsx4all/pcsx"),
                _fileSystem.Path.Combine(@"emus/picodrive/PicoDrive"),
                _fileSystem.Path.Combine(@"emus/snes9x4d/snes9x4d.dge"),
                _fileSystem.Path.Combine(@"emus/temper/temper"),
            ];

            Dictionary<string, string> keyFileHashDict = new Dictionary<string, string>();

            var hashHelper = new HashHelper(_fileSystem);

            foreach(string keyFile in keyFileList)
            {
                AnsiConsole.WriteLine($"Calculating hash of: {keyFile}");
                string keyFilePath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, keyFile);
                string hashString = hashHelper.GetFileHash(keyFilePath);

                keyFileHashDict.Add(keyFile, hashString);

                //AnsiConsole.WriteLine($"/{keyFile} Hash: {hashString}");
            }

            var generationInfo = new GenerationInfo()
            {
                GenerationId = GenerationId.Unknown,
                ApproximateDateDetails = "unknown",
                Title = "unknown",
                Notes = "unknown",
                GameListCount = gameListCount,
                GameDbBuiltinCount = gameDbBuiltinCount,
                KeyFileHashes = keyFileHashDict,
                RomDirFileCounts = romdirFileCountsDict,
                TotalRomCount = totalRomCount
            };

            AnsiConsole.WriteLine();
            GenerationInfo? matchedGenerationInfo = GenerationInfoHelper.FindMatchingGenerationInfo(generationInfo);

            if (matchedGenerationInfo != null)
            {
                
                AnsiConsole.MarkupLineInterpolated($"This SD Card appears to be one of the known SD Card Generations: [green]{matchedGenerationInfo.GenerationId}[/]");
                if (settings.Verbose)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLineInterpolated($"{SerializationHelper.SerializeToJsonString(matchedGenerationInfo)}");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Unknown SD Card Generation. If this is a stock SD Card, please join the discussions at https://github.com/nk64/GT4286/discussions and post these details[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLineInterpolated($"{SerializationHelper.SerializeToJsonString(generationInfo)}");
            }

            return Task.FromResult(0);
        }

        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (_fileSystem.Directory.Exists(settings.PathToSdCardRoot) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToSdCardRoot}");
            }

            return base.Validate(context, settings);
        }
    }
}

