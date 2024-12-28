using System.IO.Abstractions;
using Dapper;
using GT4286Util.Entities;
using GT4286Util.Helpers;
//using SkiaSharp;

namespace GT4286Util
{
    public static class Experiments
    {
        static readonly FileSystem _fileSystem = new FileSystem();

        public static void Experiment_CheckIfMAMEFavouriteGamesExist(string retroArcadeBasePath, string mameFavouritesIniFile)
        {
            List<string> favourites = MameFavouritesHelper.ExtractFavourites(mameFavouritesIniFile);

            foreach (var f in favourites)
            {
                var p = Path.Combine(retroArcadeBasePath, "roms", "FBA", f + ".zip");
                var p2 = Path.Combine(retroArcadeBasePath, "roms", "FBA", "redir", f + ".zip");
                if (File.Exists(p) == false && File.Exists(p2))
                {
                    Console.WriteLine($"A favourite rom is missing: {f}");
                }
            }
        }

#if false
        public static void Experiment_AddImagesForFavouriteGames(string retroArcadeBasePath, string imageSourceDir, string mameFavouritesIniFile)
        {
            //string imageSourceDir = @"...\MAME 0.264 EXTRAs\snap"; //*.png

            List<string> favourites = MameFavouritesHelper.ExtractFavourites(mameFavouritesIniFile);

            foreach (var f in favourites)
            {
                var p = Path.Combine(retroArcadeBasePath, "roms", "FBA", "image", f + ".jpg");
                if (File.Exists(p) == false)
                {
                    string src = Path.Combine(imageSourceDir, f + ".png");

                    if (File.Exists(src))
                    {
                        Console.WriteLine(src + " => " + p);

                        using (SKImage image = SKImage.FromEncodedData(src))
                        {
                            using(var filestream = File.OpenWrite(p))
                            {
                                using(SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                                {
                                    data.SaveTo(filestream);
                                }
                            }
                        }
                    } else {
                        Console.WriteLine(src + " => File Not Found");
                    }
                }
            }
        }
#endif
        public static void Experiment_DumpGameUnusualGames(string retroArcadeBasePath)
        {
            var gameDbManager = new GameDbManager(_fileSystem, _fileSystem.Path.Combine(retroArcadeBasePath, "game.db"));

            List<Game> games = gameDbManager.GetBuiltinGames()
                // .Where(g=> File.Exists(g.RomPath(retroArcadeBasePath)) == false)
                // .Where(g=> File.Exists(g.ImagePath(retroArcadeBasePath)) == false)
                // .Where(g=>g.gametype == "FBA")
                // .Where(g => false
                //     || g.filechinesescan != g.filechinesescan
                //     || Path.GetFileNameWithoutExtension(g.filename) != g.fileenglishscan
                //     || RetroSDCardManager.KnownRomExtensions.Contains(Path.GetExtension(g.filename)) == false
                // )
                .ToList();

            games.Dump("Games");
        }

        public static void Experiment_DeleteDownloadedRecords(string retroArcadeBasePath)
        {
            string retroArcadeGameDbPath = _fileSystem.Path.Combine(retroArcadeBasePath, "game.db");
            var gameDbManager = new GameDbManager(_fileSystem, retroArcadeGameDbPath);

            using (var con = gameDbManager.GetOpenConnection())
            {
                con.Execute("delete from game where dwonloadid > 0");
            }
        }
    }
}
