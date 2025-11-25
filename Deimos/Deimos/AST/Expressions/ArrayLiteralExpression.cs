#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Expressions
{
    public sealed class ArrayLiteralExpression : Expression
    {
        public ReadOnlyArray<Expression> Elements { get; }

        public ArrayLiteralExpression(IEnumerable<Expression> elements, TextRange range) : base(range)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public ArrayLiteralExpression(IEnumerable<Expression> elements, TextIndex from, TextIndex to) : base(from, to)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public ArrayLiteralExpression(IEnumerable<Expression> elements, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Elements)}]";
        }
    }
}
