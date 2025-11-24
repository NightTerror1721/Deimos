#nullable enable

using System;

namespace Deimos.Lexer
{
    public readonly struct TextIndex : IEquatable<TextIndex>, IComparable<TextIndex>
    {
        public static TextIndex Start { get; } = new(1, 1);

        public int Line { get; }
        public int Column { get; }

        public TextIndex(int line, int column)
        {
            Line = Math.Max(1, line);
            Column = Math.Max(1, column);
        }

        public static TextIndex Of(int line = 1, int column = 1) => new(line, column);

        public override string ToString()
        {
            return $"{Line}:{Column}";
        }

        public override bool Equals(object? obj) => obj is TextIndex offset && Equals(offset);

        public bool Equals(TextIndex other) => Line == other.Line && Column == other.Column;

        public override int GetHashCode() => HashCode.Combine(Line, Column);

        public int CompareTo(TextIndex other)
        {
            if (Line != other.Line)
                return Line.CompareTo(other.Line);
            return Column.CompareTo(other.Column);
        }

        public static bool operator ==(TextIndex left, TextIndex right) => left.Line == right.Line && left.Column == right.Column;
        public static bool operator !=(TextIndex left, TextIndex right) => left.Line != right.Line || left.Column != right.Column;

        public static bool operator <(TextIndex left, TextIndex right)
        {
            if (left.Line != right.Line)
                return left.Line < right.Line;
            return left.Column < right.Column;
        }
        public static bool operator <=(TextIndex left, TextIndex right)
        {
            if (left.Line != right.Line)
                return left.Line < right.Line;
            return left.Column <= right.Column;
        }
        public static bool operator >(TextIndex left, TextIndex right)
        {
            if (left.Line != right.Line)
                return left.Line > right.Line;
            return left.Column > right.Column;
        }
        public static bool operator >=(TextIndex left, TextIndex right)
        {
            if (left.Line != right.Line)
                return left.Line > right.Line;
            return left.Column >= right.Column;
        }
    }
}
