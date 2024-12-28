using System.IO.Compression;
using System.Xml;
using GT4286Util.Helpers;

namespace GT4286Util
{
    public class FBNeoDatHelper()
    {
        private static XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Ignore,
        };

        public static Dictionary<string, (string description, string sanetised_description)> GetArcadeGameDescriptionDictionary()
        {
                var FBNeoGameDescriptionDictionary = FBNeoDatHelper.StreamGamesFromResource()
                    .ToDictionary(t => t.Item1, t => (description: t.Item2, sanetised_description: RetroSDCardManager.SanitiseDescription(t.Item2 ?? t.Item1)));
                return FBNeoGameDescriptionDictionary;
        }


        public static IEnumerable<Tuple<string, string>> StreamGamesFromArcadeDat(string uri)
        {
            using XmlReader xmlReader = XmlReader.Create(uri, xmlReaderSettings);
            foreach (var g in StreamGamesFromXmlReader(xmlReader)) { yield return g; }
        }

        public static IEnumerable<Tuple<string, string>> StreamGamesFromResource()
        {
            using var stream = ResourceReader.ReadManifestData("GT4286Util/Resources/dats/FBNeo_-_Arcade.dat.brotli");
            using var decompressor = new BrotliStream(stream, CompressionMode.Decompress);
            using var xmlReader = XmlReader.Create(decompressor, xmlReaderSettings);
            foreach (var g in StreamGamesFromXmlReader(xmlReader)) { yield return g; }
        }


        private static IEnumerable<Tuple<string, string>> StreamGamesFromXmlReader(XmlReader reader)
        {
            reader.MoveToContent();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element
                    && reader.Name == "game")
                {
                    string gameName = reader.GetAttribute("name")!;

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element &&
                            reader.Name == "description")
                        {
                            reader.Read();
                            string gameDescription = reader.Value;
                            yield return Tuple.Create(gameName, gameDescription);
                            break;
                        }
                    }
                }
            }
        }

    }
}