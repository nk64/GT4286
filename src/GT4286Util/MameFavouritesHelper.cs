namespace GT4286Util
{
    public static class MameFavouritesHelper
    {
        public static List<string> ExtractFavourites(string favouritesMameIniFilePath)
        {
            List<string> favourites = File.ReadAllText(favouritesMameIniFilePath)
                .Split("[ROOT_FOLDER]")
                .Skip(1)
                .Single()
                .Trim()
                .Split("\r\n")
                .Select(s => s.Trim())
                .Where(s => string.IsNullOrEmpty(s) == false)
                .OrderBy(s => s)
                .ToList();

            return favourites;
        }
    }
}