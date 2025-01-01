using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class ApplyPatchCommand : AsyncCommand<ApplyPatchCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The path to the patch file")]
            [CommandArgument(0, "<patch-file>")]
            public required string PathToPatchFile { get; set; }

            [Description("The path to file to patch")]
            [CommandArgument(1, "<file-to-patch>")]
            public required string PathToFrom { get; set; }

            [Description("The path to the output target file")]
            [CommandArgument(2, "<final-file>")]
            public required string PathToOutputFile { get; set; }

            // [CommandOption("--verbose")]
            // public bool Verbose { get; set; }

            [Description("Just do it!")]
            [CommandOption("--yes")]
            public bool Yes { get; set; }
        }

        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public ApplyPatchCommand(
            IAnsiConsole console,
            ILogger<IdentifySdCardCommand> logger,
            IFileSystem fileSystem
        )
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _logger.LogDebug(message: "{Command} initialized", nameof(IdentifySdCardCommand));
        }

        public override ValidationResult Validate(CommandContext context, Settings settings)
        {
            if (_fileSystem.File.Exists(settings.PathToPatchFile) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToPatchFile}");
            }

            if (_fileSystem.File.Exists(settings.PathToFrom) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToFrom}");
            }

            return base.Validate(context, settings);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.WriteLine($"Patch File: {Path.GetFullPath(settings.PathToPatchFile)}");
            AnsiConsole.WriteLine($"Initial File: {Path.GetFullPath(settings.PathToFrom)}");
            AnsiConsole.WriteLine($"Output File: {Path.GetFullPath(settings.PathToOutputFile)}");

            var confirmation = false;

            if (_fileSystem.File.Exists(settings.PathToOutputFile))
            {
                confirmation = settings.Yes || AnsiConsole.Prompt(
                    new TextPrompt<bool>($"Destination file ({Path.GetFullPath(settings.PathToOutputFile)}) exists, overwrite?")
                        .AddChoice(true)
                        .AddChoice(false)
                        .DefaultValue(false)
                        .WithConverter(choice => choice ? "y" : "n"));
            }
            else
            {
                confirmation = true;
            }

            if (confirmation)
            {
                var simplePatchHelper = new SimplePatchHelper(_fileSystem);
                var patchJsonString = _fileSystem.File.ReadAllText(settings.PathToPatchFile);
                var simplePatchData = SerializationHelper.DeserializeJsonString<SimplePatchData>(patchJsonString, SerializationHelper.JsonSerializerContext.SimplePatchData);

                try
                {
                    await simplePatchHelper.ApplySimplePatchDataAsync(settings.PathToFrom, simplePatchData, settings.PathToOutputFile);
                    AnsiConsole.MarkupLine("The patch was applied");
                    return await Task.FromResult(0);
                }
                catch (InvalidOperationException iox)
                {
                    AnsiConsole.MarkupLine($"Patch could not be applied: {iox.Message}");
                    return await Task.FromResult(1);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("The patch was not applied");
                return await Task.FromResult(1);
            }
        }
    }
}

