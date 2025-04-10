using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringDataSorter.Core.Comparer
{
    /// <summary>
    /// A custom comparer that compares two strings based on a specific format: "number. string"
    /// The comparison logic first compares the text part of the string, then the numeric part.
    /// </summary>
    public class StringDataComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            // Check if both references are the same or either is null
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            ReadOnlySpan<char> xSpan = x.AsSpan();
            ReadOnlySpan<char> ySpan = y.AsSpan();

            int xSepIndex = xSpan.IndexOf(". ");
            int ySepIndex = ySpan.IndexOf(". ");

            ReadOnlySpan<char> xNumberSpan = xSpan.Slice(0, xSepIndex);
            ReadOnlySpan<char> yNumberSpan = ySpan.Slice(0, ySepIndex);

            ReadOnlySpan<char> xTextSpan = xSpan.Slice(xSepIndex + 2);
            ReadOnlySpan<char> yTextSpan = ySpan.Slice(ySepIndex + 2);

            int textComparison = xTextSpan.CompareTo(yTextSpan, StringComparison.Ordinal);
            if (textComparison != 0)
                return textComparison;

            if (int.TryParse(xNumberSpan, out int xNum) && int.TryParse(yNumberSpan, out int yNum))
            {
                return xNum.CompareTo(yNum);
            }

            // If parsing the numeric part fails, fall back to a direct string compare
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}
