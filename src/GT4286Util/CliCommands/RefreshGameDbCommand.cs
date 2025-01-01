using System.ComponentModel;
using System.IO.Abstractions;
using GT4286Util.Entities;
using GT4286Util.Helpers;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GT4286Util.CliCommands
{
    internal sealed class RefreshGameDbCommand : AsyncCommand<RefreshGameDbCommand.Settings>
    {
        public class Settings: CommandSettings
        {
            [Description("The Path to the Root of the SD Card (or backup folder)")]
            [CommandArgument(0, "<path-to-card>")]
            public required string PathToSdCardRoot { get; set; }

            [CommandOption("--alphabetical")]
            public bool AlphabeticalOrder { get; set; }

            [CommandOption("--verbose")]
            public bool Verbose { get; set; }

            [Description("Just do it!")]
            [CommandOption("--yes")]
            public bool Yes { get; set; }
        }

        private readonly IAnsiConsole _console;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public RefreshGameDbCommand(
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

            string gameDbPath = _fileSystem.Path.Combine(settings.PathToSdCardRoot, "game.db");
            if (_fileSystem.File.Exists(gameDbPath) == false)
            {
                return ValidationResult.Error($"GameDb not found in {settings.PathToSdCardRoot}");
            }

            return base.Validate(context, settings);
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.WriteLine("Reviewing SD Card for game changes...");
            RefreshBuiltinGameDbList(settings.PathToSdCardRoot, settings);
            return await Task.FromResult(0);
        }

        public void RefreshBuiltinGameDbList(string retroArcadeBasePath, Settings settings)
        {
            bool escapeSingleQuotes = false;
            bool includeFileExtension = false;

            string retroArcadeGameDbPath = _fileSystem.Path.Combine(retroArcadeBasePath, "game.db");
            var gameDbManager = new GameDbManager(_fileSystem, retroArcadeGameDbPath);

            var gamesFromGameDB = gameDbManager.GetAllGames();
            //gamesFromGameDB.Dump("From GameDb");

            {
                var retroSDCardManager = new RetroSDCardManager(retroArcadeBasePath);

                List<Game> gamesFromRomDir = new List<Game>();

                List<Game> gamesFromDownloadDir = new List<Game>();

                foreach (var romSubDir in RetroSDCardManager.RomSubDirs)
                {
                    var gamesFromRomSubDir = Directory.EnumerateFiles(Path.Combine(retroArcadeBasePath, "roms", romSubDir), "*")
                        .Select(rompath => RetroSDCardManager.EntryFromRomPath(rompath, escapeSingleQuotes, includeFileExtension))
                        .Where(g => g != null)
                        .Cast<Game>()
                        .ToList();

                    gamesFromRomDir.AddRange(gamesFromRomSubDir);

                    var gamesFromDownloadSubDir = Directory.EnumerateFiles(Path.Combine(retroArcadeBasePath, "download", romSubDir), "*")
                        .Select(rompath => RetroSDCardManager.EntryFromRomPath(rompath, escapeSingleQuotes, includeFileExtension))
                        .Where(g => g != null)
                        .Cast<Game>()
                        .ToList();

                    gamesFromDownloadDir.AddRange(gamesFromDownloadSubDir);
                }

                var gamesFromSdCard = new List<Game>();
                gamesFromSdCard.AddRange(gamesFromRomDir);
                gamesFromSdCard.AddRange(gamesFromDownloadDir);

                if (settings.AlphabeticalOrder)
                {
                    gamesFromSdCard = StockOrder(gamesFromSdCard);
                }
                else
                {
                    gamesFromSdCard = AlphabeticalOrder(gamesFromSdCard);
                }

                //add id
                int i = 1;
                int dlid = 1;
                foreach (var g in gamesFromSdCard)
                {
                    g.id = i++;
                    if (g.dwonloadid != 0)
                    {
                        g.dwonloadid = dlid;
                        dlid++;
                    }
                }

                // Maintain fav, his and time values where possible
                foreach (var gameFromSdCard in gamesFromSdCard)
                {
                    var gameFromGameDB = gamesFromGameDB
                        .Where(dbgame => dbgame.IsEffectivelyTheSameAs(gameFromSdCard))
                        .SingleOrDefault();

                    if (gameFromGameDB != null)
                    {
                        gameFromSdCard.fav = gameFromGameDB.fav;
                        gameFromSdCard.his = gameFromGameDB.his;
                        gameFromSdCard.time = gameFromGameDB.time;
                    }
                }

                //gamesFromSdCard.Dump("From RomDir");

                List<Game> added = new List<Game>();
                List<Game> removed = new List<Game>();
                List<Game> modified = new List<Game>();
                List<Game> renumbered = new List<Game>();
                List<Game> unchanged = new List<Game>();

                foreach (var gameFromSdCard in gamesFromSdCard)
                {
                    var gameFromGameDB = gamesFromGameDB
                        .Where(dbgame => dbgame.IsEffectivelyTheSameAs(gameFromSdCard))
                        .SingleOrDefault();
                    if (gameFromGameDB == null)
                    {
                        added.Add(gameFromSdCard);
                    }
                }

                foreach (var gameFromGameDB in gamesFromGameDB)
                {
                    var gameFromSdCard = gamesFromSdCard
                        .Where(sdcardgame => sdcardgame.IsEffectivelyTheSameAs(gameFromGameDB))
                        .SingleOrDefault();

                    if (gameFromSdCard == null)
                    {
                        removed.Add(gameFromGameDB);
                    }
                    else
                    {
                        if (gameFromGameDB.IsSameExceptForNumbering(gameFromSdCard))
                        {
                            if (gameFromGameDB.IsExactlyTheSameAs(gameFromSdCard))
                            {
                                unchanged.Add(gameFromSdCard);
                            }
                            else
                            {
                                renumbered.Add(gameFromSdCard);
                            }
                        }
                        else
                        {
                            var mod = new Game()
                            {
                                id = gameFromSdCard.id,
                                gametype = gameFromGameDB.gametype,

                                fav = gameFromGameDB.fav,
                                his = gameFromGameDB.his,
                                time = gameFromGameDB.time,
                                dwonloadid = gameFromGameDB.dwonloadid,
                                filename = gameFromGameDB.filename == gameFromSdCard.filename ? gameFromGameDB.filename : $"{gameFromGameDB.filename} -> {gameFromSdCard.filename}",
                                filechinesescan = gameFromGameDB.filechinesescan == gameFromSdCard.filechinesescan ? gameFromGameDB.filechinesescan : $"{gameFromGameDB.filechinesescan} -> {gameFromSdCard.filechinesescan}",
                                fileenglishscan = gameFromGameDB.fileenglishscan == gameFromSdCard.fileenglishscan ? gameFromGameDB.fileenglishscan : $"{gameFromGameDB.fileenglishscan} -> {gameFromSdCard.fileenglishscan}",
                            };

                            modified.Add(mod);
                        }
                    }
                }

                if (settings.AlphabeticalOrder)
                {
                    added = AlphabeticalOrder(added);
                    removed = AlphabeticalOrder(removed);
                    modified = AlphabeticalOrder(modified);
                    renumbered = AlphabeticalOrder(renumbered);
                    unchanged = AlphabeticalOrder(unchanged);
                }
                else
                {
                    added = StockOrder(added);
                    removed = StockOrder(removed);
                    modified = StockOrder(modified);
                    renumbered = StockOrder(renumbered);
                    unchanged = StockOrder(unchanged);
                }

                if (added.Count > 0 && settings.Verbose)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLineInterpolated($"Game entries to [green]add[/]: {added.Count}");
                    //added.Dump($"Games to Add: {added.Count}");
                    added.Dump();
                }

                if (removed.Count > 0 && settings.Verbose)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLineInterpolated($"Game entries to [red]remove[/]: {removed.Count}");
                    //removed.Dump($"Games to Remove: {removed.Count}");
                    removed.Dump();
                }

                if (modified.Count > 0 && settings.Verbose)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLineInterpolated($"Game entries to [purple]update[/]: {modified.Count}");
                    //modified.Dump($"Games to Modify: {modified.Count}");
                    modified.Dump();
                }

                //renumbered.Dump($"Games to Renumber: {renumbered.Count}");

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLineInterpolated($"Game entries to [green]add[/]: {added.Count}");
                AnsiConsole.MarkupLineInterpolated($"Game entries to [red]remove[/]: {removed.Count}");
                AnsiConsole.MarkupLineInterpolated($"Game entries to [purple]update[/]: {modified.Count}");
                AnsiConsole.MarkupLineInterpolated($"Game entries to [yellow]renumber[/]: {renumbered.Count}");
                AnsiConsole.MarkupLineInterpolated($"Game entries to [blue]remain unchanged[/]: {unchanged.Count}");
                AnsiConsole.WriteLine();

                if (added.Count > 0 || removed.Count > 0 || modified.Count > 0 || renumbered.Count > 0)
                {
                    var confirmation = false;

                    if (settings.Yes)
                    {
                        confirmation = true;
                    }
                    else
                    {
                        if (settings.Verbose == false)
                        {
                            AnsiConsole.MarkupLine("Re-run the command with the --verbose option for more details.");
                            AnsiConsole.WriteLine();
                        }

                        confirmation = AnsiConsole.Prompt(
                            new TextPrompt<bool>($"Would to like to update [grey]/game.db[/] (a backup will be taken)?")
                                .AddChoice(true)
                                .AddChoice(false)
                                .DefaultValue(false)
                                .WithConverter(choice => choice ? "y" : "n"));
                    }

                    if (confirmation)
                    {
                        gameDbManager.RefreshGameDatabase(gamesFromSdCard, backupGameDb: true);
                        AnsiConsole.MarkupLine("The [grey]/game.db[/] has been updated");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("The [grey]/game.db[/] has not been updated");
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine("Nothing to do. The [grey]/game.db[/] has not been updated");
                }

                AnsiConsole.WriteLine();
            }

            static List<Game> StockOrder(List<Game> source)
            {
                // This seems to be the default ordering of the system
                return source
                    .OrderBy(g => g.dwonloadid)
                    .ThenBy(g => RetroSDCardManager.OrderGameType(g))
                    .ThenBy(g => g.filename)
                    .ToList();
            }

            static List<Game> AlphabeticalOrder(List<Game> source)
            {
                return source
                    .OrderBy(g => g.dwonloadid)
                    .ThenBy(g => g.filename)
                    .ThenBy(g => RetroSDCardManager.OrderGameType(g))
                    .ToList();
            }

        }
    }
}
