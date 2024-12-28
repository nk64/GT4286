using GT4286Util.Helpers;

namespace GT4286Util
{
    public static class GenerationInfoHelper
    {
        public static List<GenerationInfo> GetGenerationInfoList()
        {
            using (var stream = ResourceReader.ReadManifestData("GT4286Util/Resources/GenerationInfo.json"))
            {
                var jsonstring = new StreamReader(stream).ReadToEnd();
                var generationInfoList = SerializationHelper.DeserializeJsonString<IEnumerable<GenerationInfo>>(jsonstring).ToList();
                return generationInfoList;
            }
        }

        public static GenerationInfo? FindMatchingGenerationInfo(GenerationInfo generationInfo)
        {
            var generationInfoList = GetGenerationInfoList();

            foreach (var gi in generationInfoList)
            {
                if (gi.IsEffectivelyTheSameAs(generationInfo))
                {
                    return gi;
                }
            }

            return null;
        }

    }

}