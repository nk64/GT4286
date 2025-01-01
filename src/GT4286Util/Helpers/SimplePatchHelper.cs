using System.IO.Abstractions;

namespace GT4286Util.Helpers
{
    public class SimplePatchData
    {
        public required int FileLength { get; set; }
        public required string SourceHash { get; set; }
        public required string ResultHash { get; set; }
        public required Dictionary<string, string> Patches { get; set; }
    }

    public class SimplePatchHelper
    {
        // Why did I write my own?
        // - BsDiff doesn't do any checking of hashes before and after patching
        // - DeltaSharp didn't seem to do hash checking and raise unexpected exceptions if the input file was too small

        private readonly IFileSystem _fileSystem;
        private readonly HashHelper _hashHelper;

        public SimplePatchHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _hashHelper = new HashHelper(_fileSystem);
        }

        public async Task<SimplePatchData> GenerateSimplePatchDataAsync(string sourceFilePath, string patchedFilePath)
        {
            var f1 = _fileSystem.FileInfo.New(sourceFilePath);
            var f2 = _fileSystem.FileInfo.New(patchedFilePath);

            if (f1.Length != f2.Length)
            {
                throw new InvalidOperationException("Files must be the same length.");
            }

            byte[] unpatchedFileBytes = await _fileSystem.File.ReadAllBytesAsync(sourceFilePath);
            byte[] patchedFileBytes = await _fileSystem.File.ReadAllBytesAsync(patchedFilePath);

            string unpatchedFileHash = await _hashHelper.GetFileHashFromBytesAsync(unpatchedFileBytes);
            string patchedFileHash = await _hashHelper.GetFileHashFromBytesAsync(patchedFileBytes);

            Dictionary<string, string> patches = new Dictionary<string, string>();

            int diffCount = 0;
            for (long i = 0; i < unpatchedFileBytes.Length; i++)
            {
                if (unpatchedFileBytes[i] != patchedFileBytes[i])
                {
                    diffCount++;
                    if (diffCount > 1024)
                    {
                        throw new InvalidOperationException("More than 1024 differences discovered.");
                    }
                    patches.Add($"0x{i:X8}", $"0x{patchedFileBytes[i]:X2}");
                }
            }

            var simplePatchData = new SimplePatchData()
            {
                FileLength = unpatchedFileBytes.Length,
                SourceHash = unpatchedFileHash,
                ResultHash = patchedFileHash,
                Patches = patches,
            };

            return simplePatchData;
        }

        public async Task ApplySimplePatchDataAsync(string sourceFilePath, SimplePatchData simplePatchData, string patchedFilePath)
        {
            var f1 = _fileSystem.FileInfo.New(sourceFilePath);

            if (f1.Length != simplePatchData.FileLength)
            {
                throw new InvalidOperationException("Source file was not of the expected length.");
            }

            byte[] sourceFileBytes = await _fileSystem.File.ReadAllBytesAsync(sourceFilePath);
            string sourceFileHash = await _hashHelper.GetFileHashFromBytesAsync(sourceFileBytes);

            if (sourceFileHash != simplePatchData.SourceHash)
            {
                throw new InvalidOperationException("Source file hash was incorrect.");
            }

            byte[] patchedFileBytes = sourceFileBytes; // re-use the array

            foreach(var kvp in simplePatchData.Patches)
            {
                var location = kvp.Key.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? int.Parse(kvp.Key[2..], System.Globalization.NumberStyles.AllowHexSpecifier)
                    : int.Parse(kvp.Key);

                var newValue = kvp.Value.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? byte.Parse(kvp.Value[2..], System.Globalization.NumberStyles.AllowHexSpecifier)
                    : byte.Parse(kvp.Value);

                patchedFileBytes[location] = newValue;
            }

            string patchedFileHash = await _hashHelper.GetFileHashFromBytesAsync(patchedFileBytes);

            if (patchedFileHash != simplePatchData.ResultHash)
            {
                throw new InvalidOperationException("Result file hash was incorrect.");
            }

            await _fileSystem.File.WriteAllBytesAsync(patchedFilePath, patchedFileBytes);
        }

    }
}