namespace GT4286Util
{
    public class GenerationInfo
    {
        public GenerationId GenerationId { get; set; }
        public required string ApproximateDateDetails { get; set; }
        public int GameListCount { get; set; }
        public int GameDbBuiltinCount { get; set; }
        public int TotalRomCount { get; set; }
        public required Dictionary<string, int> RomDirFileCounts { get; set; }
        public required Dictionary<string, string> KeyFileHashes { get; set; }
        public required string Title { get; set; }
        public string? Notes { get; set; }

        public bool IsEffectivelyTheSameAs(GenerationInfo gi)
        {
            if (this.GameListCount != gi.GameListCount) { return false; }
            if (this.GameDbBuiltinCount != gi.GameDbBuiltinCount) { return false; }
            if (this.TotalRomCount != gi.TotalRomCount) { return false; }
            foreach (var kvp in RomDirFileCounts)
            {
                if (gi.RomDirFileCounts[kvp.Key] != kvp.Value) { return false; }
            }
            foreach (var kvp in KeyFileHashes)
            {
                if (gi.KeyFileHashes[kvp.Key] != kvp.Value) { return false; }
            }

            return true;
        }
    }
}