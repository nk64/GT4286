using System.Diagnostics.CodeAnalysis;
using Dumpify;
using Spectre.Console;

namespace GT4286Util.Helpers
{
    public static class DumpExtension
    {
        public static void Dump2<T>(this T o, string? title = null)
        {
            Dumpify.DumpExtensions.Dump<T>(o, title);
        }

        public static void Dump<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>(
            this IEnumerable<T> source,
            string? title = null
        )
        {
            Type type = typeof(T);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            // Create a table
            var table = new Table();
            if (string.IsNullOrWhiteSpace(title) == false) {
                table.Title = new TableTitle(title, Style.Plain.Decoration(Decoration.Underline).Foreground(Color.Red));
            }

            table.Expand();
            table.ShowRowSeparators();

            // Add some columns
            foreach(var prop in properties)
            {
                table.AddColumn(prop.Name);
            }

            // foreach(var c in table.Columns)
            // {
            //     c.NoWrap();
            // }

            foreach(T item in source)
            {
                var strings = properties.Select(f=>$"{f.GetValue(item)}".EscapeMarkup()).ToArray();
                table.AddRow(strings);
            }

            // Render the table to the console
            AnsiConsole.Write(table);
        }

    }
}


