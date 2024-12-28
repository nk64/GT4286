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
    }
}