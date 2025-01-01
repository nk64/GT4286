using System.ComponentModel;
using System.IO.Abstractions;
using System.IO.Compression;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class BrotliCompressCommand : AsyncCommand<BrotliCompressCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The source file to compress")]
            [CommandArgument(0, "<input-filename>")]
            public required string InputFileName { get; set; }

            [Description("The destination file")]
            [CommandArgument(1, "[output-filename]")]
            public string? OutputFileName { get; set; }
        }

        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public BrotliCompressCommand(
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
            if (_fileSystem.File.Exists(settings.InputFileName) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.InputFileName}");
            }

            return base.Validate(context, settings);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            byte[] CompressBrotli(Stream inputString)
            {
                var compression = CompressionLevel.SmallestSize;
                using var output = new MemoryStream();
                using var compressor = new BrotliStream(output, compression, true);
                inputString.CopyTo(compressor);
                compressor.Close();
                return output.ToArray();
            }

            using (var stream = _fileSystem.File.OpenRead(settings.InputFileName))
            {
                string? outputFileName = settings.OutputFileName;
                if (outputFileName == null)
                {
                    outputFileName = settings.InputFileName + ".brotli";
                }

                if (_fileSystem.File.Exists(outputFileName))
                {
                    var confirmation = AnsiConsole.Prompt(
                        new TextPrompt<bool>($"Destination file ({outputFileName}) exists, overwrite?")
                            .AddChoice(true)
                            .AddChoice(false)
                            .DefaultValue(false)
                            .WithConverter(choice => choice ? "y" : "n"));
                    if (confirmation == false)
                    {
                        return await Task.FromResult(0);
                    }
                }

                var brotliBytes = CompressBrotli(stream);
                _fileSystem.File.WriteAllBytes(outputFileName, brotliBytes);
                AnsiConsole.WriteLine($"Brotli compressed file written to: {outputFileName}");
            }
            return await Task.FromResult(0);
        }

    }
}
