using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Entities;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class GameDbExperimentCommand : AsyncCommand<GameDbExperimentCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<pathtosdcard>")]
            public required string PathToSdCardRoot { get; set; }

            [Description("The experiment name")]
            [CommandArgument(0, "[experimentname]")]
            public string? ExperimentName { get; set; }

            // [Description("Perform the experiment without making any changes and show what would happen")]
            // [CommandOption("-d|--dry-run")]
            // [DefaultValue(true)]
            // public bool DryRun { get; set; }
        }

        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        ISet<string> KnownExperiments = new HashSet<string>() { "dumpbuiltingames", };

        public GameDbExperimentCommand(ILogger<GameDbExperimentCommand> logger, IFileSystem fileSystem)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public override Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            string? experimaneName = settings.ExperimentName;

            if (experimaneName == null)
            {
                experimaneName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which [green]experiment[/] would you like to run?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more experiments)[/]")
                        .AddChoices(KnownExperiments.ToArray())
                    );
            }

            switch(experimaneName)
            {
                case "dumpbuiltingames":
                    var retroArcadeGameDbPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "game.db");
                    var gameDbManager = new GameDbManager(_fileSystem, retroArcadeGameDbPath);
                    List<Game> games = gameDbManager.GetBuiltinGames();
                    games.Dump("Games");
                    break;

                default:
                    return Task.FromResult(1);
            }
            
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

            if (settings.ExperimentName != null)
            {
                if (KnownExperiments.Contains(settings.ExperimentName) == false)
                {
                    return ValidationResult.Error($"'{settings.ExperimentName}' is not a known experiment name");
                }
            }

            return base.Validate(context, settings);
        }
    }
}
