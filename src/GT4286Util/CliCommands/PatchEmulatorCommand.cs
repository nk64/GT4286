using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class PatchEmulatorCommand : AsyncCommand<PatchEmulatorCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<path-to-card>")]
            public required string PathToSdCardRoot { get; set; }

            [CommandOption("--verbose")]
            public bool Verbose { get; set; }

            [Description("Just do it!")]
            [CommandOption("--yes")]
            public bool Yes { get; set; }
        }


        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public PatchEmulatorCommand(
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
            if (_fileSystem.Directory.Exists(settings.PathToSdCardRoot) == false)
            {
                return ValidationResult.Error($"Path not found: {settings.PathToSdCardRoot}");
            }

            return base.Validate(context, settings);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.WriteLine("Checking Emulators for Patch availability...");

            AnsiConsole.WriteLine();

            string[] keyFileList = [
                _fileSystem.Path.Combine(@"emus/fbneo/fbneo"),
                _fileSystem.Path.Combine(@"emus/mame/fbneo"),
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

            return await Task.FromResult(0);
        }
    }
}

