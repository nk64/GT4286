using System.ComponentModel;
using System.Diagnostics;
using System.IO.Abstractions;
using GT4286Util.Entities;
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

        public HumaniseArcadeNamesCommand(
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
            var hashHelper = new HashHelper(_fileSystem);

            var FBNeoGameDescriptionDictionary = FBNeoDatHelper.GetArcadeGameDescriptionDictionary();
            AnsiConsole.WriteLine("Humanising Arcade Rom Names...");

            ISet<string> validRomFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".zip", ".7z" };
            ISet<string> validImageExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".jpg", ".gif" };

            var redirectedRoms = _fileSystem.Directory
                .EnumerateFiles(Path.Combine(settings.PathToSdCardRoot, "roms", "FBA"), "*.redir")
                .Select(filePath => new HumaniseEntry()
                {
                    BuiltIn = true,
                    GameType = "FBA",
                    GameName = Path.GetFileNameWithoutExtension(File.ReadAllText(filePath)),
                    RomDirFilePath = GetRelativePathLinuxStyle(settings.PathToSdCardRoot, filePath),
                    RedirFilename = File.ReadAllText(filePath),
                })
                .ToList();

            var standardRoms = _fileSystem.Directory
                .EnumerateFiles(Path.Combine(settings.PathToSdCardRoot, "roms", "FBA"), "*")
                .Where(filePath => validRomFileExtensions.Contains(Path.GetExtension(filePath)))
                .Select(filePath => new HumaniseEntry()
                {
                    BuiltIn = true,
                    GameType = "FBA",
                    GameName = Path.GetFileNameWithoutExtension(filePath),
                    RomDirFilePath = GetRelativePathLinuxStyle(settings.PathToSdCardRoot, filePath),
                    RedirFilename = null,
                })
                .ToList();

            var images = _fileSystem.Directory
                .EnumerateFiles(Path.Combine(settings.PathToSdCardRoot, "roms", "FBA", "image"), "*")
                .Where(filePath => validImageExtensions.Contains(Path.GetExtension(filePath)))
                .Select(filePath => new HumaniseEntry()
                {
                    BuiltIn = true,
                    GameType = "FBA",
                    ImageFilePath = GetRelativePathLinuxStyle(settings.PathToSdCardRoot, filePath),
                })
                .ToList();

            foreach (var rom in redirectedRoms.ToArray())
            {
                string romGameKey = Path.GetFileNameWithoutExtension(rom.RomDirFilePath)!;

                foreach (var image in new List<HumaniseEntry>(images))
                {
                    string imageGameKey = Path.GetFileNameWithoutExtension(image.ImageFilePath)!;

                    if (romGameKey.Equals(imageGameKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        rom.ImageFilePath = image.ImageFilePath;
                        images.Remove(image);
                    }
                }
            }

            foreach (var rom in standardRoms)
            {
                string romGameKey = Path.GetFileNameWithoutExtension(rom.RomDirFilePath)!;

                foreach (var image in new List<HumaniseEntry>(images))
                {
                    string imageGameKey = Path.GetFileNameWithoutExtension(image.ImageFilePath)!;

                    if (romGameKey.Equals(imageGameKey, StringComparison.InvariantCultureIgnoreCase))
                    {
                        rom.ImageFilePath = image.ImageFilePath;
                        images.Remove(image);
                    }
                }
            }

            foreach (var rom in standardRoms)
            {
                if (FBNeoGameDescriptionDictionary.ContainsKey(rom.GameName!))
                {
                    (string description, string sanetised_description) = FBNeoGameDescriptionDictionary[rom.GameName!];
                    rom.DescriptonFromDat = sanetised_description;
                    rom.RedirFilename = Path.Combine(Path.GetDirectoryName(rom.RomDirFilePath)!, $"{sanetised_description}.redir").Replace("\\", "/");
                }
            }

            List<HumaniseEntry> standardRomsToHumanise = standardRoms.Where(r=>r.DescriptonFromDat != null).ToList();
            List<HumaniseEntry> standardRomsCantHumanise = standardRoms.Where(r=>r.DescriptonFromDat == null).ToList();

            //standardRoms.Dump("Standard Roms");
            //redirectedRoms.Dump("Redirected Roms");
            //images.Dump("Images");

            if (standardRomsToHumanise.Count > 0 && settings.Verbose)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLineInterpolated($"Game entries to [green]humanise[/]: {standardRomsToHumanise.Count}");
                //added.Dump($"Games to Add: {added.Count}");
                standardRomsToHumanise.Dump();
            }

            if (standardRomsCantHumanise.Count > 0 && settings.Verbose)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLineInterpolated($"Game entries that [red]cant[/] be humanised: {standardRomsCantHumanise.Count}");
                //added.Dump($"Games to Add: {added.Count}");
                standardRomsCantHumanise.Dump();
            }

            //renumbered.Dump($"Games to Renumber: {renumbered.Count}");

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLineInterpolated($"Game entries to [green]humanise[/]: {standardRomsToHumanise.Count}");
            AnsiConsole.MarkupLineInterpolated($"Game entries that [red]cant[/] be humanised: {standardRomsCantHumanise.Count}");
            AnsiConsole.MarkupLineInterpolated($"Game entries that are already [blue]humanised[/]: {redirectedRoms.Count}");
            AnsiConsole.WriteLine();

            if (standardRomsToHumanise.Count > 0)
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
                        new TextPrompt<bool>($"Humanising your roms is a fairly invasive procedure and will move and rename your FBA roms and images. Please enusure you have a backup of your SD Card. Are you sure you want to continue humanising {standardRomsToHumanise.Count} roms?")
                            .AddChoice(true)
                            .AddChoice(false)
                            .DefaultValue(false)
                            .WithConverter(choice => choice ? "y" : "n"));
                }

                if (confirmation)
                {
                    // Rename Image files where available
                    foreach(var r in standardRomsToHumanise)
                    {
                        if (r.ImageFilePath != null)
                        {
                            string humanisedImagePath = Path.Combine(Path.GetDirectoryName(r.ImageFilePath)!, $"{r.DescriptonFromDat}{Path.GetExtension(r.ImageFilePath)}").Replace("\\", "/");
                            string source = Path.GetFullPath(Path.Combine(settings.PathToSdCardRoot, r.ImageFilePath!));
                            string dest = Path.GetFullPath(Path.Combine(settings.PathToSdCardRoot, humanisedImagePath));

                            if (_fileSystem.File.Exists(source))
                            {
                                AnsiConsole.MarkupLineInterpolated($"Renaming image file: {source} -> {GetRelativeFilePath(source, dest)}");

                                if (_fileSystem.File.Exists(dest))
                                {
                                    if (string.Compare(source, dest, StringComparison.InvariantCultureIgnoreCase) == 0)
                                    {
                                        if (string.Compare(source, dest, StringComparison.InvariantCulture) != 0)
                                        {
                                            //Source and dest string are the same but case so let's do a trick to rename to the right case
                                            string tempdest = dest + ".tmp";
                                            _fileSystem.File.Move(source, tempdest);
                                            _fileSystem.File.Move(tempdest, dest);
                                        }
                                        else
                                        {
                                            //source and dest are exactly the same file, nothing to do
                                        }
                                    }
                                    else
                                    {
                                        // source and destination filenames differ
                                        bool filesAreSame = await FilesAreSameAsync(hashHelper, source, dest);

                                        if (filesAreSame)
                                        {
                                            AnsiConsole.MarkupLineInterpolated($"Identical destination image already exists [blue]removing the source[/]: {source}");
                                            _fileSystem.File.Delete(source);
                                            continue;
                                        }
                                        else
                                        {
                                            AnsiConsole.MarkupLineInterpolated($"Destination image already exists and is different to source [blue]skipping[/]: {dest}");
                                        }
                                    }
                                }
                                else
                                {
                                    //Destination doesn't exist lets move the source to destination
                                    string tempdest = dest + ".tmp";
                                    _fileSystem.File.Move(source, tempdest);
                                    _fileSystem.File.Move(tempdest, dest);
                                }
                            }
                            else
                            {
                                if (_fileSystem.File.Exists(dest))
                                {
                                    AnsiConsole.MarkupLineInterpolated($"Image has already been renamed: {source} -> {GetRelativeFilePath(source, dest)}");
                                }
                                else
                                {
                                    AnsiConsole.MarkupLineInterpolated($"Expected image file [red]not found[/]: {source} -> {GetRelativeFilePath(source, dest)}");
                                }
                            }
                        }
                        else
                        {
                            //AnsiConsole.MarkupLineInterpolated($"No corresponding image file found: {r.RomDirFilePath}");
                        }
                    }

                    // Move Roms to redir folder
                    foreach(var r in standardRomsToHumanise)
                    {
                        string redirRomPath = Path.Combine(Path.GetDirectoryName(r.RomDirFilePath)!, "redir", Path.GetFileName(r.RomDirFilePath)!).Replace("\\", "/");
                        //AnsiConsole.MarkupLineInterpolated($"Moving Rom to redir directory: {r.RomDirFilePath} -> {redirRomPath}");
                        string source = Path.GetFullPath(Path.Combine(settings.PathToSdCardRoot, r.RomDirFilePath!));
                        string dest = Path.GetFullPath(Path.Combine(settings.PathToSdCardRoot, redirRomPath));

                        AnsiConsole.WriteLine($"Moving rom to redir directory: {source} -> {GetRelativeFilePath(source, dest)}");

                        if (_fileSystem.File.Exists(source))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                            //AnsiConsole.MarkupLineInterpolated($"Renaming rom zip file: {source} -> {dest}");

                            if (_fileSystem.File.Exists(dest))
                            {
                                if (await FilesAreSameAsync(hashHelper, source, dest))
                                {
                                    AnsiConsole.MarkupLineInterpolated($"Identical rom file already exists [yello]removing the source[/]: {source}");
                                    _fileSystem.File.Delete(source);
                                }
                                else
                                {
                                    AnsiConsole.MarkupLineInterpolated($"Target Rom zip file already exists but is different. Please use a rom manager to validate the correct roms: {dest}");
                                }
                            }
                            else
                            {
                                _fileSystem.File.Move(source, dest);
                            }
                        }
                    }

                    // Create/update .redir file
                    foreach(var r in standardRomsToHumanise)
                    {
                        if (r.RedirFilename != null)
                        {
                            string redirContents = Path.GetFileName(r.RomDirFilePath)!;
                            string dest = Path.GetFullPath(Path.Combine(settings.PathToSdCardRoot, r.RedirFilename));

                            if (_fileSystem.File.Exists(dest))
                            {
                                string oldRedirContents = await _fileSystem.File.ReadAllTextAsync(dest);
                                if (oldRedirContents != redirContents)
                                {
                                    AnsiConsole.WriteLine($"Updating redir file: {r.RedirFilename} [{redirContents}]");
                                    await _fileSystem.File.WriteAllTextAsync(dest, redirContents);
                                }
                                else
                                {
                                    // Nothing to do
                                }
                            }
                            else
                            {
                                AnsiConsole.WriteLine($"Creating redir file: {r.RedirFilename} [{redirContents}]");
                                await _fileSystem.File.WriteAllTextAsync(dest, redirContents);
                            }

                        }
                    }

                    AnsiConsole.MarkupLine("Games have been humanised.");
                    AnsiConsole.MarkupLine("Don't forget to run [grey]GT4286Util refresh-gamedb <path-to-card>[/].");
                }
                else
                {
                    AnsiConsole.MarkupLine("No games have been humanised");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("Nothing to do, All games are humanised");
            }

            AnsiConsole.WriteLine();


            return await Task.FromResult(0);
        }

        private async Task<bool> FilesAreSameAsync(HashHelper hashHelper, string source, string dest)
        {
            var sourceFileInfo = _fileSystem.FileInfo.New(source);
            var destFileInfo = _fileSystem.FileInfo.New(dest);

            if (sourceFileInfo.Length != destFileInfo.Length) { return false; }

            // replace with performat byte by byte comparison like here: https://dev.to/emrahsungu/how-to-compare-two-files-using-net-really-really-fast-2pd9
            // or maybe here: https://github.com/DimonSmart/FileByContentComparer
            var sourceHash = await hashHelper.GetFileHashAsync(source);
            var destHash = await hashHelper.GetFileHashAsync(dest);

            if (sourceHash != destHash)
            {
                return false;
            }

            return true;
        }

        static string GetRelativeFilePath(string sourceFilePath, string destFilePath)
        {
            return Path.GetRelativePath(Path.GetDirectoryName(sourceFilePath)!, destFilePath);
        }


        static string GetRelativePath(string pathToSdCardRoot, string filePath)
        {
            return Path.GetRelativePath(pathToSdCardRoot, filePath);
        }

        static string GetRelativePathLinuxStyle(string pathToSdCardRoot, string filePath)
        {
            return ConvertPathToLinuxStyle(GetRelativePath(pathToSdCardRoot, filePath));
        }

        static string ConvertPathToLinuxStyle(string filePath)
        {
            return filePath.Replace("\\", "/");
        }


        public static async void Experiment_RedirectArcadeRomsWithHumanReadableNames(FileSystem _fileSystem, string retroArcadeBasePath)
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
                            await _fileSystem.File.WriteAllTextAsync(redirFilePath, redirstring);
                        } else {
                            Console.WriteLine($"Redir file exists: {redirFilePath}, Contents are good");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Writing {redirFilePath}");
                        await _fileSystem.File.WriteAllTextAsync(redirFilePath, redirstring);
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
