#nullable enable

using System;

namespace Deimos.Lexer
{
    public readonly struct TextRange : IEquatable<TextRange>, IComparable<TextRange>
    {
        /** Starting offset of the text section. Included */
        public TextIndex Start { get; }

        /** Ending offset of the text section. Included */
        public TextIndex End { get; }

        public TextRange(TextIndex start, TextIndex end)
        {
            Start = start;
            End = end;

            if (end < start)
                throw new ArgumentException("End offset must be after start offset.");
        }

        public static TextRange From(TextIndex start, TextIndex end) => new(start, end);
        public static TextRange From(int startLine, int startColumn, int endLine, int endColumn) =>
            new(TextIndex.Of(startLine, startColumn), TextIndex.Of(endLine, endColumn));

        public bool Contains(TextIndex index) => index >= Start && index <= End;

        public override string ToString()
        {
            if (Start == End)
                return Start.ToString();
            return $"{Start}-{End}";
        }

        public override bool Equals(object? obj) => obj is TextRange range && Equals(range);

        public bool Equals(TextRange other) => Start.Equals(other.Start) && End.Equals(other.End);

        public int CompareTo(TextRange other)
        {
            int startComparison = Start.CompareTo(other.Start);
            if (startComparison != 0)
                return startComparison;
            return End.CompareTo(other.End);
        }

        public override int GetHashCode() => HashCode.Combine(Start, End);

        public static bool operator ==(TextRange left, TextRange right) => left.Start == right.Start && left.End == right.End;
        public static bool operator !=(TextRange left, TextRange right) => left.Start != right.Start && left.End != right.End;

        public static bool operator <(TextRange left, TextRange right) => left.CompareTo(right) < 0;
        public static bool operator <=(TextRange left, TextRange right) => left.CompareTo(right) <= 0;  
        public static bool operator >(TextRange left, TextRange right) => left.CompareTo(right) > 0;
        public static bool operator >=(TextRange left, TextRange right) => left.CompareTo(right) >= 0;
    }
}
