#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Expressions
{
    public sealed class NewObjectExpression : Expression
    {
        public TypeNode Type { get; }
        public ReadOnlyArray<Expression> Arguments { get; }

        public NewObjectExpression(
            TypeNode type,
            IEnumerable<Expression> arguments,
            TextRange range
        ) : base(range)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public NewObjectExpression(
            TypeNode type,
            IEnumerable<Expression> arguments,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public NewObjectExpression(
            TypeNode type,
            IEnumerable<Expression> arguments,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            Type = type ?? throw new System.ArgumentNullException(nameof(type));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public override string ToString()
        {
            if (Arguments.Count == 0)
                return $"new {Type}()";
            return $"new {Type}({string.Join(", ", Arguments)})";
        }
    }
}
