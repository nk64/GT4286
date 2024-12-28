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
}