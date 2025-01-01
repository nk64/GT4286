using System.IO.Enumeration;
using GT4286Util.Entities;

namespace GT4286Util
{
    public class RetroSDCardManager
    {
        private string baseDir;

        public RetroSDCardManager(string baseDir)
        {
            this.baseDir = baseDir;
        }

        public static int OrderGameType(Game g)
        {
            switch(g.gametype)
            {
                case "FBA": return 1;
                case "FC": return 2;
                case "GB": return 3;
                case "GBA": return 4;
                case "GBC": return 5;
                case "MAME": return 6;
                case "PCE": return 7;
                case "PS1": return 8;
                case "MD": return 9;
                case "SFC": return 10;
                
                //case "MAME": return 11;
            }
            return 0;
        }


        public static HashSet<string> KnownRomExtensions = new HashSet<string>() {
            ".zip",
            ".7z",
            ".nes",
            ".gb",
            ".gba",
            ".gbc",
            ".img",
            ".bin",
            ".sfc",
            ".smc",
            ".pce",
        };

        public static List<string> RomSubDirs = new List<string>()
        {
            "FBA",
            "FC",
            "GB",
            "GBA",
            "GBC",
            "MAME",
            "PCE",
            "PS",
            "MD",
            "SFC",
        };
    /*
    GBA    
    GBC
    MAME
    MD
    PCE
    PS
    SFC
    FBA
    FC
    GB
    */

        public static Game? EntryFromRomPath(string rompath, bool escapeSingleQuotes = false, bool includeFileExtension = false)
        {
            if (rompath.Contains(@"\image\")) { return null; }
            if (rompath.Contains(@"\redir\")) { return null; }
            if (rompath.Contains(@"/image/")) { return null; }
            if (rompath.Contains(@"/redir/")) { return null; }
            string ext = Path.GetExtension(rompath);

            // gamebatte places saved state files in the rom dir
            if (ext == ".gqs") { return null; }
            if (ext == ".preview") { return null; }

            // picodrive places saved state previews in the rom dir
            if (ext == ".prev") { return null; }
            if (ext == ".sav") { return null; }

            string gametype = Path.GetFileName(Path.GetDirectoryName(rompath)!).ToUpperInvariant().Replace(@"PS", "PS1");

            string roms_or_download = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(rompath)!)!);

            int dwonloadid_hint = string.Compare(roms_or_download, "download", StringComparison.InvariantCultureIgnoreCase) == 0 ? 1 : 0;

            string filename = Path.GetFileName(rompath);

            string filechinesescan = includeFileExtension ? filename : Path.GetFileNameWithoutExtension(filename);
            string fileenglishscan = includeFileExtension ? filename : Path.GetFileNameWithoutExtension(filename);

            if (escapeSingleQuotes)
            {
                filename = EscapeSingleQuotes(filename);
                filechinesescan = EscapeSingleQuotes(filechinesescan);
                fileenglishscan = EscapeSingleQuotes(fileenglishscan);
            }

            return new Game()
            {
                gametype = gametype,
                filename = filename,
                filechinesescan = filechinesescan,
                fileenglishscan = fileenglishscan,
                dwonloadid = dwonloadid_hint,
                sortid = 50000,
            };
        }

        private static string EscapeSingleQuotes(string filename)
        {
            //return filename.Replace("'", "''");
            return filename.Replace("'", "\"");
        }

        public static string MakeSafeFilename(string filename, char replaceChar)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, replaceChar);
            }
            return filename;
        }

        public static string SanitiseDescription(string description)
        {
            description = description.Replace(".", "")
                .Replace(":", "")
                .Replace("?", "");
                
            return MakeSafeFilename(description, '-');
        }

    }
}