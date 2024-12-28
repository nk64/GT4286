using System.Reflection;

namespace GT4286Util.Helpers
{
    internal static class ResourceReader
    {
        public static Stream ReadManifestData(string resourceName)
        {
            if (resourceName == null)
            {
                throw new ArgumentNullException("resourceName");
            }

            Assembly assembly = typeof(ResourceReader).Assembly;
            resourceName = resourceName.Replace("/", ".");
            Stream? stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }

            return stream;
        }
    }
}

