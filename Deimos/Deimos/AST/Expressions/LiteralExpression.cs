#nullable enable

using Deimos.Lexer;
using System.Globalization;

namespace Deimos.AST.Expressions
{
    public sealed class LiteralExpression : Expression
    {
        public object? Value { get; }

        public LiteralExpression(object? value, TextRange range) : base(range)
        {
            Value = value;
        }

        public LiteralExpression(object? value, TextIndex from, TextIndex to) : base(from, to)
        {
            Value = value;
        }

        public LiteralExpression(object? value, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Value = value;
        }

        public override string ToString() => Value switch
        {
            null => "null",
            true => "true",
            false => "false",
            string s => StringToStringLiteral(s),
            double d => d.ToString(CultureInfo.InvariantCulture),
            float f => f.ToString(CultureInfo.InvariantCulture),
            int i => i.ToString(CultureInfo.InvariantCulture),
            long l => l.ToString(CultureInfo.InvariantCulture),
            short s => s.ToString(CultureInfo.InvariantCulture),
            _ => Value.ToString() ?? "null",
        };

        private static string EscapeString(string str)
        {
            return str
                .Replace("\\", "\\\\")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t")
                .Replace("\"", "\\\"");
        }

        private static string StringToStringLiteral(string str)
        {
            if (str.Contains('\n'))
                return $"\"\"\"{EscapeString(str)}\"\"\"";
            return $"\"{EscapeString(str)}\"";
        }
    }
}
