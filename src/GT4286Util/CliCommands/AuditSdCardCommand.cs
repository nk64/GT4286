using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Entities;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class AuditSdCardCommand : AsyncCommand<AuditSdCardCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<path-to-card>")]
            public required string PathToSdCardRoot { get; set; }

            [CommandOption("--verbose")]
            public bool Verbose { get; set; }
        }

        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public AuditSdCardCommand(
            IAnsiConsole console,
            ILogger<AuditSdCardCommand> logger,
            IFileSystem fileSystem
        )
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _logger.LogDebug(message: "{Command} initialized", nameof(AuditSdCardCommand));
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
            _logger.LogDebug("Starting {CommandClass}.ExecuteAsync", nameof(AuditSdCardCommand));
            
            AnsiConsole.WriteLine("Auditing SD Card...");

            Dictionary<string, GT4286FileType> auditEntries = new Dictionary<string, GT4286FileType>();

            var allFiles = _fileSystem.Directory.EnumerateFiles(Path.Combine(settings.PathToSdCardRoot), "*", SearchOption.AllDirectories);

            _console.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Dqpb)
                .Start("Scanning card", ctx =>
                {
                    //ctx.Spinner(Spinner.Known.Star);

                    foreach(string filePath in allFiles)
                    {
                        string relativePath = Path.GetRelativePath(settings.PathToSdCardRoot, filePath).Replace("\\", "/");
                        ctx.Status(relativePath.EscapeMarkup());
                        auditEntries.Add(relativePath, DetectFileType(settings.PathToSdCardRoot, relativePath));
                        ctx.Refresh();
                        //Thread.Sleep(1);
                    }
                });
            
            auditEntries.Take(20).Dump();


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

            _logger.LogDebug("Completed {CommandClass}.ExecuteAsync", nameof(AuditSdCardCommand));

            return await Task.FromResult(0);
        }

        private GT4286FileType DetectFileType(string retroArcadeBasePath, string relativeFilePath)
        {
            return GT4286FileType.Unknown;
        }
    }
}

