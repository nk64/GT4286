using System.ComponentModel;
using System.IO.Abstractions;
using System.Text;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class GeneratePatchCommand : AsyncCommand<GeneratePatchCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The path to the initial file")]
            [CommandArgument(0, "<initial-file>")]
            public required string PathToFrom { get; set; }

            [Description("The path to the target file")]
            [CommandArgument(1, "<final-file>")]
            public required string PathToTarget { get; set; }

            [Description("The path to the output patch file")]
            [CommandArgument(2, "<output-patch-file>")]
            public required string PathToOutputPatchFile { get; set; }

            // [CommandOption("--verbose")]
            // public bool Verbose { get; set; }

            [Description("Just do it!")]
            [CommandOption("--yes")]
            public bool Yes { get; set; }
        }

        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public GeneratePatchCommand(
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
            if (_fileSystem.File.Exists(settings.PathToFrom) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToFrom}");
            }

            if (_fileSystem.File.Exists(settings.PathToTarget) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToTarget}");
            }

            return base.Validate(context, settings);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.WriteLine($"Initial File: {settings.PathToFrom}");
            AnsiConsole.WriteLine($"Final File: {settings.PathToTarget}");
            AnsiConsole.WriteLine($"Output Patch File: {settings.PathToOutputPatchFile}");

            var confirmation = false;

            if (_fileSystem.File.Exists(settings.PathToOutputPatchFile))
            {
                confirmation = settings.Yes || AnsiConsole.Prompt(
                    new TextPrompt<bool>($"Destination file ({settings.PathToOutputPatchFile}) exists, overwrite?")
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
                var simplePatchData = await simplePatchHelper.GenerateSimplePatchDataAsync(settings.PathToFrom, settings.PathToTarget);
                var jsonString = SerializationHelper.SerializeToJsonString(simplePatchData, SerializationHelper.JsonSerializerContext.SimplePatchData);
                await _fileSystem.File.WriteAllTextAsync(settings.PathToOutputPatchFile, jsonString);

                // using (var outputPatchFileStream = _fileSystem.File.Create(settings.PathToOutputPatchFile))
                // {
                //     var deltaAscii = DeltaSharp.DeltaSharp.CreateASCII(b1, b2);
                //     string patch = Encoding.UTF8.GetString(deltaAscii.Data.Span);
                //     outputPatchFileStream.Write(deltaAscii.Data.Span);
                // }

                AnsiConsole.MarkupLine($"Patch file was generated: {settings.PathToOutputPatchFile}");
            }
            else
            {
                AnsiConsole.MarkupLine("No patch file was generated");
            }

            return await Task.FromResult(0);
        }

    }
}


