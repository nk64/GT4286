using Dapper.Contrib.Extensions;

namespace GT4286Util.Entities
{

    [Table("game")]
    public class Game
    {
        [ExplicitKey]
        public int id { get; set; }
        public required string gametype { get; set; }
        public required string filename { get; set; }
        public required string filechinesescan { get; set; }
        public required string fileenglishscan { get; set; }
        public bool fav { get; set; }
        public bool his { get; set; }
        public long time { get; set; }
        public int dwonloadid { get; set; }
        public int sortid { get; set; }

        public string RomPath(string root) {  return dwonloadid == 0 ? 
            Path.Combine(root, "roms", gametype.ToLowerInvariant().Replace("ps1", "ps") + @"\" + filename) :
            Path.Combine(root, "download", gametype.ToLowerInvariant().Replace("ps1", "ps") + @"\" + filename);
        }

        public string ImagePath(string root)
        {
            return Path.Combine(root, "roms", "images", gametype.ToLowerInvariant().Replace("ps1", "ps") + @"\" + Path.ChangeExtension(filename, ".jpg"));
        }

        public bool IsEffectivelyTheSameAs(Game other)
        {
            if (this.gametype != other.gametype) { return false; }

            if (this.filename.Replace("\"", "'") != other.filename.Replace("\"", "'")) { return false; }
            //if (this.filename != other.filename) { return false; }

            if (this.dwonloadid == 0 && other.dwonloadid > 0) { return false; }
            if (this.dwonloadid > 0 && other.dwonloadid == 0) { return false; }

            return true;
        }

        public bool IsExactlyTheSameAs(Game other)
        {
            if (this.id != other.id) { return false; }
            if (this.gametype != other.gametype) { return false; }
            if (this.filename != other.filename) { return false; }
            if (this.filechinesescan != other.filechinesescan) { return false; }
            if (this.fileenglishscan != other.fileenglishscan) { return false; }
            if (this.fav != other.fav) { return false; }
            if (this.his != other.his) { return false; }
            if (this.time != other.time) { return false; }
            if (this.dwonloadid != other.dwonloadid) { return false; }
            if (this.sortid != other.sortid) { return false; }
            return true;
        }

        public bool IsSameExceptForNumbering(Game other)
        {
            //if (this.id != other.id) { return false; }
            if (this.gametype != other.gametype) { return false; }

            //if (this.filename.Replace("\"", "'") != other.filename.Replace("\"", "'")) { return false; }
            //if (this.filechinesescan.Replace("\"", "'") != other.filechinesescan.Replace("\"", "'")) { return false; }
            //if (this.fileenglishscan.Replace("\"", "'") != other.fileenglishscan.Replace("\"", "'")) { return false; }

            if (this.filename != other.filename) { return false; }
            if (this.filechinesescan != other.filechinesescan) { return false; }
            if (this.fileenglishscan != other.fileenglishscan) { return false; }

            if (this.fav != other.fav) { return false; }
            if (this.his != other.his) { return false; }
            if (this.time != other.time) { return false; }
            //if (this.dwonloadid != other.dwonloadid) { return false; }
            //if (this.sortid != other.sortid) { return false; }
            return true;
        }


        /*
        CREATE TABLE game (
            id integer primary key,
            gametype char(10),
            filename char(48),
            filechinesescan char(48),
            fileenglishscan char(48),
            fav bool,
            his bool,
            time bigint,
            dwonloadid int,
            sortid int
        )
        */
    }

    public class HumaniseEntry
    {
        public required bool BuiltIn { get; set; }
        public required string GameType { get; set; }
        public string? GameName { get; set; }
        public string? RomDirFilePath { get; set; }
        public string? RedirFilename { get; set; }
        public string? DescriptonFromDat { get; set; }
        public string? ImageFilePath { get; set; }
    }



    public class GameAuditEntry
    {
        public required bool Downloaded { get; set; }
        public required string GameType { get; set; }
        public string? DbFilename { get; set; }
        public string? RomDirFilePath { get; set; }
        public string? RedirFilename { get; set; }
        public string? DescriptonFromDat { get; set; }
        public string? ImageFilePath { get; set; }
        public string? ConfigFilePath { get; set; }
        /*
        public string? State0StateFilePath { get; set; }
        public string? State0PreviewFilePath { get; set; }
        public string? State1StateFilePath { get; set; }
        public string? State1PreviewFilePath { get; set; }
        public string? State2StateFilePath { get; set; }
        public string? State2PreviewFilePath { get; set; }
        public string? State3StateFilePath { get; set; }
        public string? State3PreviewFilePath { get; set; }
        public string? State4StateFilePath { get; set; }
        public string? State4PreviewFilePath { get; set; }
        public string? State5StateFilePath { get; set; }
        public string? State5PreviewFilePath { get; set; }
        public string? State6StateFilePath { get; set; }
        public string? State6PreviewFilePath { get; set; }
        public string? State7StateFilePath { get; set; }
        public string? State7PreviewFilePath { get; set; }
        public string? State8StateFilePath { get; set; }
        public string? State8PreviewFilePath { get; set; }
        */
    }

    [Flags]
    public enum GT4286FileType : UInt64
    {
        Unknown = 0,
        UnusedFile =  1L << 1,

        Downloaded = 1L << 2,
        Builtin = 1L << 3,

        GameTypeFBA = 1L << 4,
        GameTypeFC = 1L << 5,
        GameTypeGB = 1L << 6,
        GameTypeGBC = 1L << 7,
        GameTypeGBA = 1L << 8,
        GameTypeMAME = 1L << 9,
        GameTypeMD = 1L << 10,
        GameTypePCE = 1L << 11,
        GameTypePS = 1L << 12,
        GameTypeSFC = 1L << 13,

        GameRomFile = 1L << 14,
        GameRomRedirection = 1L << 15,
        GameRedirectTargetFile = 1L << 16,
        GameImageFile = 1L << 17,
        GameRomShellScript = 1L << 18,

        StockBinary = 1L << 19,
        ShellScript = 1L << 20,
        PatchedBinary = 1L << 21,

        EmulatorConfigFile = 1L << 22,
        EmulatorOtherFile = 1L << 23,
        GameConfigFile = 1L << 24,
    }
}
