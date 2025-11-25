#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Expressions
{
    public sealed class DictionaryLiteralExpression : Expression
    {
        public ReadOnlyArray<(Expression Key, Expression Value)> Entries { get; }

        public DictionaryLiteralExpression(IEnumerable<(Expression, Expression)> entries, TextRange range) : base(range)
        {
            Entries = entries?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(entries));
        }

        public DictionaryLiteralExpression(IEnumerable<(Expression, Expression)> entries, TextIndex from, TextIndex to) : base(from, to)
        {
            Entries = entries?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(entries));
        }

        public DictionaryLiteralExpression(IEnumerable<(Expression, Expression)> entries, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Entries = entries?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(entries));
        }

        public override string ToString()
        {
            if (Entries.Count == 0)
                return "{}";

            var entriesStrings = new List<string>();
            foreach (var (Key, Value) in Entries)
                entriesStrings.Add($"{Key}: {Value}");

            return $"{{{string.Join(", ", entriesStrings)}}}";
        }
    }
}
