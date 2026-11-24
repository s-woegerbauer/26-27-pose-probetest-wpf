namespace Core.Pixel;

public static class PixelParser
{
    public const string TagsHeader = "#tags:";

    /// <summary>
    /// Parses the content of a picture file (see assignment: "Dateiinhalt").
    /// </summary>
    public static ParseResult Parse(string text)
    {
        var tags  = new List<string>();
        var runs  = new List<Run>();
        int row   = 0;
        int width = 0;

        foreach (var rawLine in text.Split('\n'))
        {
            var line = rawLine.Trim();
            if (line.Length == 0)
            {
                continue; // empty lines are ignored
            }

            if (line.StartsWith('#'))
            {
                if (line.StartsWith(TagsHeader, StringComparison.OrdinalIgnoreCase))
                {
                    tags.AddRange(line[TagsHeader.Length..]
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                }

                continue; // other '#' lines are comments
            }

            int column = 0, prefix = 0;
            foreach (char c in line)
            {
                if (char.IsAsciiDigit(c))
                {
                    prefix = prefix * 10 + (c - '0'); // multi digit prefix
                    continue;
                }

                int count = prefix == 0 ? 1 : prefix;
                if (c == '.')
                {
                    column += count; // transparent: no run
                }
                else if (char.IsAsciiLetterUpper(c))
                {
                    runs.Add(new Run(row, column, count, c));
                    column += count;
                }
                else
                {
                    continue; // unknown character: ignore
                }

                prefix = 0; // reset after each command
            }

            width = Math.Max(width, column);
            row++;
        }

        return new ParseResult(tags, runs, width, row);
    }
}
