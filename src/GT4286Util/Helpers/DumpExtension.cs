using Spectre.Console;

namespace GT4286Util.Helpers
{
    public static class DumpExtension
    {
        public static void Dump<T>(this IEnumerable<T> source, string? title = null)
        {
            // Create a table
            var table = new Table();
            if (string.IsNullOrWhiteSpace(title) == false) {
                table.Title = new TableTitle(title, Style.Plain.Decoration(Decoration.Underline).Foreground(Color.Red));
            }

            table.Expand();
            table.ShowRowSeparators();

            var fields = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            // Add some columns
            foreach(var f in fields)
            {
                table.AddColumn(f.Name);
            }

            // foreach(var c in table.Columns)
            // {
            //     c.NoWrap();
            // }

            foreach(T item in source)
            {
                var strings = fields.Select(f=>$"{f.GetValue(item)}".EscapeMarkup()).ToArray();
                table.AddRow(strings);
            }


            // Render the table to the console
            AnsiConsole.Write(table);
        }
    }
}


