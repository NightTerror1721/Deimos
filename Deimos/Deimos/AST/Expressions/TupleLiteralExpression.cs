#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Expressions
{
    public sealed class TupleLiteralExpression : Expression
    {
        public ReadOnlyArray<Expression> Elements { get; }

        public TupleLiteralExpression(IEnumerable<Expression> elements, TextRange range) : base(range)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public TupleLiteralExpression(IEnumerable<Expression> elements, TextIndex from, TextIndex to) : base(from, to)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public TupleLiteralExpression(IEnumerable<Expression> elements, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Elements = elements?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elements));
        }

        public override string ToString()
        {
            if (Elements.Count == 0)
                return "(,)";

            if (Elements.Count == 1)
                return $"({Elements[0]},)";

            return $"({string.Join(", ", Elements)})";
        }
    }
}
