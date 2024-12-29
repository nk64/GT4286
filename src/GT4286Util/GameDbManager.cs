using System.IO.Abstractions;
using Dapper.Contrib.Extensions;
using GT4286Util.Entities;
using Microsoft.Data.Sqlite;

namespace GT4286Util
{
    public class GameDbManager
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _retroArcadeGameDbPath;

        public GameDbManager(IFileSystem fileSystem, string retroArcadeGameDbPath)
        {
            _fileSystem = fileSystem;
            _retroArcadeGameDbPath = retroArcadeGameDbPath;
        }

        public SqliteConnection GetOpenConnection()
        {
            var sqliteCSB = new SqliteConnectionStringBuilder()
            {
                 DataSource = _retroArcadeGameDbPath,
                 Pooling = false 
            };

            var con = new  SqliteConnection(sqliteCSB.ConnectionString);
            con.Open();
            return con;
        }

        public List<Game> GetAllGames()
        {
            using (var con = GetOpenConnection())
            {
                return con.GetAll<Game>().ToList();
            }
        }

        public List<Game> GetBuiltinGames()
        {
            return GetAllGames()
                .Where(g=>g.dwonloadid == 0)
                .ToList();
        }

        public List<Game> GetDownloadedGames()
        {
            return GetAllGames()
                .Where(g=>g.dwonloadid > 0)
                .ToList();
        }

        public void RefreshGameDatabase(List<Game> gameList, bool backupGameDb = true)
        {
            if (backupGameDb)
            {
                string backupFileName = _retroArcadeGameDbPath + $".{DateTime.Now:yyyyMMddHHmmss}.bak";
                _fileSystem.File.Copy(_retroArcadeGameDbPath, backupFileName);
            }

            using (var con = GetOpenConnection())
            {
                using (var tx = con.BeginTransaction())
                {
                    con.DeleteAll<Game>(tx);
                    foreach (var g in gameList)
                    {
                        con.Insert<Game>(g, tx);
                    }
                    
                    tx.Commit();
                }
            }
        }

        public void ClearPlayedState()
        {
            using (var con = GetOpenConnection())
            {
                using (var tx = con.BeginTransaction())
                {
                    foreach (var game in con.GetAll<Game>(tx))
                    {
                        game.his = false;
                        game.time = 0;
                        con.Update<Game>(game, tx);
                    }
                    tx.Commit();
                }
            }
        }

    }
}


