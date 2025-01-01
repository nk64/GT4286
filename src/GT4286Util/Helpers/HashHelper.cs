using System.IO.Abstractions;
using System.Security.Cryptography;

namespace GT4286Util.Helpers
{
    public class HashHelper
    {
        private readonly IFileSystem _fileSystem;
        public HashHelper(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string GetFileHash(string filePath)
        {
            using (FileSystemStream fileStream = _fileSystem.File.Open(filePath, FileMode.Open))
            {
                return GetFileHashFromStream(fileStream);
            }
        }

        private string GetFileHashFromStream(Stream inputStream)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(inputStream);
                return Convert.ToHexString(hashValue);
            }
        }


        public async Task<string> GetFileHashAsync(string filePath, CancellationToken cancellationToken = default)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4098, useAsync: true))
            {
                return await GetFileHashFromStreamAsync(fileStream, cancellationToken);
            }
        }

        public async Task<string> GetFileHashFromStreamAsync(Stream inputStream, CancellationToken cancellationToken = default)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = await mySHA256.ComputeHashAsync(inputStream, cancellationToken);
                return Convert.ToHexString(hashValue);
            }
        }

        public async Task<string> GetFileHashFromBytesAsync(byte[] inputBytes, CancellationToken cancellationToken = default)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                using (var memoryStream = new MemoryStream(inputBytes))
                {
                    byte[] hashValue = await mySHA256.ComputeHashAsync(memoryStream, cancellationToken);
                    return Convert.ToHexString(hashValue);
                }
            }
        }


    }
}