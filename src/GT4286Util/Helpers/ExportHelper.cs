using CsvHelper;

namespace GT4286Util.Helpers
{
    public static class ExportHelper
    {
        public static void ExportToCsv<T>(this IEnumerable<T> input, string outputFilePath)
        {
            using (var csvWriter = new CsvWriter(new StreamWriter(outputFilePath), System.Globalization.CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(input);
            }
        }
    }
}